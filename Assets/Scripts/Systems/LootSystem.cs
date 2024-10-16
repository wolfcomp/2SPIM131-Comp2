using System;
using Unity.Entities;

[UpdateAfter(typeof(InputSystem))]
public partial class LootSystem : SystemBase
{
    private EntityCommandBufferSystem _commandBufferSystem;
    
    protected override void OnCreate()
    {
        _commandBufferSystem = World.GetOrCreateSystemManaged<EntityCommandBufferSystem>();
        RequireForUpdate<InputComponent>();
        RequireForUpdate<Player>();
    }

    protected override void OnUpdate()
    {
        var inputEntity = SystemAPI.GetSingletonEntity<InputComponent>();
        var inputComponent = SystemAPI.GetComponentRO<InputComponent>(inputEntity);
        if (!inputComponent.IsValid || !inputComponent.ValueRO.DidInteract) return;
        var playerEntity = SystemAPI.GetSingletonEntity<Player>();
        var inventoryComponent = SystemAPI.GetComponentRW<InventoryComponent>(playerEntity);
        foreach (var (lootContainer, loot, lootEntity) in SystemAPI.Query<RefRO<InventoryContainerComponent>, RefRO<LootComponent>>().WithEntityAccess())
        {
            if(!loot.ValueRO.IsInPickupRange) continue;
            foreach (var _inventoryItemComponent in lootContainer.ValueRO.ItemsSpan)
            {
                inventoryComponent.ValueRW.AddItem(_inventoryItemComponent);
            }
            _commandBufferSystem.CreateCommandBuffer().DestroyEntity(lootEntity);
        }
    }
}