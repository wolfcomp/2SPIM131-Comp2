using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;

public partial struct EnemyAISystem : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        var entityManager = state.EntityManager;
        var _playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();

        foreach (var (_enemyComponent, _transformComponent) in SystemAPI.Query<Enemy, RefRW<LocalTransform>>())
        {
            float3 _direction = entityManager.GetComponentData<LocalTransform>(_playerEntity).Position - _transformComponent.ValueRO.Position;
            float _angle = math.radians(90) + math.atan2(_direction.y, _direction.x);

            _transformComponent.ValueRW.Rotation = quaternion.Euler(new float3(0, 0, _angle));
            _transformComponent.ValueRW.Position += math.normalize(_direction) * SystemAPI.Time.DeltaTime;
        }
    }
}

