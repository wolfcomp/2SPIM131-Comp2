using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


public class EnemyContainerComponent : IComponentData
{
    public List<EnemyData> enemies;
}

public struct EnemyData
{
    public Entity Prefab;
    public float Health;
    public float Damage;
    public float MoveSpeed;
}