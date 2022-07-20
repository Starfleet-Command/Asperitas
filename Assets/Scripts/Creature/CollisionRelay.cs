using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if(colliderInfo.socketType == InteractionSocketType.Feeding)
                {
                    CreatureEvents.InteractionTriggeredEvent(colliderInfo.socketType);
                    Destroy(_coll.gameObject);
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
