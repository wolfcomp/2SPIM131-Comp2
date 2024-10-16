using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
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
        Interact(ref state);
        Inventory(ref state);
        

    }
    [BurstCompile]
    private void Move(ref SystemState state)
    {
        var _physicsV = SystemAPI.GetComponentRW<PhysicsVelocity>(_playerEntity);
        _physicsV.ValueRW.Linear += new float3(_inputComponent.MVector * _movementComponent.MoveSpeed 
                                                                       * _movementComponent.MoveMultiplier * SystemAPI.Time.DeltaTime, 0);
    }

    private void Interact(ref SystemState state)
    {
        
    }

    private void Inventory(ref SystemState state)
    {
        
    }
}
