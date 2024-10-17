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
        if (!SystemAPI.TryGetSingleton(out InputComponent _inputComponent))
            EntityManager.CreateEntity(typeof(InputComponent));
        _input = new PlayerInputAction();
        _input.Enable();
    }

    protected override void OnUpdate()
    {
        var mVector = _input.Player.Move.ReadValue<Vector2>();
        var inventory = _input.Player.Inventory.WasPressedThisFrame();
        var didInteract = _input.Player.Interact.WasPressedThisFrame();
        var isInteracting = _input.Player.Interact.IsPressed();
        var isShooting = _input.Player.Shoot.IsPressed();
        var rapidMode = _input.Player.RapidMode.WasPerformedThisFrame();
        SystemAPI.SetSingleton(new InputComponent
        {
            MVector = (float2)mVector,
            DidInteract = didInteract,
            Inventory = inventory,
            IsInteracting = isInteracting,
            IsShooting = isShooting,
            RapidMode = rapidMode
        });
    }
    
}
