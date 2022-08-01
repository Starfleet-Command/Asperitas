using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class creates a persistent asset for the inventory using Unity's ScriptableObjects, in accordance to the MVC pattern.
/// </summary>
[CreateAssetMenu(fileName = "InventoryData", menuName = "ScriptableObjects/Inventory", order = 1)]
[System.Serializable]
public class InventoryScriptableObject : ScriptableObject
{
     public InventoryItem[] inventory;
}
