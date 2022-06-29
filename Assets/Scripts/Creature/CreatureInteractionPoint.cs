using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
public class CreatureInteractionPoint : MonoBehaviour
{
    [SerializeField] private InteractionSocketType socketType;

    public void TriggerInteractionEvent(LeanSelectableByFinger _selectStatus)
    {
        Debug.Log(" "+socketType.ToString());
        CreatureEvents.InteractionTriggeredEvent(socketType);
    }
    

}
