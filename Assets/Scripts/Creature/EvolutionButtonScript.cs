using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject _attachedButton;
    private bool isChecklistFinished= false;
    private bool isCreaturePlaced= false;
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
            _attachedButton.SetActive(true);
        }
            
    }

    private void ResetButton()
    {
        _attachedButton.SetActive(false);
    }

    public void OnEvolutionButtonClicked()
    {
        CreatureEvents.CreatureEvolvingEvent();
        ResetButton();
    }
}
