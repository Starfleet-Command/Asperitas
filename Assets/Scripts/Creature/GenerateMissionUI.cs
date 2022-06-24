using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMissionUI : MonoBehaviour
{
    [SerializeField] private ChecklistMissionSystem missionSystem;
    [SerializeField] private GameObject missionUIPrefab;

    [SerializeField] private GameObject canvasParent;
    private MissionUIListener instanceListener;

    private void Start()
    {

    }


    public void GenerateAllUI(ChecklistMission[] _currentChecklist)
    {
        foreach(Transform child in canvasParent.transform)
        {
                Destroy(child.gameObject);
        }

        foreach (ChecklistMission _mission in _currentChecklist)
        {
            GameObject uiInstance = Instantiate(missionUIPrefab);
            if(uiInstance.TryGetComponent<MissionUIListener>(out instanceListener))
            {
                instanceListener.flavourText.text = _mission.getMissionText();
                instanceListener.missionSlider.minValue = _mission.getMissionProgress();
                instanceListener.missionSlider.maxValue = _mission.getRequiredProgress();
                instanceListener.progressText.text = " "+_mission.getMissionProgress()+"/"+ _mission.getRequiredProgress();
                instanceListener.trackedMission = _mission;
            }
            uiInstance.transform.SetParent(canvasParent.transform);
        }
    }

}
