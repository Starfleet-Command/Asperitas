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


public class ObjectSelectedListener : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Delete Button")]
    private Button deleteButton;
    // Start is called before the first frame update
    
    private List<GameObject> _selectedGameObjects = new List<GameObject>();
    
    private void OnEnable()
    {
        BiomeEditingEvents.OnObjectSelected += EnableDeleteButton;
        BiomeEditingEvents.OnObjectDeselected += DisableDeleteButton;
        BiomeEditingEvents.OnObjectSelected += SaveSelectedObject;
        BiomeEditingEvents.OnObjectDeselected += RemoveSelectedObject;
    }

    private void RemoveSelectedObject(GameObject item)
    {
        _selectedGameObjects.Remove(item);
    }

    private void SaveSelectedObject(GameObject item)
    {
        _selectedGameObjects.Add(item);
    }

    private void DisableDeleteButton(GameObject item)
    {
        if (_selectedGameObjects.Count <= 1)
        {
            deleteButton.gameObject.SetActive(false);
        }
        
    }

    private void EnableDeleteButton(GameObject item)
    {
        deleteButton.gameObject.SetActive(true);
    }

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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        BiomeEditingEvents.OnObjectSelected -= EnableDeleteButton;
        BiomeEditingEvents.OnObjectDeselected -= DisableDeleteButton;
        BiomeEditingEvents.OnObjectSelected -= SaveSelectedObject;
        BiomeEditingEvents.OnObjectDeselected -= RemoveSelectedObject;
        
    }

    
}
