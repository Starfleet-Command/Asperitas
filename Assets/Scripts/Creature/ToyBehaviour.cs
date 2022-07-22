using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBehaviour : MonoBehaviour
{
    private bool canSendToyEvent= false;
    private int currentCreatureStage=0;

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
        if(currentCreatureStage==1)
        {
            canSendToyEvent=true;
        }
        
        currentCreatureStage++;

        
        
    }
    public void OnItemInteracted()
    {
        if(canSendToyEvent)
        {
            CreatureEvents.CreatureSummonedEvent(this.gameObject.transform.position);
        }
            

        Debug.Log("item interacted");
    }
}
