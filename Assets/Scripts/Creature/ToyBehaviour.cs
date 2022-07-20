using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBehaviour : MonoBehaviour
{
    private bool canSendToyEvent= false;

    private void OnEnable()
    {
        CreatureEvents.OnCreatureEvolving+=EnableToyEvent;
    }

    private void OnDisable()
    {
        CreatureEvents.OnCreatureEvolving-=EnableToyEvent;
    }

    private void EnableToyEvent()
    {
        canSendToyEvent=true;
    }
    public void OnItemInteracted()
    {
        if(canSendToyEvent)
            CreatureEvents.CreatureSummonedEvent(this.gameObject.transform.position);

        Debug.Log("item interacted");
    }
}
