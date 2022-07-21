using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerer : MonoBehaviour
{
    [SerializeField] private Animator animController;

    private void OnEnable()
    {
        CreatureEvents.OnCreatureBeginPet+=TriggerPetting;
        CreatureEvents.OnInteractionTriggered+=ReactToInteraction;
    }

        private void OnDisable()
    {
        CreatureEvents.OnInteractionTriggered-=ReactToInteraction;
        CreatureEvents.OnCreatureBeginPet-=TriggerPetting;
    }

    private void ReactToInteraction(InteractionSocketType _interactionType)
    {
        if(_interactionType== InteractionSocketType.Feeding)
        {
            TriggerEating();
        }
        else if(_interactionType== InteractionSocketType.Petting)
        {
            TriggerFeedbackDirectly();
        }
        else if(_interactionType== InteractionSocketType.Playing)
        {
            TriggerFeedbackDirectly();
        }
    }

    public void TriggerFeedbackDirectly()
    {
        StopMoving();
        animController.SetTrigger("feedbackTrigger");
    }

    public void TriggerEating()
    {
        StopMoving();
        animController.SetTrigger("eatingTrigger");
    }

    public void TriggerPetting()
    {
        StopMoving();
        animController.SetTrigger("pettingTrigger");
    }

    public void SetMoving()
    {
        animController.SetBool("isMoving",true);
    }

    public void StopMoving()
    {
        animController.SetBool("isMoving",false);
    }
}
