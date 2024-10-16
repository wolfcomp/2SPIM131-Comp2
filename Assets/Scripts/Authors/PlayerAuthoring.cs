using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    [SerializeField]
    public byte MaxHealth;

    [SerializeField]
    public GameObject ShotPrefab;

    [SerializeField]
    public float ShotCooldown;

    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Player>(entity);
            AddComponent(entity, new PlayerComponent
            {
                CanPickup = false,
                InvulnDelta = 0,
                ShotDelta = 0,
                ShotPrefab = GetEntity(authoring.ShotPrefab, TransformUsageFlags.Dynamic),
                ShotCooldown = authoring.ShotCooldown
            });
            AddComponent(entity, new HealthComponent
            {
                MaxHealth = authoring.MaxHealth,
                Health = authoring.MaxHealth,
            });
            var inventory = InventoryComponent.CreateDefault();
            AddComponent(entity, inventory);
        }
    }
}
