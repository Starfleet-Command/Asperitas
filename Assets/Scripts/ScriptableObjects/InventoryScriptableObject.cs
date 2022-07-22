using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "ScriptableObjects/Inventory", order = 1)]
[System.Serializable]
public class InventoryScriptableObject : ScriptableObject
{
     public InventoryItem[] inventory;
}
