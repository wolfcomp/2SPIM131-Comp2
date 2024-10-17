using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;

public partial struct MovementSystem : ISystem
{
    private EntityManager _entityManager;
    private Entity _playerEntity;
    private Entity _inputEntity;

    private MovementComponent _movementComponent;
    private InputComponent _inputComponent;

    private PhysicsWorldSingleton _physicsWorld;

    private void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MovementComponent>();
    }

    private void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;
        _playerEntity = SystemAPI.GetSingletonEntity<MovementComponent>();
        _inputEntity = SystemAPI.GetSingletonEntity<InputComponent>();

        _movementComponent = _entityManager.GetComponentData<MovementComponent>(_playerEntity);
        _inputComponent = _entityManager.GetComponentData<InputComponent>(_inputEntity);

        Move(ref state);
        Drag(ref state);
        Interact(ref state);
        Inventory(ref state);
    }

    [BurstCompile]
    private void Move(ref SystemState state)
    {
        var physicsVelocity = SystemAPI.GetComponentRW<PhysicsVelocity>(_playerEntity);
        // reset these to be 0 z due to DOTS only working in 3d
        physicsVelocity.ValueRW.Linear = new float3(physicsVelocity.ValueRW.Linear.xy, 0);
        var transform = SystemAPI.GetComponentRW<LocalTransform>(_playerEntity);
        transform.ValueRW.Position = new float3(transform.ValueRW.Position.xy, 0);
        if (!_movementComponent.EasterEggMode)
            transform.ValueRW.Rotation = Quaternion.identity;
        // update velocity
        physicsVelocity.ValueRW.Linear += new float3(_movementComponent.MoveMultiplier * _movementComponent.MoveSpeed * SystemAPI.Time.DeltaTime * _inputComponent.MovementVector, 0);
    }

    [BurstCompile]
    private void Drag(ref SystemState state)
    {
        var physicsVelocity = SystemAPI.GetComponentRW<PhysicsVelocity>(_playerEntity);
        physicsVelocity.ValueRW.Linear -= SystemAPI.Time.DeltaTime * _movementComponent.Drag * float4x4.identity.InverseTransformDirection(physicsVelocity.ValueRO.Linear);
    }

    private void Interact(ref SystemState state)
    {

    }

    private void Inventory(ref SystemState state)
    {

    }
}
