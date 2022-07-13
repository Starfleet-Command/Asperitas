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

}
