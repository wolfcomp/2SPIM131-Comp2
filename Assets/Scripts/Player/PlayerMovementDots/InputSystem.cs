using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    private PlayerInputAction _Input;
    protected override void OnCreate()
    {
        if (!SystemAPI.TryGetSingleton(out InputComponent _inputComponent))
            EntityManager.CreateEntity(typeof(InputComponent));
        _Input = new PlayerInputAction();
        _Input.Enable();
    }

    protected override void OnUpdate()
    {
        Vector2 mVector = _Input.Player.Move.ReadValue<Vector2>();
        bool inventory = _Input.Player.Inventory.WasPressedThisFrame();
        bool didInteract = _Input.Player.Interact.WasPressedThisFrame();
        bool isInteracting = _Input.Player.Interact.IsPressed();
        bool isShooting = _Input.Player.Shoot.IsPressed();
        SystemAPI.SetSingleton(new InputComponent
        {
            MVector = (float2)mVector,
            DidInteract = didInteract,
            Inventory = inventory,
            IsInteracting = isInteracting,
            IsShooting = isShooting
        });
    }
    
}
