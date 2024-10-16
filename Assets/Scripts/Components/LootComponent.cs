using Unity.Entities;

public struct Loot : IComponentData
{

}

public struct LootComponent : IComponentData
{
    public bool IsInPickupRange;
}