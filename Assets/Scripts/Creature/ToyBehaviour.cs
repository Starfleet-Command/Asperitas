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
    

    private void Start()
    {
        CreatureScriptReferences levelData = CreatureScriptReferences.Instance;
        currentCreatureStage = levelData.missionSystemScript.currentStage;
    }
    private void OnEnable()
    {
        CreatureEvents.OnCreatureReleased+=ResetSummoningStatus;
        CreatureEvents.OnCreatureEvolving+=HandleEvolutions;
    }

    private void OnDisable()
    {
        CreatureEvents.OnCreatureReleased+=ResetSummoningStatus;
        CreatureEvents.OnCreatureEvolving-=HandleEvolutions;
    }
    
    public void OnItemInteracted()
    {
        if(!isSummoningCreature & currentCreatureStage>0)
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

    private void HandleEvolutions()
    {
        currentCreatureStage++;
    }

    private void OnDestroy()
    {
        CreatureEvents.CreatureReleasedEvent();
    }
}
