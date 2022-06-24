using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeHabitability : MonoBehaviour
{
    public BiomePercentageTuple[] biomeHabitabilityList;

    private void OnEnable()
    {
        BiomeEditingEvents.OnBiomeHabitabilityModified+=ModifyHabitability;
    }

    private void OnDisable()
    {
        BiomeEditingEvents.OnBiomeHabitabilityModified-=ModifyHabitability;
    }


    public void ModifyHabitability(BiomePercentageTuple modifiedTuple)
    {
        foreach(BiomePercentageTuple biomeTuple in biomeHabitabilityList)
        {
            if(biomeTuple.getBiome()==modifiedTuple.getBiome())
            {
                biomeTuple.setBiomeAffinity(biomeTuple.getBiomeAffinity()+modifiedTuple.getBiomeAffinity());
            }
        }
    }
}


[System.Serializable]
public class BiomePercentageTuple
{
    [SerializeField] private BiomeType biome;
    [SerializeField] private int affinityPercentage;

    public BiomeType getBiome()
    {
        return biome;
    }

    public int getBiomeAffinity()
    {
        return affinityPercentage;
    }

    public void setBiomeAffinity(int newAffinity)
    {
        if(newAffinity> 100)
            affinityPercentage=100;

        else
        affinityPercentage = newAffinity;

    }
}
