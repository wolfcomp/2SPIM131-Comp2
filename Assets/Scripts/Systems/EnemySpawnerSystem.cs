using Unity.Entities;
using Random = Unity.Mathematics.Random;
using Unity.Mathematics;
using Unity.Transforms;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemySpawnerSystem : SystemBase
{
    private EnemySpawnerComponent _enemySpawnerComponent;
    private EnemyContainerComponent _enemyContainerComponent;
    private Entity _enemySpawnerEntity;
    private float _nextSpawnTime;
    private Random _random;
    private bool _isRapid;

    protected override void OnCreate()
    {
        _random = Random.CreateFromIndex((uint)_enemySpawnerComponent.GetHashCode());
    }

    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingletonEntity<EnemySpawnerComponent>(out _enemySpawnerEntity))
        {
            return;
        }

        _enemySpawnerComponent = EntityManager.GetComponentData<EnemySpawnerComponent>(_enemySpawnerEntity);
        _enemyContainerComponent = EntityManager.GetComponentObject<EnemyContainerComponent>(_enemySpawnerEntity);

        if (SystemAPI.TryGetSingletonEntity<InputComponent>(out var inputComponentEntity))
        {
            var inputComponent = EntityManager.GetComponentData<InputComponent>(inputComponentEntity);
            if (inputComponent.RapidMode)
            {
                _isRapid = !_isRapid;
                _enemySpawnerComponent.SpawnCooldown *= _isRapid ? 0.1f : 10;
                _nextSpawnTime *= _isRapid ? 0.1f : 10;
                EntityManager.SetComponentData(_enemySpawnerEntity, _enemySpawnerComponent);
            }
        }

        if (SystemAPI.Time.ElapsedTime > _nextSpawnTime)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var availableEnemies = new List<EnemyData>();

        foreach (var enemyData in _enemyContainerComponent.Enemies)
        {
            availableEnemies.Add(enemyData);
        }

        var index = _random.NextInt(availableEnemies.Count);

        var newEnemy = EntityManager.Instantiate(availableEnemies[index].Prefab);
        EntityManager.SetComponentData(newEnemy, new LocalTransform
        {
            Position = GetPositionOutsideOfCameraRange(),
            Rotation = quaternion.identity,
            Scale = 0.8f
        });
        EntityManager.AddComponentData(newEnemy, availableEnemies[index]);

        EntityManager.AddComponentData(newEnemy, new Enemy());
        EntityManager.AddComponentData(newEnemy, new HealthComponent { Health = availableEnemies[index].Health, MaxHealth = availableEnemies[index].Health });

        _nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + _enemySpawnerComponent.SpawnCooldown;
    }

    private float3 GetPositionOutsideOfCameraRange()
    {
        var size = _enemySpawnerComponent.CameraSize * 2;
        var position = new float3(UnityEngine.Random.Range(-size.x, size.x), UnityEngine.Random.Range(-size.y, size.y), 0);

        while (position.x < _enemySpawnerComponent.CameraSize.x && position.x > -_enemySpawnerComponent.CameraSize.x
            && position.y < _enemySpawnerComponent.CameraSize.y && position.y > -_enemySpawnerComponent.CameraSize.y)
        {
            position = new float3(UnityEngine.Random.Range(-size.x, size.x), UnityEngine.Random.Range(-size.y, size.y), 0);
        }

        position += new float3(Camera.allCameras[0].transform.position.x, Camera.allCameras[0].transform.position.y, 0);

        return position;
    }
}