using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ShotAuthoring : MonoBehaviour
{
    public float Speed;

    class Baker : Baker<ShotAuthoring>
    {
        public override void Bake(ShotAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShotComponent
            {
                Velocity = float2.zero,
                Speed = authoring.Speed
            });
        }
    }
}
