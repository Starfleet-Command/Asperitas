using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the animator for the toy, reacting to the players taps and the creature's nearness
/// </summary>
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
