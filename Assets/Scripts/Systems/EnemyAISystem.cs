using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;

public partial struct EnemyAISystem : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        var entityManager = state.EntityManager;
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();

        foreach (var (_, transformComponent) in SystemAPI.Query<Enemy, RefRW<LocalTransform>>())
        {
            var direction = entityManager.GetComponentData<LocalTransform>(playerEntity).Position - transformComponent.ValueRO.Position;
            var angle = math.radians(90) + math.atan2(direction.y, direction.x);

            transformComponent.ValueRW.Rotation = quaternion.Euler(new float3(0, 0, angle));
            transformComponent.ValueRW.Position += math.normalize(direction) * SystemAPI.Time.DeltaTime;
        }
    }
}

