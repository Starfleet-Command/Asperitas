using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseCreature : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CreatureEvents.InteractionTriggeredEvent(InteractionSocketType.Playing);
        CreatureEvents.CreatureReleasedEvent();
    }
}
