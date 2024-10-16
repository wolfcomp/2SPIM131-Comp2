using Unity.Entities;

public struct Player : IComponentData
{

}

public struct PlayerComponent : IComponentData
{
    public bool CanPickup;
    public byte MaxHealth;
    public byte Health;
    public float InvulnDelta;
}
