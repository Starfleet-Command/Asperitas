using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class acts as a decreasing counter for a number, to simulate items being taken out of an inventory
/// </summary>
public class ThrowableObjectCount : MonoBehaviour
{
   public int objectQty=1;
    [SerializeField] private Text qtyText;
   private void Start()
   {
       qtyText.text= objectQty.ToString();
   }

   public void TakeOutItem()
   {
        objectQty--;
        qtyText.text=objectQty.ToString();
   }
}
