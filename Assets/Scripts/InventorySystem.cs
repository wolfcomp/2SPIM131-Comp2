using Unity.Burst;
using Unity.Entities;

partial struct InventorySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InventoryComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

    }
}
