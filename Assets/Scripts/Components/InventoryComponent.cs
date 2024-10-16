using System;
using Unity.Entities;

[ChunkSerializable]
public unsafe struct InventoryComponent : IComponentData
{
    public InventoryContainerComponent* Containers;
    public int ContainerCount;

    public Span<InventoryContainerComponent> ContainersSpan => new(Containers, ContainerCount);

    public static InventoryComponent CreateDefault()
    {
        var containerComponent = InventoryContainerComponent.CreateNewArray(2);
        var containerSpan = new Span<InventoryContainerComponent>(containerComponent, 2);
        containerSpan[0].InventoryType = InventoryType.Hotbar;
        containerSpan[1].InventoryType = InventoryType.Bag1;
        containerSpan[0].Items = InventoryItemComponent.CreateNewArray(10);
        containerSpan[0].ItemsCount = 10;
        containerSpan[1].Items = InventoryItemComponent.CreateNewArray(45);
        containerSpan[1].ItemsCount = 45;
        return new InventoryComponent
        {
            ContainerCount = 2,
            Containers = containerComponent
        };
    }

    public void AddItem(InventoryItemComponent item)
    {
        foreach (var container in ContainersSpan)
        {
            for (var j = 0; j < container.ItemsSpan.Length; j++)
            {
                if ((container.ItemsSpan[j].ItemId != 0 && container.ItemsSpan[j].ItemId != item.ItemId) || item.ItemId == 0) continue;
                container.ItemsSpan[j].ItemId = item.ItemId;
                container.ItemsSpan[j].Count += item.Count;
                container.ItemsSpan[j].SpriteIndex = item.SpriteIndex;
                container.ItemsSpan[j].Name = item.Name;
                return;
            }
        }
    }
}

public enum InventoryType : uint
{
    Invalid = 0,
    Hotbar = 100,

    Bag1 = 1000,
    Bag2 = 1001,
    Bag3 = 1002,
    Bag4 = 1003,

    Loot = 2000
}