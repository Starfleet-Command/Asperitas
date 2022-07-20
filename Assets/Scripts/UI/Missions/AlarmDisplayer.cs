using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject alarmIconObject;
    
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
        alarmIconObject.SetActive(isVisible);
    }
}
