using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class InventorySystem: MonoBehaviour
{
    private InventoryItem _checkedItem;
    [HideInInspector] public InventoryItem[] inventory;
    public TagSpriteTuple[] tagSprites;
    [SerializeField] private InventoryScriptableObject inventoryData;
    

    private void OnEnable()
    {
        InventoryEvents.OnItemCheckedIn+=ReAddToInventory;
        BiomeEditingEvents.OnItemPlaced+=SubtractFromInventory;

        InventoryScriptableObject dataInstance = Instantiate<InventoryScriptableObject>(inventoryData);
        inventory = dataInstance.inventory;
        SortInventory(inventory);
    }

        private void OnDisable()
    {
        InventoryEvents.OnItemCheckedIn-=ReAddToInventory;
        BiomeEditingEvents.OnItemPlaced-=SubtractFromInventory;
    }


    public void SortInventory(InventoryItem[] _invToSort)
    {
        Array.Sort(_invToSort, (x,y) => String.Compare(x.getName(), y.getName(),true));
    }

    public InventoryItem FindByName(string name)
    {
        InventoryItem foundItem= null;
        foundItem = Array.Find(this.inventory, element => element.getName() == name);
        return foundItem;
    }

    public TagSpriteTuple FindSpriteTupleWithTag(ItemTag tag)
    {
        foreach (TagSpriteTuple _tuple in tagSprites)
        {
            if(_tuple.getTag()==tag)
            {
                return _tuple;
            }
        }

        return null;
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
        _checkedItem = null;
        PlacedObjectAttributes objectAttributeScript = null;
        GameObject spawnedItem = null;
        _checkedItem = FindByName(_name);
        
        if(_checkedItem != null)
        {
            spawnedItem = Instantiate(_checkedItem.getItemPrefab());
            if(spawnedItem.TryGetComponent<PlacedObjectAttributes>(out objectAttributeScript))
            {
                objectAttributeScript.sourceItem = _checkedItem;
            }
        }

        BiomeEditingEvents.ItemGeneratedEvent(spawnedItem);
    }

    public void SubtractFromInventory(GameObject _ignoreThisItem)
    {
        _checkedItem?.removeItems(1);
    }

    public void ReAddToInventory(InventoryItem _item)
    {
        InventoryItem itemRef = FindByName(_item.getName());
        itemRef.addItems(1);
    }
}

[System.Serializable]
public class InventoryItem
{
    [SerializeField]private string name;
    [SerializeField]private GameObject itemPrefab;
    [SerializeField]private Sprite itemSprite;
    [SerializeField]private List<ItemTag> tags = new List<ItemTag>();
    [SerializeField]private int quantity;

    public InventoryItem(InventoryItem _pastItem)
    {
        name = _pastItem.getName();
        itemPrefab = _pastItem.getItemPrefab();
        itemSprite = _pastItem.getItemSprite();
        tags = _pastItem.getTagsList();
        quantity = _pastItem.getItemQuantity();
    }

    public void setItems(int _newQuantity)
    {
        if(_newQuantity>quantity)
        {
            this.addItems(_newQuantity-quantity);
        }
        else if(_newQuantity<quantity)
        {
            this.removeItems(quantity-_newQuantity);
        }
        else return;
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
        this.tags.Add(newTag);
    }

    public ItemTag getTag(int index)
    {
        return this.tags[index];
    }

    public ItemTag[] getTags()
    {
        return tags.ToArray();
    }

    public List<ItemTag> getTagsList()
    {
        return tags;
    }

    public bool hasTag(ItemTag tag)
    {
        foreach (ItemTag singleTag in tags)
        {
            if(tag == singleTag)
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

[System.Serializable]
public class TagSpriteTuple
{
    [SerializeField] private ItemTag tag;
    [SerializeField] private Sprite itemSprite;

    public ItemTag getTag()
    {
        return tag;
    }

    public Sprite getSprite()
    {
        return itemSprite;
    }


}



