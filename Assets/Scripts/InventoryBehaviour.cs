using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class InventoryBehaviour : MonoBehaviour
{
    class Baker : Baker<InventoryBehaviour>
    {
        public override unsafe void Bake(InventoryBehaviour authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var containerComponent = InventoryContainerComponent.CreateNewArray(2);
            var containerSpan = new Span<InventoryContainerComponent>(containerComponent, 2);
            containerSpan[0].InventoryType = InventoryType.Hotbar;
            containerSpan[1].InventoryType = InventoryType.Bag1;
            containerSpan[0].Items = InventoryItemComponent.CreateNewArray(10);
            containerSpan[0].ItemsCount = 10;
            containerSpan[1].Items = InventoryItemComponent.CreateNewArray(45);
            containerSpan[1].ItemsCount = 45;
            AddComponent(entity, new InventoryComponent
            {
                ContainerCount = 2,
                Containers = containerComponent 
            });
        }
    }
}
