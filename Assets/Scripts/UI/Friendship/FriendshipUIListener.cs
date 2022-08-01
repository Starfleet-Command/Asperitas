using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class updates the friendship bar whenever friendship is gained in the course of the game
/// </summary>
public class FriendshipUIListener : MonoBehaviour
{
    public Slider friendshipBar;
    public float savedCurrentFriendship;
    public float savedMaxFriendship;

    private void OnEnable()
    {
        CreatureEvents.OnFriendshipGained+=UpdateFriendshipBar;
        CreatureEvents.OnCreaturePlaced+=ReadCreatureSettings;
    }

        private void OnDisable()
    {
        CreatureEvents.OnFriendshipGained-=UpdateFriendshipBar;
        CreatureEvents.OnCreaturePlaced-=ReadCreatureSettings;
    }

    /// <summary>
    /// This method reads the friendship settings from a creature after it's been placed, and adjusts the slider settings accordingly
    /// </summary>
    public void ReadCreatureSettings(GameObject _creature)
    {
        CreatureFriendship creatureFriendshipScript = null;

        if(_creature.TryGetComponent<CreatureFriendship>(out creatureFriendshipScript))
        {
            InitialSliderSetup(creatureFriendshipScript.getCurrentFriendship(),creatureFriendshipScript.getMaxFriendship());
        }
    }

    public void InitialSliderSetup(float currentFriendship,float maxFriendship)
    {
        friendshipBar.minValue = 0;
        friendshipBar.value = currentFriendship;
        friendshipBar.maxValue = maxFriendship;
        savedMaxFriendship = maxFriendship;
        savedCurrentFriendship = currentFriendship;
    }

    public void UpdateFriendshipBar(float friendshipToAdd)
    {
        savedCurrentFriendship+=friendshipToAdd;
        savedCurrentFriendship = Mathf.Clamp(savedCurrentFriendship,0,savedMaxFriendship); 
        savedCurrentFriendship = Mathf.Round(savedCurrentFriendship);  
           
        friendshipBar.value = savedCurrentFriendship;
    }
}
