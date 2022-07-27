using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject _attachedButton;
    private bool isChecklistFinished= false;
    private bool isCreaturePlaced= false;
    
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite deactivatedSprite;
    [SerializeField] private Sprite activatedSprite;
    private void OnEnable()
    {
        CreatureEvents.OnChecklistFinished+=SetChecklistFinished;
        CreatureEvents.OnCreaturePlaced+=SetCreaturePlaced;
    }

    private void OnDisable()
    {
        CreatureEvents.OnChecklistFinished-=SetChecklistFinished;
        CreatureEvents.OnCreaturePlaced-=SetCreaturePlaced;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void SetChecklistFinished()
    {
        isChecklistFinished = true;
        TryEnableButton();
    }

    private void SetCreaturePlaced(GameObject _ignoreThisItem)
    {
        isCreaturePlaced = true;
        TryEnableButton();
    }

    private void TryEnableButton()
    {
        if(isChecklistFinished && isCreaturePlaced)
        {
            buttonImage.sprite = activatedSprite;
            _attachedButton.SetActive(true);
        }
            
    }

    private void ResetButton()
    {
        buttonImage.sprite = deactivatedSprite;
        _attachedButton.SetActive(false);
    }

    public void OnEvolutionButtonClicked()
    {
        CreatureEvents.CreatureEvolvingEvent();
        ResetButton();
    }
}
