using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class EnemySpawnerAuthoring : MonoBehaviour 
{

    public float SpawnCooldown = 0.1f;
    public List<EnemySO> EnemiesSOList;
    public Vector2 CameraSize;

    public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            Entity enemySpawnerAuthoring = GetEntity(TransformUsageFlags.None);

            AddComponent(enemySpawnerAuthoring, new EnemySpawnerComponent
            {
                SpawnCooldown = authoring.SpawnCooldown,
                CameraSize = authoring.CameraSize,
            });

            List<EnemyData> EnemyDataList = new List<EnemyData>();

            foreach (EnemySO e in authoring.EnemiesSOList)
            {
                EnemyDataList.Add(new EnemyData
                {
                    Damage = e.Damage,
                    Health = e.Health,
                    MoveSpeed = e.MoveSpeed,
                    Prefab = GetEntity(e.Prefab, TransformUsageFlags.None)
                });
            }

            AddComponentObject(enemySpawnerAuthoring, new EnemyContainerComponent { enemies = EnemyDataList });

        }
    }
}
