using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
public class CreatureInteractionPoint : MonoBehaviour
{
    public InteractionSocketType socketType;

    public void TriggerInteractionEvent(LeanSelectableByFinger _selectStatus)
    {
        CreatureEvents.InteractionTriggeredEvent(socketType);
    }
    

}
