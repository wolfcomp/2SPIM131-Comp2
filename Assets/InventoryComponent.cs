using System;
using System.Runtime.InteropServices;
using Unity.Entities;

[ChunkSerializable] 
public unsafe struct InventoryComponent : IComponentData
{
    public InventoryContainerComponent* Containers;
    public uint ContainerCount;
}

[ChunkSerializable] 
public unsafe struct InventoryContainerComponent : IComponentData
{
    public InventoryType InventoryType;
    public InventoryItemComponent* Items;
    public uint ItemsCount;

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
}

public struct InventoryItemComponent : IComponentData
{
    public uint ItemId;
    public uint Count;

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

}