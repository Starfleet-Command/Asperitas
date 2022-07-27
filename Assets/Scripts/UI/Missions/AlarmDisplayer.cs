using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AlarmDisplayer : MonoBehaviour
{
    [SerializeField] private Sprite openBaseMenuSprite;
    [SerializeField] private Sprite openAlarmMenuSprite;
    [SerializeField] private Sprite closeBaseMenuSprite;
    [SerializeField] private Sprite closeAlarmMenuSprite;
    [SerializeField] private Image openButtonImage;
    [SerializeField] private Image closeButtonImage;

    
    private void OnEnable()
    {
        UiEvents.OnChangeAlarmState+=ToggleAlarmVisibility;
    }

    private void OnDisable()
    {
        UiEvents.OnChangeAlarmState-=ToggleAlarmVisibility;
    }

    private void ToggleAlarmVisibility(bool isVisible)
    {
        if(isVisible)
        {
            openButtonImage.sprite= openAlarmMenuSprite;
            closeButtonImage.sprite= closeAlarmMenuSprite;
        }
            
        else
        {
            openButtonImage.sprite=openBaseMenuSprite;
            closeButtonImage.sprite= closeBaseMenuSprite;
        }
            
    }
}
