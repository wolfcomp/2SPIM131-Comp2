using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemComponentScriptableObject", order = 1)]
public class ItemComponentScriptableObject : ScriptableObject
{
    public uint ItemId;
    public uint Count;
    public string Name;
    public uint SpriteIndex;
}
