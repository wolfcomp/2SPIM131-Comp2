using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerShotSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<InputComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var entityManager = state.EntityManager;
        var inputEntity = SystemAPI.GetSingletonEntity<InputComponent>();
        var inputComponent = SystemAPI.GetComponentRO<InputComponent>(inputEntity);
        var playerEntity = SystemAPI.GetSingletonEntity<Player>();
        var playerComponent = entityManager.GetComponentData<PlayerComponent>(playerEntity);
        var newCooldown = playerComponent.ShotDelta - SystemAPI.Time.DeltaTime;
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        if (newCooldown > 0)
        {
            Debug.Log($"Shot on cooldown: {newCooldown}");
            playerComponent.ShotDelta = newCooldown;
            entityCommandBuffer.SetComponent(playerEntity, playerComponent);
            entityCommandBuffer.Playback(entityManager);
            return;
        }
        if (!inputComponent.IsValid || !inputComponent.ValueRO.IsShooting) return;
        var mousePosition = Camera.allCameras[0].ScreenToWorldPoint(UnityEngine.Input.mousePosition with { z = 0 });
        var playerPos = SystemAPI.GetComponentRO<LocalTransform>(playerEntity).ValueRO.Position;
        var shot = entityManager.Instantiate(playerComponent.ShotPrefab);
        var shotComponent = entityManager.GetComponentData<ShotComponent>(shot);
        var vector = (float3)(mousePosition - (Vector3)playerPos).normalized * shotComponent.Speed;
        var transform = entityManager.GetComponentData<LocalTransform>(shot);
        transform.Position = playerPos;
        shotComponent.Velocity = vector.xy;
        playerComponent.ShotDelta = playerComponent.ShotCooldown;
        entityCommandBuffer.SetComponent(shot, shotComponent);
        entityCommandBuffer.SetComponent(shot, transform);
        entityCommandBuffer.SetComponent(playerEntity, playerComponent);
        entityCommandBuffer.Playback(entityManager);
    }
}
