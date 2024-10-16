using Unity.Entities;

public struct HealthComponent : IComponentData
{
    public byte MaxHealth;
    public byte Health;
}
