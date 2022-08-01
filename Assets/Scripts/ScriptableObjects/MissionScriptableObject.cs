using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class creates a persistent asset for the mission system using Unity's ScriptableObjects, in accordance to the MVC pattern.
/// </summary>
[CreateAssetMenu(fileName = "MissionData", menuName = "ScriptableObjects/Missions", order = 1)]
[System.Serializable]
public class MissionScriptableObject : ScriptableObject
{
    public ChecklistWrapper[] missions;
}
