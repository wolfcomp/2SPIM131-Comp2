using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;

public struct ShotComponent : IComponentData
{
    public float2 Velocity;
    public float LivingTime;
    public float Speed;
}
