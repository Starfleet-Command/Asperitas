using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Simple singleton-like script that centralises references to other important scripts
/// </summary>
public class CreatureScriptReferences : MonoBehaviour
{
    public static CreatureScriptReferences Instance;
    private void Awake() => Instance = this;

    public BiomeHabitability habitabilityTrackingScript;
    public ChecklistMissionSystem missionSystemScript;
    public GameObject sceneCamera;

}
