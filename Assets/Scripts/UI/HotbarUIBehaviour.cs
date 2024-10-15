using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class HotbarUIBehaviour : MonoBehaviour
{
    public GameObject HotbarCanvas => _hotbarCanvas;
    [SerializeField]
    private GameObject _hotbarCanvas;

    public HotbarSlotUIBehaviour HotbarSlot => _hotbarSlot;
    [SerializeField]
    private HotbarSlotUIBehaviour _hotbarSlot;

    [SerializeField]
    private readonly Dictionary<int, HotbarSlotUIBehaviour> _hotbarSlots = new();

    [SerializeField]
    private List<Sprite> _icons = new();
    
    void Start()
    {
        var entitySystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<HotbarUIInventorySystem>();
        entitySystem.AttachUI(this);
    }

    public void AddUI(int index)
    {
        if (_hotbarSlots.ContainsKey(index))
            return;
        var slot = Instantiate(HotbarSlot, HotbarCanvas.transform);
        slot.UpdateOffset(index);
        _hotbarSlots.Add(index, slot);
    }

    public void SetIcon(int index, uint iconIndex)
    {
        if (!_icons.TryGetValue((int)iconIndex, out var icon))
            return;
        if (!_hotbarSlots.TryGetValue(index, out var slot))
            return;
        slot.SetImage(icon);
    }

    public void SetText(int index, string text)
    {
        if (!_hotbarSlots.TryGetValue(index, out var slot))
            return;
        slot.SetText(text);
    }
}
