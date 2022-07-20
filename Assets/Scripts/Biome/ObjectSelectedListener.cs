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
    
    private GameObject _selectedGameObject;
    
    private void OnEnable()
    {
        BiomeEditingEvents.OnObjectSelected += EnableDeleteButton;
        BiomeEditingEvents.OnObjectDeselected += DisableDeleteButton;
        BiomeEditingEvents.OnObjectSelected += SaveSelectedObject;
        BiomeEditingEvents.OnObjectDeselected += RemoveSelectedObject;
    }

    private void RemoveSelectedObject(GameObject _item)
    {
        _selectedGameObject = null;
    }

    private void SaveSelectedObject(GameObject item)
    {
        _selectedGameObject = item;
    }

    private void DisableDeleteButton(GameObject item)
    {
        deleteButton.gameObject.SetActive(false);
    }

    private void EnableDeleteButton(GameObject item)
    {
        deleteButton.gameObject.SetActive(true);
    }

    public void DeleteGameObject()
    {
        if (_selectedGameObject != null)
        {
            if(_selectedGameObject.TryGetComponent<PlacedObjectAttributes>(out PlacedObjectAttributes objectData))
            {
                InventoryEvents.ItemCheckedInEvent(objectData.sourceItem);
            }
            Destroy(_selectedGameObject);
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
