using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiEvents : MonoBehaviour
{
    public delegate void IsThrowingStatusChanged(bool _status);

    public static event IsThrowingStatusChanged OnIsThrowingStatusChanged;

    public static void IsThrowingStatusChangedEvent(bool _status)
    {
        OnIsThrowingStatusChanged(_status);
    }

    public delegate void ChangeAlarmState(bool _newState);

    public static event ChangeAlarmState OnChangeAlarmState;
    
    public static void ChangeAlarmStateEvent(bool _newState)
    {
        OnChangeAlarmState(_newState);
    }

    public delegate void MissionSetChanged(int _newSet);

    public static event MissionSetChanged OnMissionSetChanged;

    public static void MissionSetChangedEvent(int _newSet)
    {
        OnMissionSetChanged(_newSet);
    }

    public delegate void PlacementCanceled();

    public static event PlacementCanceled OnPlacementCanceled;

    public static void PlacementCanceledEvent()
    {
        OnPlacementCanceled();
    }

}
