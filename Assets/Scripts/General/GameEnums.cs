using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is only used to centralise all enums used in the game. <br/>
/// Enums are used to simplify in-editor parameter modification
/// </summary>
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
    Egg,
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