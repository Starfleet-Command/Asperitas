using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(this.gameObject.transform.childCount >0 && currentCreatureMesh==null)
        {
            currentCreatureMesh = this.gameObject.transform.GetChild(0).gameObject;
        }
    }

    private void EvolveCreature()
    {
        int currentStage = (int)creatureStage;
        currentStage++;
        creatureStage = (EvolutionStage)currentStage;
        ReplaceCreatureMesh(creatureStage);
    }

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
