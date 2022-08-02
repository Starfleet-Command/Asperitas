using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class holds the biome attributes such as stackability hierarchy, habitability effect and their related functions
/// </summary>
public class PlacedObjectAttributes : MonoBehaviour
{
    public StackabilityType stackabilityType;
    public Vector3 offsetAfterPlacement;

    public BiomePercentageTuple biomeEffect;

    [HideInInspector]public InventoryItem sourceItem;

    /// <summary>
    /// Can place checks if the calling object can be placed on top of the object being passed
    /// </summary>
    /// <param name="collidedObjectStackability"> Object that the current object is being placed upon</param>
    /// <returns> true if placeable, false if not placeable</returns>
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
}


