using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseCreature : MonoBehaviour
{
    private bool canPlay = false;

    private void OnEnable()
    {
        CreatureEvents.OnCreatureSummoned+=ToggleCanPlay;
    }

    private void OnDisable()
    {
        CreatureEvents.OnCreatureSummoned-=ToggleCanPlay;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(canPlay)
        {
            CreatureEvents.InteractionTriggeredEvent(InteractionSocketType.Playing);
            CreatureEvents.CreatureReleasedEvent();
            canPlay = false;
        }
        
    }

    private void ToggleCanPlay(Vector3 _ignoreThisItem)
    {
        canPlay= true;
    }
}
