using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the triggering of the feeding event, after receiving a collision from any collider in the creature hierarchy 
/// </summary>
public class CollisionRelay : MonoBehaviour
{
    private bool canCollide = true;
    private void OnCollisionEnter(Collision _coll)
    {

        
        if(_coll.gameObject.tag =="Food"  && canCollide)
        {
                CreatureInteractionPoint colliderInfo;
            if(_coll.GetContact(0).thisCollider.gameObject.TryGetComponent<CreatureInteractionPoint>(out colliderInfo))
            {
                if(colliderInfo.socketType == InteractionSocketType.Feeding && canCollide)
                {
                    CreatureEvents.InteractionTriggeredEvent(colliderInfo.socketType);
                    Destroy(_coll.gameObject);
                    StartCoroutine("CollisionCooldown");
                }
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
