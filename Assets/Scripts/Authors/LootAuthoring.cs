using System;
using Unity.Entities;
using Unity.Physics.Stateful;
using UnityEngine;

public class LootAuthoring : MonoBehaviour
{
    public InventoryType InventoryType = InventoryType.Loot;
    public int Size = 10;
    public ItemComponentScriptableObject[] Items = Array.Empty<ItemComponentScriptableObject>();

    class Baker : Baker<LootAuthoring>
    {
        public override unsafe void Bake(LootAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Loot());
            var container = InventoryContainerComponent.CreateDefault(authoring.InventoryType, authoring.Size);
            for (var i = 0; i < Math.Min(authoring.Items.Length, authoring.Size); i++)
            {
                container.Items[i] = authoring.Items[i];
            }
            AddComponent(entity, container);
            AddBuffer<StatefulTriggerEvent>(entity);
        }
    }
}