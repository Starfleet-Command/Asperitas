using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseCreature : MonoBehaviour
{
    private bool canPlay = false;
    private bool hasSentEvent = false;
    [SerializeField] private int timeUntilRelease = 8;

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
            if(other.gameObject.tag == "Creature")
            {
                SendInteractionEvents();
                hasSentEvent = true;
            }
            
        }
        
    }

    private void SendInteractionEvents()
    {
        CreatureEvents.InteractionTriggeredEvent(InteractionSocketType.Playing);
        CreatureEvents.CreatureReleasedEvent();
        canPlay = false;
    }

    private void ToggleCanPlay(Vector3 _ignoreThisItem)
    {
        canPlay= true;
        hasSentEvent = false;
        StartCoroutine("StartReleaseCountdown");
    }

    IEnumerator StartReleaseCountdown()
    {
        yield return new WaitForSeconds(timeUntilRelease);
        
        if(!hasSentEvent)
            SendInteractionEvents();
    }
}
