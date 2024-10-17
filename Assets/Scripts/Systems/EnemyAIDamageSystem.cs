using Unity.Assertions;
using Unity.Entities;
using Unity.Physics.Stateful;
using Unity.Physics.Systems;
using UnityEditor;
using UnityEngine;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(StatefulTriggerEventBufferSystem))]
[UpdateAfter(typeof(PlayerShotSystem))]
public partial class EnemyAIDamageSystem : SystemBase
{
    private EndFixedStepSimulationEntityCommandBufferSystem _commandBufferSystem;
    private EntityQuery _nonTriggerPlayerQuery;
    private EntityQueryMask _nonTriggerPlayerMask;
    private EntityQuery _nonTriggerShotQuery;
    private EntityQueryMask _nonTriggerShotMask;

    protected override void OnCreate()
    {
        _commandBufferSystem = World.GetOrCreateSystemManaged<EndFixedStepSimulationEntityCommandBufferSystem>();
        _nonTriggerPlayerQuery = GetEntityQuery(new EntityQueryDesc
        {
            Any = new ComponentType[]
            {
                typeof(PlayerComponent),
            }
        });
        Assert.IsFalse(_nonTriggerPlayerQuery.HasFilter(), "The use of EntityQueryMask in this system will not respect the query's active filter settings.");
        _nonTriggerPlayerMask = _nonTriggerPlayerQuery.GetEntityQueryMask();
        _nonTriggerShotQuery = GetEntityQuery(new EntityQueryDesc
        {
            Any = new ComponentType[]
            {
                typeof(ShotComponent),
            }
        });
        Assert.IsFalse(_nonTriggerShotQuery.HasFilter(), "The use of EntityQueryMask in this system will not respect the query's active filter settings.");
        _nonTriggerShotMask = _nonTriggerShotQuery.GetEntityQueryMask();
    }

    protected override void OnUpdate()
    {
        var commandBuffer = _commandBufferSystem.CreateCommandBuffer();

        // capture variable for foreach loop
        var nonTriggerPlayerMask = _nonTriggerPlayerMask;
        var nonTriggerShotMask = _nonTriggerShotMask;

        var playerHealthLookup = GetComponentLookup<HealthComponent>();
        var playerLookup = GetComponentLookup<PlayerComponent>();
        var enemyLookup = GetComponentLookup<EnemyData>();

        foreach (var (eventBuffer, _, entity) in SystemAPI.Query<DynamicBuffer<StatefulTriggerEvent>, RefRO<Enemy>>().WithEntityAccess())
        {
            foreach (var triggerEvent in eventBuffer)
            {
                var otherEntity = triggerEvent.GetOtherEntity(entity);
                if (!nonTriggerPlayerMask.MatchesIgnoreFilter(otherEntity))
                    continue;

                var playerInfo = playerLookup[otherEntity];
                if (playerInfo.InvulnDelta > 0)
                {
                    return;
                }

                var playerHealthInfo = playerHealthLookup[otherEntity];

                playerHealthInfo.Health -= enemyLookup[entity].Damage;
                if (playerHealthInfo.Health == 0)
                {
#if !UNITY_EDITOR
                    Application.Quit();
#else
                    EditorApplication.ExitPlaymode();
#endif

                    return;
                }
                playerInfo.InvulnDelta = 0.7f;

                commandBuffer.SetComponent(otherEntity, playerHealthInfo);
                commandBuffer.SetComponent(otherEntity, playerInfo);
            }
        }

        foreach (var (eventBuffer, _, entity) in SystemAPI.Query<DynamicBuffer<StatefulTriggerEvent>, RefRO<Enemy>>().WithEntityAccess())
        {
            foreach (var triggerEvent in eventBuffer)
            {
                var otherEntity = triggerEvent.GetOtherEntity(entity);
                if (!nonTriggerShotMask.MatchesIgnoreFilter(otherEntity))
                    continue;

                var enemyInfo = enemyLookup[entity];

                enemyInfo.Health -= 1;
                if (enemyInfo.Health == 0)
                    commandBuffer.DestroyEntity(entity);
                else
                    commandBuffer.SetComponent(entity, enemyInfo);
                commandBuffer.DestroyEntity(otherEntity);
            }
        }
    }
}
