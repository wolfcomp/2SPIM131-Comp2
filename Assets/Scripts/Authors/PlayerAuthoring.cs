using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Player>(entity);
            AddComponent(entity, new PlayerComponent
            {
                CanPickup = false
            });
            var inventory = InventoryComponent.CreateDefault();
            AddComponent(entity, inventory);
        }
    }
}
