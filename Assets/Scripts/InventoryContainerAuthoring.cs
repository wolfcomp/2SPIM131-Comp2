using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class InventoryContainerAuthoring : MonoBehaviour
{
    public InventoryType InventoryType;
    public int Size;

    // ReSharper disable once UnusedType.Local
    private class Baker : Baker<InventoryContainerAuthoring>
    {
        public override void Bake(InventoryContainerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, InventoryContainerComponent.CreateDefault(authoring.InventoryType, authoring.Size));
        }
    }
}
