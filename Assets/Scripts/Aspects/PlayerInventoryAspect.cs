using Unity.Entities;

public readonly partial struct PlayerInventoryAspect : IAspect
{
    public readonly RefRO<Player> Player;
    public readonly RefRO<InventoryComponent> InventoryComponent;
}
