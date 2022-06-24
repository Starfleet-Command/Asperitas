using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EvolutionButtonScript : MonoBehaviour
{
    private Button _attachedButton;
    private void OnEnable()
    {
        CreatureEvents.OnChecklistFinished+=EnableButton;
    }

    private void OnDisable()
    {
        CreatureEvents.OnChecklistFinished-=EnableButton;
    }

    // Start is called before the first frame update
    void Start()
    {
        _attachedButton = this.gameObject.GetComponent<Button>();
    }

    private void EnableButton()
    {
        _attachedButton.interactable=true;
    }

    public void OnEvolutionButtonClicked()
    {
        CreatureEvents.CreatureEvolvingEvent();
    }
}
