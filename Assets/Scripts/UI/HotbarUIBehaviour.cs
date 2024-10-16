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
    private List<Sprite> _icons = new();

    private readonly Dictionary<int, HotbarSlotUIBehaviour> _hotbarSlots = new();
    
    void Start()
    {
        var entitySystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<HotbarUIInventorySystem>();
        entitySystem.AttachUI(this);
    }

    /// <summary>
    /// Adds a slot to the Hotbar UI
    /// </summary>
    /// <param name="index">The index of the slot to spawn</param>
    public void AddUI(int index)
    {
        if (_hotbarSlots.ContainsKey(index))
            return;
        var slot = Instantiate(HotbarSlot, HotbarCanvas.transform);
        slot.UpdateOffset(index);
        _hotbarSlots.Add(index, slot);
    }

    /// <summary>
    /// Sets the icon of the specified <paramref name="index"/> to the specified <paramref name="iconIndex"/> based on the Sprite list of this object
    /// </summary>
    /// <param name="index">The index of the slot to modify</param>
    /// <param name="iconIndex">The index of the sprite to set</param>
    public void SetIcon(int index, uint iconIndex)
    {
        if (!_icons.TryGetValue((int)iconIndex, out var icon))
            return;
        if (!_hotbarSlots.TryGetValue(index, out var slot))
            return;
        slot.SetImage(icon);
    }

    /// <summary>
    /// Sets the text of the specified <paramref name="index"/> to the specified <paramref name="text"/>
    /// </summary>
    /// <param name="index">The index of the slot to modify</param>
    /// <param name="text">The text to set on the slot</param>
    public void SetText(int index, string text)
    {
        if (!_hotbarSlots.TryGetValue(index, out var slot))
            return;
        slot.SetText(text);
    }
}
