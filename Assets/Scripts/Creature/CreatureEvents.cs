using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureEvents : MonoBehaviour
{
    public delegate void MissionProgressed(ChecklistMission _mission);

    public static event MissionProgressed OnMissionProgressed;

    public static void MissionProgressedEvent(ChecklistMission _mission)
    {
        OnMissionProgressed(_mission);
    }

    public delegate void MissionFinished(ChecklistMission _finishedMission);

    public static event MissionFinished OnMissionFinished;

    public static void MissionFinishedEvent(ChecklistMission _finishedMission)
    {
        OnMissionFinished(_finishedMission);
    }

    public delegate void ChecklistFinished();

    public static event ChecklistFinished OnChecklistFinished;

    public static void ChecklistFinishedEvent()
    {
        OnChecklistFinished();
    }

    public delegate void FriendshipGained(float _friendship);

    public static event FriendshipGained OnFriendshipGained;

    public static void FriendshipGainedEvent(float _friendship)
    {
        OnFriendshipGained(_friendship);
    }

    public delegate void CreatureEvolving();

    public static event CreatureEvolving OnCreatureEvolving;

    public static void CreatureEvolvingEvent()
    {
        OnCreatureEvolving();
    }

    public delegate void CreatureEvolved();

    public static event CreatureEvolved OnCreatureEvolved;

    public static void CreatureEvolvedEvent()
    {
        OnCreatureEvolved();
    }

}
