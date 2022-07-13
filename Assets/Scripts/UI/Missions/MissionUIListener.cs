using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MissionUIListener : MonoBehaviour
{
    public ChecklistMission trackedMission;
    public Slider missionSlider;
    public Text progressText;
    public Text flavourText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        CreatureEvents.OnMissionProgressed+=CheckForUpdates;
    }

    private void OnDisable()
    {
        CreatureEvents.OnMissionProgressed-=CheckForUpdates;
    }

    private void CheckForUpdates(ChecklistMission _progressedMission)
    {
        if(_progressedMission.getMissionID() == trackedMission.getMissionID())
        {
            UpdateUI(_progressedMission);
        }
    }

    private void UpdateUI(ChecklistMission _progressedMission)
    {
        missionSlider.value=_progressedMission.getMissionProgress();
        progressText.text = " "+_progressedMission.getMissionProgress()+"/"+ _progressedMission.getRequiredProgress();
    }
}
