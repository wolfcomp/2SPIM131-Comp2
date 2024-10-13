using System;
using System.Runtime.InteropServices;
using Unity.Entities;

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