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
        
        LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);
        Move(ref state, ref playerTransform);
        Interact(ref state);
        Inventory(ref state);
        

    }

    [BurstCompile]
    private void Move(ref SystemState state, ref LocalTransform playerTransform)
    {
        
        playerTransform.Position += new float3(_inputComponent.MVector * _movementComponent.MoveSpeed * SystemAPI.Time.DeltaTime, 0);
        
        //physics
        _physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        NativeList<ColliderCastHit> _hits = new NativeList<ColliderCastHit>(Allocator.Temp);
        float3 point1 = new float3(playerTransform.Position - playerTransform.Right() * 0.15f);
        float3 point2 = new float3(playerTransform.Position + playerTransform.Right() * 0.15f);

        _physicsWorld.CapsuleCastAll(point1, point2, _movementComponent.Height / 2, float3.zero, 1f, ref _hits, new CollisionFilter
        {
            BelongsTo = (uint)CollisionLayers.Player,
            CollidesWith = (uint)CollisionLayers.Water
        });
        
        _entityManager.SetComponentData(_playerEntity, playerTransform);
        _hits.Dispose();
    }

    private void Interact(ref SystemState state)
    {
        
    }

    private void Inventory(ref SystemState state)
    {
        
    }
}
