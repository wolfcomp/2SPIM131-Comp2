using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public partial struct ShotSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        var entityManager = state.EntityManager;
        foreach (var (_, entity) in SystemAPI.Query<ShotComponent>().WithEntityAccess())
        {
            var shotComponent = entityManager.GetComponentData<ShotComponent>(entity);
            if (shotComponent.LivingTime > 10)
            {
                entityCommandBuffer.DestroyEntity(entity);
                continue;
            }
            var transform = entityManager.GetComponentData<LocalTransform>(entity);
            transform.Position += new float3(shotComponent.Velocity * SystemAPI.Time.DeltaTime, 0);
            transform.Rotation = quaternion.identity;
            shotComponent.LivingTime += SystemAPI.Time.DeltaTime;
            entityCommandBuffer.SetComponent(entity, transform);
            entityCommandBuffer.SetComponent(entity, shotComponent);
        }
        entityCommandBuffer.Playback(entityManager);
    }


}