using System;
using System.Runtime.InteropServices;
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
}

[ChunkSerializable] 
public unsafe struct InventoryContainerComponent : IComponentData
{
    public InventoryType InventoryType;
    public InventoryItemComponent* Items;
    public int ItemsCount;

    public Span<InventoryItemComponent> ItemsSpan => new(Items, ItemsCount);

    public static InventoryContainerComponent* CreateNewArray(int size)
    {
        var ret = (InventoryContainerComponent*)Marshal.AllocHGlobal(sizeof(InventoryContainerComponent) * size);
        var span = new Span<InventoryContainerComponent>(ret, size);
        for (var _index = 0; _index < span.Length; _index++)
        {
            span[_index].ItemsCount = 0;
            span[_index].InventoryType = InventoryType.Invalid;
            span[_index].Items = null;
        }

        return ret;
    }

    public static InventoryContainerComponent CreateDefault(InventoryType type, int size) => new InventoryContainerComponent
    {
        InventoryType = type,
        Items = InventoryItemComponent.CreateNewArray(size),
        ItemsCount = size
    };
}

public struct InventoryItemComponent : IComponentData
{
    public uint ItemId;
    public uint Count;
    public Utf16StringComponent Name;

    public static unsafe InventoryItemComponent* CreateNewArray(int size)
    {
        var ret = (InventoryItemComponent*)Marshal.AllocHGlobal(sizeof(InventoryItemComponent) * size);
        var span = new Span<InventoryItemComponent>(ret, size);
        for (var _index = 0; _index < span.Length; _index++)
        {
            span[_index].Count = span[_index].ItemId = 0;
        }

        return ret;
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