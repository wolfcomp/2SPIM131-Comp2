using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateBefore(typeof(PlayerShotSystem))]
public partial class InputSystem : SystemBase
{
    private PlayerInputAction _input;

    protected override void OnCreate()
    {
        _input = new PlayerInputAction();
        _input.Enable();
    }

    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingletonEntity<InputComponent>(out var inputComponentEntity))
            return;
        var inputComponent = EntityManager.GetComponentData<InputComponent>(inputComponentEntity);
        var movementVector = _input.Player.Move.ReadValue<Vector2>();
        var inventory = _input.Player.Inventory.WasPressedThisFrame();
        var didInteract = _input.Player.Interact.WasPressedThisFrame();
        var rapidMode = _input.Player.RapidMode.WasPressedThisFrame();
        var isInteracting = _input.Player.Interact.IsPressed();
        var isShooting = _input.Player.Shoot.IsPressed();
        inputComponent.DidInteract = didInteract;
        inputComponent.IsInteracting = isInteracting;
        inputComponent.IsShooting = isShooting;
        inputComponent.RapidMode = rapidMode;
        inputComponent.MovementVector = movementVector;
        inputComponent.Inventory = inventory;
        EntityManager.SetComponentData(inputComponentEntity, inputComponent);
    }
    
}
