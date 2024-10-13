using System;
using Unity.Entities;
using UnityEngine;

public class LootAuthoring : MonoBehaviour
{
    class Baker : Baker<LootAuthoring>
    {
        public override void Bake(LootAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Loot());
        }
    }
}