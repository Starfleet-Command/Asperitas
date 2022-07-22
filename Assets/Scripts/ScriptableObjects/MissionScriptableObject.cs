using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MissionData", menuName = "ScriptableObjects/Missions", order = 1)]
[System.Serializable]
public class MissionScriptableObject : ScriptableObject
{
    public ChecklistWrapper[] missions;
}
