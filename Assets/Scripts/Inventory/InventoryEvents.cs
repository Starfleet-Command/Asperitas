using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEvents : MonoBehaviour
{
    public delegate void ItemCheckoutRequest(InventoryItem _item);
    
    public static event ItemCheckoutRequest OnItemCheckoutRequested;

    public static void ItemCheckoutRequestEvent(InventoryItem _item)
    {
        OnItemCheckoutRequested(_item);
    }

    public delegate void ItemCheckedOut(InventoryItem _item);
    
    public static event ItemCheckedOut OnItemCheckedOut;

    public static void ItemCheckedOutEvent(InventoryItem _item)
    {
        OnItemCheckedOut(_item);
    }

    public delegate void ItemCheckedIn(InventoryItem _item);
    
    public static event ItemCheckedIn OnItemCheckedIn;

    public static void ItemCheckedInEvent(InventoryItem _item)
    {
        OnItemCheckedIn(_item);
    }
}
