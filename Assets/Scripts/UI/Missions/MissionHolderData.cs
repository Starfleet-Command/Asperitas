using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class acts as a data container for mission holders, the representation of each stage of missions, <br/>
/// both in the UI and logically
/// </summary>
public class MissionHolderData : MonoBehaviour
{
    public Text percentageText;
    public Text titleText;
    public Text largeNumberText;
    public GameObject alarmIcon;
    public GameObject missionParent;
    public GameObject backgroundImage;
    public Button expandButton;
    public List<GameObject> missionObjects = new List<GameObject>();
}
