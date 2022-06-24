using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChecklistMissionSystem : MonoBehaviour
{
    private int currentStage=0;

    [SerializeField] private GenerateMissionUI uiFromChecklistScript;
    [SerializeField] private BiomeHabitability habitabilityTrackingScript;
    public ChecklistWrapper[] allMissions;
    [HideInInspector] public ChecklistMission[] currentChecklist;
    private void OnEnable()
    {
        BiomeEditingEvents.OnItemPlaced+=PlacedMissionProgressedCheck;
        CreatureEvents.OnMissionFinished+=StageCompleteCheck;
        CreatureEvents.OnCreatureEvolving+=SwitchChecklist;
        CreatureEvents.OnFriendshipGained+=FriendshipMissionProgressedCheck;
        BiomeEditingEvents.OnBiomeHabitabilityModified+=BiomeMissionProgressedCheck;
    }

    private void OnDisable()
    {
        BiomeEditingEvents.OnItemPlaced-=PlacedMissionProgressedCheck;
        CreatureEvents.OnMissionFinished-=StageCompleteCheck;
        CreatureEvents.OnCreatureEvolving-=SwitchChecklist;
        CreatureEvents.OnFriendshipGained-=FriendshipMissionProgressedCheck;
        BiomeEditingEvents.OnBiomeHabitabilityModified-=BiomeMissionProgressedCheck;
    }

    private void Start()
    {
        currentChecklist = allMissions[currentStage].stageChecklist;
        uiFromChecklistScript.GenerateAllUI(currentChecklist);
    }

    //Currently only checks for placed object progress. 
    private void PlacedMissionProgressedCheck(GameObject placedItem)
    {
        int[] itemTags = placedItem.GetComponent<PlacedObjectAttributes>().sourceItem.getTags();

        

        foreach (ChecklistMission mission in currentChecklist)
        {
            if(!mission.CheckMissionComplete() && mission.getMissionType()==MissionType.PlaceItems)
            {
                foreach(int tag in itemTags)
                {
                
                    if(tag == mission.getMissionTagAsInt())
                    {
                        HandleMissionEvents(mission,1);
                        break;
                    }
                }
            }
            
        }
    }

    private void FriendshipMissionProgressedCheck(float friendshipGain)
    {
        foreach(ChecklistMission mission in currentChecklist)
        {
            if(!mission.CheckMissionComplete() && mission.getMissionType() == MissionType.FriendshipPercentage)
            {
                HandleMissionEvents(mission,Mathf.CeilToInt(friendshipGain));
            }
        }
    }

    private void BiomeMissionProgressedCheck(BiomePercentageTuple modifiedBiome)
    {
        foreach(ChecklistMission mission in currentChecklist)
        {
            if(!mission.CheckMissionComplete() && mission.getMissionType()==MissionType.BiomePercentage)
            {
                HandleMissionEvents(mission,modifiedBiome.getBiomeAffinity());
            }
        }
    }

    private void StageCompleteCheck(ChecklistMission _completedMission)
    {
        bool isComplete = true;
        foreach (ChecklistMission mission in currentChecklist)
        {
            if(mission.getRequiredStatus() && !mission.getMissionStatus())
            {
                isComplete = false;
            }
        }

        if(isComplete)
        {
            CreatureEvents.ChecklistFinishedEvent();
        }
    }

    private void SwitchChecklist()
    {
        currentStage++;
        currentChecklist = allMissions[currentStage].stageChecklist;
        uiFromChecklistScript.GenerateAllUI(currentChecklist);
        PollFriendshipAndBiomeStatus();
    }

    private void PollFriendshipAndBiomeStatus()
    {
        foreach(ChecklistMission mission in currentChecklist)
        {
            if(mission.getMissionType()==MissionType.BiomePercentage)
            {
                foreach(BiomePercentageTuple biomeStatus in habitabilityTrackingScript.biomeHabitabilityList)
                {
                    if(biomeStatus.getBiome()==mission.GetBiomeType())
                    {
                        HandleMissionEvents(mission,biomeStatus.getBiomeAffinity());
                    }
                }
            }
        }
        
    }
    
    private void HandleMissionEvents(ChecklistMission _mission, int newMissionProgress)
    {
        _mission.setMissionProgress(_mission.getMissionProgress()+newMissionProgress);
        CreatureEvents.MissionProgressedEvent(_mission);
        if(_mission.CheckMissionComplete())
            CreatureEvents.MissionFinishedEvent(_mission);
    }
}

[System.Serializable]
public class ChecklistWrapper
{
    public ChecklistMission[] stageChecklist;
}

[System.Serializable]
public class ChecklistMission
{
    [SerializeField] private int missionID;
    [SerializeField]private string flavourText;
    [SerializeField]private MissionType missionType;
    [SerializeField] private BiomeType missionBiomeType;
    [SerializeField]private ItemTag missionRequiredTag;
    [SerializeField]private int missionThreshold;

    [SerializeField]private bool isRequired;
    [SerializeField]private int missionProgress;
    [SerializeField]private bool isMissionComplete;

    public string getMissionText()
    {
        return flavourText;
    }
    
    public int getMissionID()
    {
        return missionID;
    }

    public MissionType getMissionType()
    {
        return missionType;
    }

    public BiomeType GetBiomeType()
    {
        if(missionType!= MissionType.BiomePercentage)
        {
            return BiomeType.None;
        }
        else return missionBiomeType;
    }

    public int getMissionProgress()
    {
        return missionProgress;
    }

    public int getRequiredProgress()
    {
        return missionThreshold;
    }
    public void setMissionProgress(int progress)
    {
        if(progress>missionThreshold)
            missionProgress = missionThreshold;

        else missionProgress = progress;
    }

    public ItemTag getMissionTag()
    {
        return missionRequiredTag;
    }

    public int getMissionTagAsInt()
    {
        return (int)missionRequiredTag;
    }

    public bool getMissionStatus()
    {
        return isMissionComplete;
    }

    public bool getRequiredStatus()
    {
        return isRequired;
    }

    public bool CheckMissionComplete()
    {
        if(this.missionProgress >= this.missionThreshold)
        {
            isMissionComplete = true;
            return true;
        }
        else return false;
    }

}

