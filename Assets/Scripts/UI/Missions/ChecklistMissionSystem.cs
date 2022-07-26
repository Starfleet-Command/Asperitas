using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChecklistMissionSystem : MonoBehaviour
{
    [SerializeField] private BiomeHabitability habitabilityTrackingScript;

    [SerializeField] private CreatureFriendship creatureFriendshipScript;
    [SerializeField] private MissionScriptableObject missionData;
    public ChecklistWrapper[] allMissions;
    [HideInInspector] public ChecklistMission[] currentChecklist;
    [HideInInspector] public int currentStage=0;
    private void OnEnable()
    {
        MissionScriptableObject dataInstance = Instantiate<MissionScriptableObject>(missionData);
        allMissions = dataInstance.missions;
        
        BiomeEditingEvents.OnItemPlaced+=PlacedMissionProgressedCheck;
        CreatureEvents.OnMissionFinished+=StageCompleteCheck;
        CreatureEvents.OnCreatureEvolving+=SwitchChecklist;
        CreatureEvents.OnFriendshipGained+=FriendshipMissionProgressedCheck;
        CreatureEvents.OnCreaturePlaced+=GetFriendshipScript;
        CreatureEvents.OnInteractionTriggered+=InteractionMissionProgressedCheck;
        BiomeEditingEvents.OnBiomeHabitabilityModified+=BiomeMissionProgressedCheck;
    }

    private void OnDisable()
    {
        BiomeEditingEvents.OnItemPlaced-=PlacedMissionProgressedCheck;
        CreatureEvents.OnMissionFinished-=StageCompleteCheck;
        CreatureEvents.OnCreatureEvolving-=SwitchChecklist;
        CreatureEvents.OnFriendshipGained-=FriendshipMissionProgressedCheck;
        CreatureEvents.OnCreaturePlaced+=GetFriendshipScript;
        CreatureEvents.OnInteractionTriggered-=InteractionMissionProgressedCheck;
        BiomeEditingEvents.OnBiomeHabitabilityModified-=BiomeMissionProgressedCheck;
    }

    private void Start()
    {
        currentChecklist = allMissions[currentStage].stageChecklist;
    }

    private void GetFriendshipScript(GameObject _creatureObject)
    {
        _creatureObject.TryGetComponent<CreatureFriendship>(out creatureFriendshipScript);
    }

    //Currently only checks for placed object progress. 
    private void PlacedMissionProgressedCheck(GameObject placedItem)
    {
        ItemTag[] itemTags = placedItem.GetComponent<PlacedObjectAttributes>().sourceItem.getTags();

        

        foreach (ChecklistMission mission in currentChecklist)
        {
            if(!mission.CheckMissionComplete() && mission.getMissionType()==MissionType.PlaceItems)
            {
                foreach(ItemTag tag in itemTags)
                {
                
                    if(tag == mission.getMissionTag())
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
                HandleMissionEvents(mission,Mathf.RoundToInt(friendshipGain));
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

    private void InteractionMissionProgressedCheck(InteractionSocketType missionType)
    {
        if(missionType==InteractionSocketType.Feeding)
        {
            foreach(ChecklistMission mission in currentChecklist)
            {
                if(!mission.CheckMissionComplete() && mission.getMissionType()==MissionType.TimesFed)
                {
                    HandleMissionEvents(mission,1);
                }
            }
        }
        else
        {
            foreach(ChecklistMission mission in currentChecklist)
            {
                if(!mission.CheckMissionComplete() && mission.getMissionType()==MissionType.TimesInteracted)
                {
                    HandleMissionEvents(mission,1);
                }
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
        if (allMissions.Length <= currentStage) 
            return;
        UiEvents.MissionSetChangedEvent(currentStage);
        currentChecklist = allMissions[currentStage].stageChecklist;
        
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

            else if(mission.getMissionType()==MissionType.FriendshipPercentage)
            {
                HandleMissionEvents(mission,(int)creatureFriendshipScript.getCurrentFriendship());
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

    public ChecklistWrapper(ChecklistMission[] missions)
    {
        stageChecklist = missions;
    }
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

    public ChecklistMission(ChecklistMission _previousMission)
    {
        missionID = _previousMission.getMissionID();
        flavourText = _previousMission.getMissionText();
        missionType = _previousMission.getMissionType();
        missionBiomeType = _previousMission.GetBiomeType();
        missionRequiredTag = _previousMission.getMissionTag();
        missionThreshold = _previousMission.getRequiredProgress();
        isRequired = _previousMission.getRequiredStatus();
        missionProgress = _previousMission.getMissionProgress();
        isMissionComplete = _previousMission.getMissionStatus();
    }

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

