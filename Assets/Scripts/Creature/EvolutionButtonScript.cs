using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EvolutionButtonScript : MonoBehaviour
{
    private Button _attachedButton;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        _attachedButton = this.gameObject.GetComponent<Button>();
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
            _attachedButton.interactable=true;
    }

    public void OnEvolutionButtonClicked()
    {
        CreatureEvents.CreatureEvolvingEvent();
    }
}
