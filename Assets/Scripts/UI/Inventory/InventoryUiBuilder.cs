using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUiBuilder : MonoBehaviour
{
    [SerializeField]private InventorySystem gameInventory;
    [SerializeField] private GameObject rowUiPrefab;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject UiParent;

    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private int itemsPerRow;

    private bool hasBeenCreated = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        BiomeEditingEvents.OnItemGenerated+=CloseUIEventWrapper;
    }

        private void OnDisable()
    {
        BiomeEditingEvents.OnItemGenerated-=CloseUIEventWrapper;
    }

    public void BuildOrRebuildUI()
    {
        if(!hasBeenCreated)
        {
            FirstTimeBuildUI();
            Debug.Log("open Inventory pressed");
        }
        else
        {
            inventoryCanvas.SetActive(true);
        }
    }

    private void FirstTimeBuildUI()
    {
        inventoryCanvas.SetActive(true);
        hasBeenCreated = true;
        
        int itemsInRow = 0;
        GameObject currentRow = null;
        GameObject currentButton = null;
        for(int i = 0; i <gameInventory.inventory.Length; i++)
        {
            if(itemsInRow == itemsPerRow || currentRow == null)
            {
                itemsInRow=0;
                currentRow = Instantiate(rowUiPrefab);
                currentRow.transform.SetParent(UiParent.transform,false);
            }

            currentButton = Instantiate(buttonPrefab);
            currentButton.transform.SetParent(currentRow.transform,false);
            currentButton.GetComponent<InventoryButton>().SetInventoryItem(gameInventory.inventory[i]);
            currentButton.GetComponent<InventoryButton>().InitialSetup(gameInventory.inventory[i],gameInventory);
            itemsInRow++;

        }
        
        
    }

    public void CloseUIEventWrapper(GameObject _ignoreThisItem)
    {
        CloseUI();
    }

    public void CloseUI()
    {
        inventoryCanvas.SetActive(false);
    }
}
