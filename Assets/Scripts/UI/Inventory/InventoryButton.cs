using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryButton : MonoBehaviour
{
    [SerializeField] private InventoryItem itemToSpawn;
    [SerializeField] private InventorySystem itemInventoryReference;
    [SerializeField] private Image _thumbnail;
    [SerializeField] private Text qtyText;
    [SerializeField] private Image biomeIcon;
    [SerializeField] private Image tagIcon;
    [SerializeField] private Button itemButton;

    public bool isNewSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if(isNewSpawn)
        {
            isNewSpawn = false;
        }

        else 
            UpdateQuantityText();
    }

    public void SetInventoryItem(InventoryItem _itemToSet)
    {
        itemToSpawn = _itemToSet;
        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(delegate {RequestCheckout();});

    }

    public void InitialSetup(InventoryItem _item, InventorySystem _systemReference)
    {
        qtyText.text = _item.getItemQuantity().ToString();
        _thumbnail.sprite = _item.getItemSprite();

        itemInventoryReference = _systemReference;
        TagSpriteTuple _tuple = null;
        foreach (ItemTag _tag in _item.getTagsList())
        {
            _tuple = itemInventoryReference.FindSpriteTupleWithTag(_tag);

            if(_tuple!=null)
            {
                tagIcon.sprite = _tuple.getSprite();
            }
        }
        
        if(_item.getItemQuantity()<=0)
        {
            itemButton.enabled = false;
        }
    }

    //Used in case the player deletes or crafts items when inventory is closed.
    private void UpdateQuantityText()
    {
        qtyText.text = itemToSpawn.getItemQuantity().ToString();

        if(itemToSpawn.getItemQuantity()<=0)
        {
            itemButton.enabled = false;
        }

        else if(!itemButton.enabled && itemToSpawn.getItemQuantity()>0)
        {
            itemButton.enabled = true;
        }
               
    }

    public void RequestCheckout()
    {
        if(itemToSpawn!= null)
        {
            itemInventoryReference.CheckoutAndSpawnItem(itemToSpawn.getName());
               
        }
    }
}
