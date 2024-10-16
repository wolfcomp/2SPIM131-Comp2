using Unity.Entities;

public struct Player : IComponentData
{

}

public struct PlayerComponent : IComponentData
{
    public bool CanPickup;
}
