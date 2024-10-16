using Unity.Entities;
using Random = Unity.Mathematics.Random;
using Unity.Mathematics;
using Unity.Transforms;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemySpawnerSystem : SystemBase
{
    private EnemySpawnerComponent enemySpawnerComponent;
    private EnemyContainerComponent enemyContainerComponent;
    private Entity enemySpawnerEntity;
    private float NextSpawnTime;
    private EndFixedStepSimulationEntityCommandBufferSystem _commandBufferSystem;
    private Random random;

    protected override void OnCreate()
    {

        _commandBufferSystem = World.GetOrCreateSystemManaged<EndFixedStepSimulationEntityCommandBufferSystem>();
        random = Random.CreateFromIndex((uint)enemySpawnerComponent.GetHashCode());
        //random = Random.CreateFromIndex((uint)SystemAPI.Time.ElapsedTime.GetHashCode());

    }

    protected override void OnUpdate()
    {
        if (!SystemAPI.TryGetSingletonEntity<EnemySpawnerComponent>(out enemySpawnerEntity))
        {
            return;
        }

        enemySpawnerComponent = EntityManager.GetComponentData<EnemySpawnerComponent>(enemySpawnerEntity);
        enemyContainerComponent = EntityManager.GetComponentObject<EnemyContainerComponent>(enemySpawnerEntity);

        if (SystemAPI.Time.ElapsedTime > NextSpawnTime)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        List<EnemyData> availableEnemies = new List<EnemyData>();

        foreach (EnemyData enemyData in enemyContainerComponent.enemies)
        {
            availableEnemies.Add(enemyData);
        }

        int index = random.NextInt(availableEnemies.Count);

        Entity newEnemy = EntityManager.Instantiate(availableEnemies[index].Prefab);
        EntityManager.SetComponentData(newEnemy, new LocalTransform
        {
            Position = GetPositionOutsideOfCameraRange(),
            Rotation = quaternion.identity,
            Scale = 1
        });


        NextSpawnTime = (float)SystemAPI.Time.ElapsedTime + enemySpawnerComponent.SpawnCooldown;
    }

    private float3 GetPositionOutsideOfCameraRange()
    {
        var size = enemySpawnerComponent.CameraSize * 2;
        float3 position = new float3(UnityEngine.Random.Range(-size.x, size.x), UnityEngine.Random.Range(-size.y, size.y), 0);

        while (position.x < enemySpawnerComponent.CameraSize.x && position.x > -enemySpawnerComponent.CameraSize.x
            && position.y < enemySpawnerComponent.CameraSize.y && position.y > -enemySpawnerComponent.CameraSize.y)
        {
            position = new float3(UnityEngine.Random.Range(-size.x, size.x), UnityEngine.Random.Range(-size.y, size.y), 0);
        }

        position += new float3(Camera.allCameras[0].transform.position.x, Camera.allCameras[0].transform.position.y, 0);

        return position;
    }
}