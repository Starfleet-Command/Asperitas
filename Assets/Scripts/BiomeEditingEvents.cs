using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeEditingEvents : MonoBehaviour
{

    public delegate void ItemGenerated(GameObject _item);
    
    public static event ItemGenerated OnItemGenerated;

    public static void ItemGeneratedEvent(GameObject _item)
    {
        OnItemGenerated?.Invoke(_item);
    }

    public delegate void ItemSelected(GameObject _item);
    
    public static event ItemSelected OnItemSelected;

    public static void ItemChangedEvent(GameObject _item)
    {
        OnItemSelected?.Invoke(_item);
    }

    public delegate void ItemPlaced(GameObject _item);

    public static event ItemPlaced OnItemPlaced;

    public static void ItemPlacedEvent(GameObject _item)
    {
        OnItemPlaced?.Invoke(_item);
    }

    //InventoryItem to prevent reference being lost on deletion
    public delegate void PlacedItemRemoved(InventoryItem _item);

    public static event PlacedItemRemoved OnPlacedItemRemoved;

    public static void PlacedItemRemovedEvent(InventoryItem _item)
    {
        OnPlacedItemRemoved?.Invoke(_item);
    }

    public delegate void PlacedItemMoved(GameObject _item);

    public static event PlacedItemMoved OnPlacedItemMoved;

    public static void PlacedItemMovedEvent(GameObject _item)
    {
        OnPlacedItemMoved?.Invoke(_item);
    }

    public delegate void BiomeHabitabilityModified(BiomePercentageTuple modifiedBiome);

    public static event BiomeHabitabilityModified OnBiomeHabitabilityModified;

    public static void BiomeHabitabilityModifiedEvent(BiomePercentageTuple modifiedBiome)
    {
        OnBiomeHabitabilityModified?.Invoke(modifiedBiome);
    }

    
}
