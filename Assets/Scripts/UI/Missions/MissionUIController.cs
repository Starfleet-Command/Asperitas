using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MissionUIController : MonoBehaviour
{
    [SerializeField] private GameObject[] stageMissionHolders;
    [SerializeField] private GameObject[] connectingArrows;
    [SerializeField] private GameObject missionCanvas;
    [SerializeField] private Color activeMissionColor;
    [SerializeField] private Sprite activeMissionHolderSprite;
    [SerializeField] private Color filledSliderColor;
    [SerializeField] private GameObject missionEntryPrefab;
    private ChecklistMissionSystem missionDatabase;
    
    private int currentStage=0;
    // Start is called before the first frame update
    void Start()
    {
        CreatureScriptReferences levelData = CreatureScriptReferences.Instance;
        missionDatabase = levelData.missionSystemScript;
        FirstTimeBuildUI();
    }

    
    private void OnEnable()
    {
        CreatureEvents.OnMissionProgressed+=PollForMissionUpdates;
        CreatureEvents.OnMissionFinished+=HandleMissionFinished;
        UiEvents.OnMissionSetChanged+=HandleChecklistFinished;
    }

    private void OnDisable()
    {
        CreatureEvents.OnMissionProgressed-=PollForMissionUpdates;
        CreatureEvents.OnMissionFinished-=HandleMissionFinished;
        UiEvents.OnMissionSetChanged-=HandleChecklistFinished;
    }


    public void PollForMissionUpdates(ChecklistMission _progressedMission)
    {
        
        GameObject currentStageHolder = stageMissionHolders[currentStage];
        if(currentStageHolder.TryGetComponent<MissionHolderData>(out MissionHolderData _holderData))
        {
            
            foreach (GameObject missionObj in _holderData.missionObjects)
            {
                if(missionObj.TryGetComponent<MissionEntryData>(out MissionEntryData _entryData))
                {
                    if(_entryData.trackedMission.getMissionID()==_progressedMission.getMissionID())
                    {
                        int missionProgressPercent = Mathf.RoundToInt(((float)_progressedMission.getMissionProgress()/(float)_progressedMission.getRequiredProgress())*100);
                        
                        _entryData.progressBar.value= _progressedMission.getMissionProgress();
                        _entryData.progressText.text = ""+missionProgressPercent+"%";

                        if(missionProgressPercent==100)
                        {
                            _entryData.progressBar.fillRect.gameObject.GetComponent<Image>().color=filledSliderColor;
                        }
                    }
                }
            }
        }
    }

    public void HandleMissionFinished(ChecklistMission _finishedMission)
    {
        
        GameObject currentStageHolder = stageMissionHolders[currentStage];
        if(currentStageHolder.TryGetComponent<MissionHolderData>(out MissionHolderData _holderData))
        {
            _holderData.percentageText.text= ""+GetCategoryProgressPercentage()+"%";
            _holderData.alarmIcon.SetActive(true);
            
        }
        UiEvents.ChangeAlarmStateEvent(true);
    }

    public void HandleChecklistFinished(int newStage)
    {
        if (connectingArrows.Length > currentStage)
        {
            if(connectingArrows[currentStage].TryGetComponent<Text>(out Text _arrow))
            {
                _arrow.color=activeMissionColor;
            }
        }
        

        currentStage=newStage;

        if(stageMissionHolders.Length>currentStage)
        {
            SetHolderAsInteractable();
        }
    }

    public void OpenUI()
    {
        
        missionCanvas.SetActive(true);
    }

    private void FirstTimeBuildUI()
    {
        
        missionCanvas.SetActive(true);
        int currentSet = 0;
        MissionHolderData _holderData;
        foreach (ChecklistWrapper missionSet in missionDatabase.allMissions)
        {
            _holderData = stageMissionHolders[currentSet].GetComponent<MissionHolderData>();

            foreach(ChecklistMission mission in missionSet.stageChecklist)
            {
                GameObject missionEntry = Instantiate(missionEntryPrefab);
                if(missionEntry.TryGetComponent<MissionEntryData>(out MissionEntryData _entryData))
                {
                    _entryData.trackedMission = mission;
                    _entryData.descriptionText.text = mission.getMissionText();
                    _entryData.progressBar.maxValue = mission.getRequiredProgress();
                    _entryData.progressBar.value = mission.getMissionProgress();
                    _entryData.progressText.text="0%";
                }

                _holderData.missionObjects.Add(missionEntry);
                missionEntry.transform.SetParent(_holderData.missionParent.transform,false);
            }
            currentSet++;
        }
        
        SetHolderAsInteractable();
        missionCanvas.SetActive(false);    

        
    }

    private void SetHolderAsInteractable()
    {
        GameObject currentStageHolder = stageMissionHolders[currentStage];
        MissionHolderData _holderData;
        if(currentStageHolder.TryGetComponent<MissionHolderData>(out _holderData))
        {
            _holderData.percentageText.color=Color.black;
            _holderData.titleText.color=Color.black;
            _holderData.largeNumberText.color=Color.black;
            _holderData.backgroundImage.GetComponent<Image>().sprite = activeMissionHolderSprite;
            _holderData.backgroundImage.GetComponent<Image>().color=Color.white;
            _holderData.expandButton.interactable=true;
        }
    }


    private int GetCategoryProgressPercentage()
    {
        float completedMissions = 0;
        float requiredMissions = 0;

        foreach (ChecklistMission mission in missionDatabase.currentChecklist)
        {
            if(mission.getRequiredStatus())
            {
                requiredMissions++;

                if(mission.getMissionStatus())
                {
                    completedMissions++;
                }
            }
        }
        
        return Mathf.RoundToInt((completedMissions/requiredMissions)*100);
    }
}
