using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This class acts as a data holder for each programatically generated individual mission UI element <br/>
/// </summary>
public class MissionEntryData : MonoBehaviour
{
    public Text progressText;
    public Text descriptionText;
    public Slider progressBar;
    public ChecklistMission trackedMission;
}
