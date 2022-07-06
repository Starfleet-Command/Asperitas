using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
public class CreatureInteractionPoint : MonoBehaviour
{
    public InteractionSocketType socketType;

    private bool canCollide = true;
    private void OnCollisionEnter(Collision other)
    {
        if(socketType == InteractionSocketType.Feeding)
        {
            if(other.gameObject.tag =="Food" && canCollide)
            {
                CreatureEvents.InteractionTriggeredEvent(socketType);
                Destroy(other.gameObject);
                StartCoroutine("CollisionCooldown");
            }
        }
    }

    IEnumerator CollisionCooldown()
    {
        canCollide = false;
        yield return new WaitForSeconds(2);
        canCollide = true;
    }

}
