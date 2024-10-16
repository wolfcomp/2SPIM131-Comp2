using JetBrains.Annotations;
using Unity.Entities;

public partial class HotbarUIInventorySystem : SystemBase
{
    [CanBeNull]
    private HotbarUIBehaviour _hotbarCanvas;

    protected override void OnCreate()
    {
        RequireForUpdate<Player>();
    }

    protected override void OnUpdate()
    {
        var player = SystemAPI.GetSingletonEntity<Player>();
        var inventoryComponent = SystemAPI.GetComponentRO<InventoryComponent>(player);
        if (!inventoryComponent.IsValid || _hotbarCanvas == null) return;
        var container = inventoryComponent.ValueRO.ContainersSpan.GetRef(t => t.InventoryType == InventoryType.Hotbar);
        for (var i = 0; i < container.ItemsSpan.Length; i++)
        {
            var item = container.ItemsSpan.GetRef(i);
            _hotbarCanvas.AddUI(i);
            if (item.ItemId == 0) continue;
            _hotbarCanvas.SetIcon(i, item.SpriteIndex);
            if (item.Count > 1)
                _hotbarCanvas.SetText(i, item.Count.ToString());
        }
    }

    public void AttachUI(HotbarUIBehaviour hotbarCanvas)
    {
        _hotbarCanvas = hotbarCanvas;
    }
}
