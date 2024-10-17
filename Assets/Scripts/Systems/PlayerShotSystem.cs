using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
public partial struct PlayerShotSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingletonEntity<InputComponent>(out var inputEntity) || 
            !SystemAPI.TryGetSingletonEntity<PlayerComponent>(out var playerEntity))
            return;

        var entityManager = state.EntityManager;
        var inputComponent = SystemAPI.GetComponentRO<InputComponent>(inputEntity);
        var playerComponent = entityManager.GetComponentData<PlayerComponent>(playerEntity);
        if (UpdateDeltas(ref state, ref inputEntity, ref playerEntity) || 
            !inputComponent.IsValid || 
            !inputComponent.ValueRO.IsShooting)
            return;

        var mousePosition = Camera.allCameras[0].ScreenToWorldPoint(UnityEngine.Input.mousePosition with { z = 0 });
        var vector = GetVector(ref state, mousePosition, ref playerEntity);
        SpawnShot(ref state, vector, 0, ref playerEntity);
        if (playerComponent.IsRapid)
        {
            SpawnShot(ref state, vector, math.radians(17), ref playerEntity);
            SpawnShot(ref state, vector, math.radians(-17), ref playerEntity);
        }

        playerComponent.ShotDelta = playerComponent.ShotCooldown;
        entityManager.SetComponentData(playerEntity, playerComponent);
    }

    [BurstCompile]
    private bool UpdateDeltas(ref SystemState state, ref Entity inputEntity, ref Entity playerEntity)
    {
        var entityManager = state.EntityManager;
        var inputComponent = SystemAPI.GetComponentRO<InputComponent>(inputEntity);
        var playerComponent = entityManager.GetComponentData<PlayerComponent>(playerEntity);
        var updated = false;
        if (playerComponent.ShotDelta > 0)
        {
            playerComponent.ShotDelta -= SystemAPI.Time.DeltaTime;
            updated = true;
        }
        if (playerComponent.InvulnDelta > 0)
        {
            playerComponent.InvulnDelta -= SystemAPI.Time.DeltaTime;
            updated = true;
        }
        if (inputComponent.ValueRO.RapidMode)
        {
            playerComponent.IsRapid = !playerComponent.IsRapid;
            playerComponent.ShotDelta *= playerComponent.IsRapid ? 0.1f : 10;
            playerComponent.ShotCooldown *= playerComponent.IsRapid ? 0.1f : 10;
            updated = true;
        }
        if (updated)
        {
            entityManager.SetComponentData(playerEntity, playerComponent);
        }

        return updated;
    }

    [BurstCompile]
    private float3 GetVector(ref SystemState state, float3 cameraPos, ref Entity playerEntity)
    {
        var playerPos = SystemAPI.GetComponentRO<LocalTransform>(playerEntity).ValueRO.Position;
        return math.normalize(cameraPos - playerPos);
    }

    [BurstCompile]
    private void SpawnShot(ref SystemState state, float3 vector, float angle, ref Entity playerEntity)
    {
        var entityManager = state.EntityManager;
        var playerComponent = entityManager.GetComponentData<PlayerComponent>(playerEntity);
        var playerPos = entityManager.GetComponentData<LocalTransform>(playerEntity).Position;
        var shot = entityManager.Instantiate(playerComponent.ShotPrefab);
        var shotComponent = entityManager.GetComponentData<ShotComponent>(shot);
        var transform = entityManager.GetComponentData<LocalTransform>(shot);
        transform.Position = playerPos;
        shotComponent.Velocity = math.rotate(quaternion.AxisAngle(new float3(0,0,1), angle), vector * shotComponent.Speed).xy;
        playerComponent.ShotDelta = playerComponent.ShotCooldown;
        entityManager.SetComponentData(shot, shotComponent);
        entityManager.SetComponentData(shot, transform);
    }
}
