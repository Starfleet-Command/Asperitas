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
using UnityEngine.PlayerLoop;
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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // foreach (GameObject currentObject in _selectedGameObjects)
        // {
        //     BoundTranslation(currentObject);
        // }
    }


    private void BoundTranslation(GameObject item)
    {
        PlacedObjectAttributes ownAttributes;
        item.TryGetComponent<PlacedObjectAttributes>(out ownAttributes);
        
        
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
                Destroy(_selectedGameObjects[0]);
                _selectedGameObjects.RemoveAt(0);
            }
        }
    }

    private void OnDisable()
    {
        BiomeEditingEvents.OnObjectSelected -= EnableDeleteButton;
        BiomeEditingEvents.OnObjectDeselected -= DisableDeleteButton;
        BiomeEditingEvents.OnObjectSelected -= SaveSelectedObject;
        BiomeEditingEvents.OnObjectDeselected -= RemoveSelectedObject;
        
    }

    
}
