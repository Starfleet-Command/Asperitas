using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is attached to any creature base, and holds the meshes for all the stages of the creature.<br/>
/// It is also responsible for swapping the creature models during the evolution
/// </summary>
public class CreatureData : MonoBehaviour
{

    [SerializeField] private EvolutionStage creatureStage;
    [SerializeField] private string creatureName; 
    [SerializeField] private EvolutionStageData[] creatureStageData;

    [SerializeField] private GameObject currentCreatureMesh;

    public BiomeType preferredBiome;
    //Add references to lore panel here

    private void OnEnable()
    {
        CreatureEvents.OnCreatureEvolving+=EvolveCreature;
    }

    private void OnDisable()
    {
        CreatureEvents.OnCreatureEvolving-=EvolveCreature;
    }

    // Start is called before the first frame update
    void Start()
    {
        ReplaceCreatureMesh(EvolutionStage.Egg);
    }

    private void EvolveCreature()
    {
        int currentStage = (int)creatureStage;
        currentStage++;
        creatureStage = (EvolutionStage)currentStage;
        ReplaceCreatureMesh(creatureStage);
    }

    /// <summary>
    /// This method replaces the creature's model according to the data for the current stage
    /// </summary>
    private void ReplaceCreatureMesh(EvolutionStage newStage)
    {
        GameObject newMesh = null;
        foreach (EvolutionStageData stage in creatureStageData)
        {
            if(stage.creatureStage==newStage)
            {
                newMesh = stage.stageCreaturePrefab;
                break;
            }
            
        }

        if(newMesh!=null)
        {
            GameObject newMeshInstance = Instantiate(newMesh,currentCreatureMesh.transform.position,newMesh.transform.rotation);
            newMeshInstance.transform.SetParent(this.gameObject.transform);
            if(currentCreatureMesh!= null)
                Destroy(currentCreatureMesh);

            currentCreatureMesh = newMeshInstance;
        }
        
        
    }
}

[System.Serializable]
public class EvolutionStageData
{
    public EvolutionStage creatureStage;
    public GameObject stageCreaturePrefab;
}
