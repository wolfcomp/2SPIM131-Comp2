using Unity.Entities;
using Unity.Mathematics;

public partial struct InputComponent : IComponentData
{
    public float2 MVector;
    public bool IsInteracting;
    public bool DidInteract;
    public bool Inventory;
    public bool IsShooting;
    public bool RapidMode;
}
