using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemySpawnerAuthoring : MonoBehaviour 
{

    public float SpawnCooldown = 0.1f;
    public List<EnemySo> EnemiesSoList;
    public Vector2 CameraSize;

    public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            var enemySpawnerAuthoring = GetEntity(TransformUsageFlags.None);

            AddComponent(enemySpawnerAuthoring, new EnemySpawnerComponent
            {
                SpawnCooldown = authoring.SpawnCooldown,
                CameraSize = authoring.CameraSize,
            });

            var _enemyDataList = new List<EnemyData>();

            foreach (var e in authoring.EnemiesSoList)
            {
                _enemyDataList.Add(new EnemyData
                {
                    Damage = e.Damage,
                    Health = e.Health,
                    MoveSpeed = e.MoveSpeed,
                    Prefab = GetEntity(e.Prefab, TransformUsageFlags.None)
                });
            }

            AddComponentObject(enemySpawnerAuthoring, new EnemyContainerComponent { Enemies = _enemyDataList });

        }
    }
}
