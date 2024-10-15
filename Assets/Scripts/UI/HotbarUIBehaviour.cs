using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class HotbarUIBehaviour : MonoBehaviour
{
    public GameObject HotbarCanvas => _hotbarCanvas;
    [SerializeField]
    private GameObject _hotbarCanvas;

    public HotbarSlotUIBehaviour HotbarSlot => _hotbarSlot;
    [SerializeField]
    private HotbarSlotUIBehaviour _hotbarSlot;

    [SerializeField]
    private Dictionary<int, GameObject> _hotbarSlots;

    [SerializeField]
    private SerializedDictionary<int, Sprite> _icons = new ();

    void Start()
    {
    }

    void AddUI(int index)
    {

    }

    void SetIcon(int index, uint iconIndex)
    {

    }

    void SetText(int index, string text)
    {

    }
}
