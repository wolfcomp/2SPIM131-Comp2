using System;
using Unity.Assertions;
using Unity.Entities;
using Unity.Physics.Stateful;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(StatefulTriggerEventBufferSystem))]
public partial class PickupSystem : SystemBase
{
    private EndFixedStepSimulationEntityCommandBufferSystem _commandBufferSystem;
    private EntityQuery _nonTriggerQuery;
    private EntityQueryMask _nonTriggerMask;

    protected override void OnCreate()
    {
        _commandBufferSystem = World.GetOrCreateSystemManaged<EndFixedStepSimulationEntityCommandBufferSystem>();
        _nonTriggerQuery = GetEntityQuery(new EntityQueryDesc
        {
            None = new ComponentType[]
            {
                typeof(StatefulTriggerEvent),
            }
        });
        Assert.IsFalse(_nonTriggerQuery.HasFilter(), "The use of EntityQueryMask in this system will not respect the query's active filter settings.");
        _nonTriggerMask = _nonTriggerQuery.GetEntityQueryMask();

        RequireForUpdate<Loot>();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = _commandBufferSystem.CreateCommandBuffer();

        // capture variable for foreach loop
        var nonTriggerMask = _nonTriggerMask;

        var playerLookup = GetComponentLookup<PlayerComponent>();

        foreach (var (eventBuffer, _, entity) in SystemAPI.Query<DynamicBuffer<StatefulTriggerEvent>, RefRO<Loot>>().WithEntityAccess())
        {
            foreach (var triggerEvent in eventBuffer)
            {
                var otherEntity = triggerEvent.GetOtherEntity(entity);
                if (triggerEvent.State == StatefulEventState.Stay || !nonTriggerMask.MatchesIgnoreFilter(otherEntity))
                    continue;

                var playerInfo = playerLookup[otherEntity];

                playerInfo.CanPickup = triggerEvent.State == StatefulEventState.Enter;

                commandBuffer.SetComponent(otherEntity, playerInfo);
            }
        }
    }
}
