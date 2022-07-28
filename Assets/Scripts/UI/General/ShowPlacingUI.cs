using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowPlacingUI : MonoBehaviour
{
    [SerializeField] GameObject placingPanel;
    [SerializeField] GameObject preventSelectionPanel;
    [SerializeField] Image placingPanelImage;

    private Sprite itemBeingPlacedImage;
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
        BiomeEditingEvents.OnItemGenerated+=CatchGeneratedItem;
        BiomeEditingEvents.OnItemPlaced+=CloseOnPlace;
        CreatureEvents.OnCreaturePlaced+=CloseOnPlace;
        UiEvents.OnPlacementCanceled+=CloseOnCancel;
    }

    private void OnDisable()
    {
        BiomeEditingEvents.OnItemGenerated-=CatchGeneratedItem;
        BiomeEditingEvents.OnItemPlaced-=CloseOnPlace;
        CreatureEvents.OnCreaturePlaced-=CloseOnPlace;
        UiEvents.OnPlacementCanceled-=CloseOnCancel;
    }

    private void CatchGeneratedItem(GameObject _generatedItem)
    {
         if(_generatedItem.TryGetComponent<PlacedObjectAttributes>(out PlacedObjectAttributes _attributeScript))
         {
            ToggleUI(true);
            itemBeingPlacedImage = _attributeScript.sourceItem.getItemSprite();
            placingPanelImage.sprite=itemBeingPlacedImage;
         }
    }

    private void CloseOnPlace(GameObject _ignoreThisItem)
    {
        ToggleUI(false);
        itemBeingPlacedImage = null;
    }

    private void CloseOnCancel()
    {
        ToggleUI(false);
        itemBeingPlacedImage = null;
    }

    private void ToggleUI(bool status)
    {
        placingPanel.SetActive(status);
        preventSelectionPanel.SetActive(status);
    }
}
