using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
