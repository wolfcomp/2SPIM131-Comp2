using System;
using Unity.Entities;
using UnityEngine;

public class InventoryContainerAuthoring : MonoBehaviour
{
    public InventoryType InventoryType;
    public int Size;
    public ItemComponentScriptableObject[] Items = Array.Empty<ItemComponentScriptableObject>();

    // ReSharper disable once UnusedType.Local
    private class Baker : Baker<InventoryContainerAuthoring>
    {
        public override unsafe void Bake(InventoryContainerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            var container = InventoryContainerComponent.CreateDefault(authoring.InventoryType, authoring.Size);
            for (var i = 0; i < Math.Min(authoring.Items.Length, authoring.Size); i++)
            {
                container.Items[i] = authoring.Items[i];
            }
            AddComponent(entity, container);
        }
    }
}
