using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HabitabilityUIListener : MonoBehaviour
{
    
    [SerializeField] private Slider habitatSlider;
    [SerializeField] private Image biomeIcon;
    private BiomeType currentBiome;
    private bool isSliderInactive=true;
    private BiomeHabitability habitatScript;

    private void Start()
    {
        CreatureScriptReferences levelData = CreatureScriptReferences.Instance;
        habitatScript = levelData.habitabilityTrackingScript;
    }

    private void OnEnable()
    {
        BiomeEditingEvents.OnBiomeHabitabilityModified+=UpdateHabitatValues;
        CreatureEvents.OnCreaturePlaced+=ChangeTrackedBiome;
    }

    private void OnDisable()
    {
        BiomeEditingEvents.OnBiomeHabitabilityModified-=UpdateHabitatValues;
        CreatureEvents.OnCreaturePlaced-=ChangeTrackedBiome;
    }

    private void UpdateHabitatValues(BiomePercentageTuple _ignoreThisItem)
    {
        habitatSlider.value=habitatScript.getBiomeTuple(currentBiome).getBiomeAffinity();
    }

    private void ChangeTrackedBiome(GameObject _creature)
    {
        if(_creature.TryGetComponent<CreatureData>(out CreatureData creatureDataScript))
        {
            BiomePercentageTuple newTrackedBiome;

            currentBiome = creatureDataScript.preferredBiome;
            newTrackedBiome = habitatScript.getBiomeTuple(currentBiome);

            if(newTrackedBiome.getBiomeIcon()!=null)
            {
                biomeIcon.gameObject.SetActive(true);
                biomeIcon.sprite= newTrackedBiome.getBiomeIcon();
            }
                

            if(isSliderInactive)
            {
                habitatSlider.enabled=true;
                isSliderInactive=false;
            }
            
            habitatSlider.value=newTrackedBiome.getBiomeAffinity();
        }
    }

}
