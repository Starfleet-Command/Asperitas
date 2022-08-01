using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Intended to go on a creature, same as CreatureData. 
/// <summary>
/// This class is attached to any creature base, and holds the settings for friendship and friendship growth
/// </summary>
public class CreatureFriendship : MonoBehaviour
{
    [SerializeField] private float frGrowthMultiplier=0.5f;
    [SerializeField] private float currentFriendship;
    [SerializeField] private float maxFriendship=100f;
    [SerializeField] private float maxBiomeAffinity=100f;
    [SerializeField] private float biomeToGrowthMult=0.01f;

    [SerializeField] private CreatureData _creatureData;
    [SerializeField] private BondGainPerInteraction[] friendshipGainTable;

    private void OnEnable()
    {
        CreatureEvents.OnFriendshipGained+=AddFriendship;
        BiomeEditingEvents.OnBiomeHabitabilityModified+=CheckBiomeChange;
        CreatureEvents.OnInteractionTriggered+=ProcessInteraction;
    }

    private void OnDisable()
    {
        CreatureEvents.OnFriendshipGained-=AddFriendship;
        BiomeEditingEvents.OnBiomeHabitabilityModified-=CheckBiomeChange;
        CreatureEvents.OnInteractionTriggered-=ProcessInteraction;
    }

    void Start()
    {
        CreatureEvents.CreaturePlacedEvent(this.gameObject);
    }

    private void ProcessInteraction(InteractionSocketType _interactionType)
    {
        foreach (BondGainPerInteraction tuple in friendshipGainTable)
        {
            if(tuple.interactionType==_interactionType)
            {
                CreatureEvents.FriendshipGainedEvent(tuple.friendshipGain*frGrowthMultiplier);
            }
        }
    }

    public void AddFriendship(float frGrowth)
    {
        currentFriendship += frGrowth;
        currentFriendship = Mathf.Clamp(currentFriendship,0,maxFriendship);
        currentFriendship = Mathf.Round(currentFriendship);
    }

    //If changed biome is creature's preferred biome, adjust the rate of friendship growth.  
    private void CheckBiomeChange(BiomePercentageTuple changedBiome)
    {
        if(changedBiome.getBiome()==_creatureData.preferredBiome)
        {
            if(changedBiome.getBiomeAffinity()<=maxBiomeAffinity)
                ModifyMultiplier(frGrowthMultiplier+ (changedBiome.getBiomeAffinity()* biomeToGrowthMult));
        }
    }

    public void ModifyMultiplier(float newMultiplier)
    {
        frGrowthMultiplier = newMultiplier;
    }

    public float getCurrentFriendship()
    {
        return currentFriendship;
    }

    public float getMaxFriendship()
    {
        return maxFriendship;
    }
}

[System.Serializable]
public class BondGainPerInteraction
{
    public InteractionSocketType interactionType;
    public float friendshipGain;
}
