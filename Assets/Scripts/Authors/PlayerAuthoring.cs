using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    [SerializeField]
    public byte MaxHealth;

    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Player>(entity);
            AddComponent(entity, new PlayerComponent
            {
                CanPickup = false,
                MaxHealth = authoring.MaxHealth,
                Health = authoring.MaxHealth,
                InvulnDelta = 0
            });
            var inventory = InventoryComponent.CreateDefault();
            AddComponent(entity, inventory);
        }
    }
}
