using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlacedObjectAttributes : MonoBehaviour
{
    public StackabilityType stackabilityType;
    public Vector3 offsetAfterPlacement;

    public BiomePercentageTuple biomeEffect;

    [HideInInspector]public InventoryItem sourceItem;
    private void Start()
    {
    }

    public bool CanPlace(StackabilityType collidedObjectStackability)
    {
        if(stackabilityType==StackabilityType.Foundation)
        {
            if(collidedObjectStackability==StackabilityType.Gameboard)
                return true;
            
            else return false;
        }
        else if(stackabilityType==StackabilityType.Stackable || stackabilityType==StackabilityType.Nonstackable)
        {
            if(collidedObjectStackability==StackabilityType.Foundation || collidedObjectStackability==StackabilityType.Stackable)
            {
                return true;
            }

            else return false;
        }

        else return false;
        
    
    }

    private void OnDisable()
    {
        if(sourceItem.getItemPrefab()!=null)
        {
            InventoryItem returnItem = new InventoryItem(sourceItem);
            // BiomeEditingEvents.PlacedItemRemovedEvent(returnItem);
        }
       
    }

   /*  private void OnCollisionEnter(Collision other)
    {
        noOfCollisions++;

        if(noOfCollisions>0)
        {
            doneButton.interactable=false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
         if(noOfCollisions>0)
        {
            
            noOfCollisions--;
        }
        

        if(noOfCollisions<=0)
        {
            
            doneButton.interactable=true;
        }
    } */   

}


