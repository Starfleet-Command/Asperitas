using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class InventorySystem: MonoBehaviour
{

    public InventoryItem[] inventory;
    public void SortInventory()
    {
        Array.Sort(this.inventory, (x,y) => String.Compare(x.getName(), y.getName(),true));
    }

    public InventoryItem FindByName(string name)
    {
        InventoryItem foundItem= null;
        foundItem = Array.Find(this.inventory, element => element.getName() == name);
        return foundItem;
    }

    public InventoryItem[] FindAllWithTag(ItemTag tag)
    {
        InventoryItem[] result=null;
        result = Array.FindAll(this.inventory, element => element.hasTag(tag) == true);
        return result;
    }

    public void OnInventoryButtonClick(GameObject _buttonObj)
    {
        InventorySpawnButtonData buttonInfo = null;
        if(_buttonObj.TryGetComponent<InventorySpawnButtonData>(out buttonInfo))
        {
            CheckoutAndSpawnItem(buttonInfo.itemToSpawn.getName());
        }
        
    }

    public void CheckoutAndSpawnItem(string _name)
    {
        InventoryItem checkedItem = null;
        PlacedObjectAttributes objectAttributeScript = null;
        GameObject spawnedItem = null;
        checkedItem = FindByName(_name);
        
        if(checkedItem != null)
        {
            checkedItem.removeItems(1);
            spawnedItem = Instantiate(checkedItem.getItemPrefab());
            if(spawnedItem.TryGetComponent<PlacedObjectAttributes>(out objectAttributeScript))
            {
                objectAttributeScript.sourceItem = checkedItem;
            }
        }

        BiomeEditingEvents.ItemGeneratedEvent(spawnedItem);
    }
}

[System.Serializable]
public class InventoryItem
{
    [SerializeField]private string name;
    [SerializeField]private GameObject itemPrefab;
    [SerializeField]private Sprite itemSprite;
    [SerializeField]private List<int> itemTags = new List<int>();
    [SerializeField]private int quantity;

    public InventoryItem(InventoryItem _pastItem)
    {
        name = _pastItem.getName();
        itemPrefab = _pastItem.getItemPrefab();
        itemSprite = _pastItem.getItemSprite();
        itemTags = _pastItem.getTagsList();
        quantity = _pastItem.getItemQuantity();
    }
    public void addItems(int quantityToAdd)
    {
        if(quantityToAdd>0)
        {
            this.quantity+= quantityToAdd;
        }
    }

    public void removeItems(int quantityToRemove)
    {
        if(this.quantity-quantityToRemove>=0)
        {
            this.quantity-= quantityToRemove;
        }
        else Debug.LogError("Attempting to remove more items than are available in the inventory");
    }

    public void Rename(string newName)
    {
        this.name=newName;
    }

    public string getName()
    {
        return this.name;
    }

    public void addTag(ItemTag newTag)
    {
        this.itemTags.Add((int) newTag);
    }

    public ItemTag getTag(int index)
    {
        return (ItemTag)this.itemTags[index];
    }

    public int[] getTags()
    {
        return itemTags.ToArray();
    }

    public List<int> getTagsList()
    {
        return itemTags;
    }

    public bool hasTag(ItemTag tag)
    {
        int tagIndex = (int)tag;

        foreach (int singleTag in itemTags)
        {
            if(tagIndex == singleTag)
            {
                return true;
            }
        }

        return false;
    }

    public GameObject getItemPrefab()
    {
        return this.itemPrefab;
    }

    public Sprite getItemSprite()
    {
        return this.itemSprite;
    }

    public int getItemQuantity()
    {
        return this.quantity;
    }


}



