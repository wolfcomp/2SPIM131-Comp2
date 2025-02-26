using System.Collections.Generic;
using Unity.Entities;


public class EnemyContainerComponent : IComponentData
{
    public List<EnemyData> Enemies;
}

public struct EnemyData : IComponentData
{
    public Entity Prefab;
    public byte Health;
    public byte Damage;
    public float MoveSpeed;
}