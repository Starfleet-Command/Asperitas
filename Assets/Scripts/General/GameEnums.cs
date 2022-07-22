using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnums : MonoBehaviour
{

}
public enum ItemTag
{
    Stackable,
    NonStackable,
    Foundation,
    Machine,
    Nature,
    Required,
    Small,
    Medium,
    Large,
    None,
}


public enum MissionType
{
    PlaceItems,
    BiomePercentage,
    FriendshipPercentage,
    TimesFed,
    TimesInteracted,
}

public enum StackabilityType
{
    Gameboard,
    Foundation,
    Stackable,
    Nonstackable,
}

public enum EvolutionStage
{
    Egg,
    Baby,
    Medium,
    Large
}

public enum BiomeType
{
    Ocean,
    Plains,
    Desert,
    None
}

public enum InteractionSocketType
{
    Feeding,
    Petting,
    Playing,
}