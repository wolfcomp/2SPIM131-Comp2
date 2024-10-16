using Unity.Mathematics;
using Unity.Entities;

public struct EnemySpawnerComponent : IComponentData
{
    public float SpawnCooldown;
    public float2 CameraSize;
}
