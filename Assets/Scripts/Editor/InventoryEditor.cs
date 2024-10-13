using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(InventoryComponent))]
public class InventoryComponentEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var inventoryComponent = (InventoryComponent)(object)serializedObject.targetObject;
        var element = new VisualElement();
        return element;
    }
}

[CustomEditor(typeof(InventoryContainerComponent))]
public class InventoryContainerComponentEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var inventoryComponent = (InventoryContainerComponent)(object)serializedObject.targetObject;
        var element = new VisualElement();
        return element;
    }
}