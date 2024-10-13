using System;
using System.Runtime.InteropServices;
using Unity.Entities;

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

    public static implicit operator InventoryItemComponent(ItemComponentScriptableObject v)
    {
        return new InventoryItemComponent
        {
            ItemId = v.ItemId,
            Count = v.Count,
            Name = new Utf16StringComponent(v.Name)
        };
    }
}