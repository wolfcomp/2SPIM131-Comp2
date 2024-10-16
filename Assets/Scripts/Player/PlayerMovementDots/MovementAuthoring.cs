using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class MovementAuthoring : MonoBehaviour
{
    public float MoveSpeed = 7f;
    public float MoveMultiplier = 10f;
    public float Drag    = 15f;
    public float Height  = 2f;
    public float Width   = 1f;

    public class MovementAuthoringBaker : Baker<MovementAuthoring>
    {
        public override void Bake(MovementAuthoring authoring)
        {
            Entity playerAuthoring = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(playerAuthoring, new MovementComponent{MoveSpeed = authoring.MoveSpeed, 
                MoveMultiplier = authoring.MoveMultiplier, 
                Drag = authoring.Drag,
                Height = authoring.Height,
                Width = authoring.Width
            });
        }
    }
}

public struct MovementComponent : IComponentData
{
    public float MoveSpeed;
    public float MoveMultiplier;
    public float Drag;
    public float Height;
    public float Width;

}
