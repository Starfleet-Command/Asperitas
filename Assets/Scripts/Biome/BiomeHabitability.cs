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

    public BiomePercentageTuple getBiomeTuple(BiomeType chosenBiome)
    {
        foreach (BiomePercentageTuple tuple in biomeHabitabilityList)
        {
            if(tuple.getBiome() == chosenBiome)
            {
                return tuple;
            }
        }
        return null;
    }
}


[System.Serializable]
public class BiomePercentageTuple
{
    [SerializeField] private BiomeType biome;
    [SerializeField] private int affinityPercentage;
    [SerializeField] private Sprite biomeIcon;

    public BiomePercentageTuple(BiomeType _biome, int _affinityPercentage, Sprite _biomeIcon)
    {
        biome = _biome;
        affinityPercentage = _affinityPercentage;
        biomeIcon = _biomeIcon;
    }


    public BiomeType getBiome()
    {
        return biome;
    }

    public int getBiomeAffinity()
    {
        return affinityPercentage;
    }

    public Sprite getBiomeIcon()
    {
        return biomeIcon;
    }

    public void setBiomeAffinity(int newAffinity)
    {
        if(newAffinity> 100)
            affinityPercentage=100;

        if(newAffinity< 0)
            affinityPercentage=0;

        else
        affinityPercentage = newAffinity;

    }
}
