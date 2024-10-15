using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

public partial class HotbarUIInventorySystem : SystemBase
{
    private HotbarUIBehaviour _hotbarCanvas;

    protected override void OnCreate()
    {
        RequireForUpdate<Player>();
    }

    protected override void OnUpdate()
    {
        var player = SystemAPI.GetSingletonEntity<Player>();
        var inventoryComponent = SystemAPI.GetComponentRO<InventoryComponent>(player);
    }

    public void AttachUI(HotbarUIBehaviour hotbarCanvas)
    {
        _hotbarCanvas = hotbarCanvas;
    }
}
