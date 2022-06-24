using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Intended to go on a creature, same as CreatureData. 
public class CreatureFriendship : MonoBehaviour
{
    [SerializeField] private float frGrowthMultiplier=0.5f;
    [SerializeField] private float currentFriendship;
    [SerializeField] private float maxFriendship=100f;
    [SerializeField] private float biomeToGrowthMult=0.01f;

    [SerializeField] private CreatureData _creatureData;

    // Start is called before the first frame update

    private void OnEnable()
    {
        CreatureEvents.OnFriendshipGained+=AddFriendship;
        BiomeEditingEvents.OnBiomeHabitabilityModified+=CheckBiomeChange;
    }

    private void OnDisable()
    {
        CreatureEvents.OnFriendshipGained-=AddFriendship;
    }

    void Start()
    {
        
    }

    public void AddFriendship(float frGrowth)
    {
        currentFriendship += (frGrowth*frGrowthMultiplier);
    }

    public void ShowFriendship()
    {

    }

    //If changed biome is creature's preferred biome, adjust the rate of friendship growth.  
    private void CheckBiomeChange(BiomePercentageTuple changedBiome)
    {
        if(changedBiome.getBiome()==_creatureData.preferredBiome)
        {
            ModifyMultiplier(frGrowthMultiplier+ (changedBiome.getBiomeAffinity()* biomeToGrowthMult));
        }
    }

    public void ModifyMultiplier(float newMultiplier)
    {
        frGrowthMultiplier = newMultiplier;
    }
}
