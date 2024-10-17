using Unity.Entities;
using Unity.Mathematics;

public struct ShotComponent : IComponentData
{
    public float2 Velocity;
    public float LivingTime;
    public float Speed;
}
