using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyAnimTriggerer : MonoBehaviour
{
    public Animator toyAnimator;
    private void OnEnable()
    {
        CreatureEvents.OnCreatureSummoned+=TriggerDeploy;
        CreatureEvents.OnCreatureReleased+=TriggerCollapse;
    }
    
    private void OnDisable()
    {
        CreatureEvents.OnCreatureSummoned-=TriggerDeploy;
        CreatureEvents.OnCreatureReleased-=TriggerCollapse;
    }

    private void TriggerDeploy(Vector3 _ignoreThisItem)
    {
        toyAnimator.SetTrigger("playerHasInteracted");
    }

    private void TriggerCollapse()
    {
        toyAnimator.SetTrigger("creatureHasInteracted");
    }
}
