using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBehaviour : MonoBehaviour
{
    private bool canSendToyEvent= false;
    private int currentCreatureStage=0;
    [SerializeField] private int stageToEnableToy=0;
    private bool isSummoningCreature=false;

    private void OnEnable()
    {
        CreatureEvents.OnCreatureReleased+=ResetSummoningStatus;
    }

    private void OnDisable()
    {
        CreatureEvents.OnCreatureReleased+=ResetSummoningStatus;
    }
    
    public void OnItemInteracted()
    {
        if(!isSummoningCreature)
        {
            CreatureEvents.CreatureSummonedEvent(this.gameObject.transform.position);
            isSummoningCreature=true;
        }
            

        Debug.Log("item interacted");
    }

    private void ResetSummoningStatus()
    {
        isSummoningCreature=false;
    }

    private void OnDestroy()
    {
        CreatureEvents.CreatureReleasedEvent();
    }
}
