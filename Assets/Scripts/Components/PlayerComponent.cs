using Unity.Entities;

public struct Player : IComponentData
{

}

public struct PlayerComponent : IComponentData
{
    public bool CanPickup;
    public float InvulnDelta;
    public float ShotDelta;
    public Entity ShotPrefab;
    public float ShotCooldown;
    public bool IsRapid;
}
