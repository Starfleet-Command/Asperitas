using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureScriptReferences : MonoBehaviour
{
    public static CreatureScriptReferences Instance;
    private void Awake() => Instance = this;

    public GenerateMissionUI uiFromChecklistScript;
    public BiomeHabitability habitabilityTrackingScript;
    public CreatureFriendship creatureFriendshipScript;
    public GameObject sceneCamera;

}