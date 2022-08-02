using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Niantic.ARDK.Extensions.Gameboard;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;
using UnityEngine.Serialization;

/// <summary>
/// This class has the controls for deleting or modifying selected objects
/// </summary>
public class ObjectSelectedListener : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Delete Button")]
    private Button deleteButton;
    // Start is called before the first frame update
    
    private List<GameObject> _selectedGameObjects = new List<GameObject>();
    
    /// <summary>
    /// On enable, subscribe the functions to OnObjectSelected event
    /// </summary>
    private void OnEnable()
    {
        BiomeEditingEvents.OnObjectSelected += EnableDeleteButton;
        BiomeEditingEvents.OnObjectDeselected += DisableDeleteButton;
        BiomeEditingEvents.OnObjectSelected += SaveSelectedObject;
        BiomeEditingEvents.OnObjectDeselected += RemoveSelectedObject;
    }

    /// <summary>
    /// Remove object from the list of selected objects
    /// </summary>
    /// <param name="item"> object to be removed </param>
    private void RemoveSelectedObject(GameObject item)
    {
        _selectedGameObjects.Remove(item);
    }

    /// <summary>
    /// Add object to the list of selected objects
    /// </summary>
    /// <param name="item"> object to be added </param>
    private void SaveSelectedObject(GameObject item)
    {
        _selectedGameObjects.Add(item);
    }

    /// <summary>
    /// If this was the last selected object then deactivate the delete button
    /// </summary>
    /// <param name="item"></param>
    private void DisableDeleteButton(GameObject item)
    {
        // ToDo: check if it will solve the break on accessing the removed button
        if (_selectedGameObjects.Count == 1)
        {
            deleteButton.gameObject.SetActive(false);
        }
        
    }

    /// <summary>
    /// Enable delete button
    /// </summary>
    private void EnableDeleteButton(GameObject item)
    {
        deleteButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Delete all selected objects.
    /// Return the object to the inventory and decrease the biome meter.
    /// TODO: Should also decrease the mission meter.
    /// </summary>
    public void DeleteGameObject()
    {
        while (_selectedGameObjects.Count > 0)
        {
            if (_selectedGameObjects[0] != null)
            {
                if(_selectedGameObjects[0].TryGetComponent<PlacedObjectAttributes>(out PlacedObjectAttributes objectData))
                {
                    InventoryEvents.ItemCheckedInEvent(objectData.sourceItem);
                }
                BiomePercentageTuple inverseEffectTuple = new BiomePercentageTuple(objectData.biomeEffect.getBiome(),-1*objectData.biomeEffect.getBiomeAffinity(),objectData.biomeEffect.getBiomeIcon());
                BiomeEditingEvents.BiomeHabitabilityModifiedEvent(inverseEffectTuple);
                Destroy(_selectedGameObjects[0]);
                _selectedGameObjects.RemoveAt(0);
            }
        }
    }
    
    /// <summary>
    /// On disable, unsubscribe the functions to OnObjectSelected event
    /// </summary>
    private void OnDisable()
    {
        BiomeEditingEvents.OnObjectSelected -= EnableDeleteButton;
        BiomeEditingEvents.OnObjectDeselected -= DisableDeleteButton;
        BiomeEditingEvents.OnObjectSelected -= SaveSelectedObject;
        BiomeEditingEvents.OnObjectDeselected -= RemoveSelectedObject;
        
    }
}
