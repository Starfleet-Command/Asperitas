using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DimBoxes;
using Lean.Common;
using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Niantic.ARDK.Extensions.Gameboard;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;
using UnityEngine.Serialization;

public class EditOnSelect : MonoBehaviour
{
    private GameObject _selectedGameObject;
    
    public void ObjectSelected(LeanSelectByFinger selectedObject, LeanFinger leanFinger)
    {
        if (leanFinger.SnapshotDuration < 0.08f)
            return;

        _selectedGameObject = gameObject;
        if (_selectedGameObject.CompareTag("Creature"))
        {
            GameObject childObject = _selectedGameObject.transform.GetChild(0).gameObject;
            if (childObject.CompareTag("Egg"))
                _selectedGameObject = childObject;
        }
        else
        {
            BiomeEditingEvents.ObjectSelectedEvent(_selectedGameObject);
        }

        if (_selectedGameObject.TryGetComponent<LeanDragTranslate>(out var dragTranslate)) 
            dragTranslate.enabled = true;
        if (_selectedGameObject.TryGetComponent<BoundBox>(out var boundingBox))
        {
            boundingBox.lineColor.a = 200;
            boundingBox.enabled = true;
        }
        
    }
    
    public void ObjectDeselected(LeanSelect deselectedObject)
    {
        if (_selectedGameObject == null)
            return;
        if (_selectedGameObject.CompareTag("Creature"))
        {
            GameObject childObject = _selectedGameObject.transform.GetChild(0).gameObject;
            if (childObject.CompareTag("Egg"))
                _selectedGameObject = childObject;
        }
        if (_selectedGameObject.TryGetComponent<LeanDragTranslate>(out var dragTranslate)) 
            dragTranslate.enabled = false;

        if (_selectedGameObject.TryGetComponent<BoundBox>(out var boundingBox))
        {
            boundingBox.enabled = false;
        }
        BiomeEditingEvents.ObjectDeselectedEvent(_selectedGameObject);
        _selectedGameObject = null;
    }

    public void FunctionalObjectSelected(LeanSelectByFinger selectedObject, LeanFinger leanFinger)
    {
        if (leanFinger.SnapshotDuration < 0.08f)
            return;
        _selectedGameObject = gameObject;
        if (leanFinger.SnapshotDuration > 0.4f)
        {
            if(gameObject.TryGetComponent<ToyBehaviour>(out ToyBehaviour toyTrigger))
            {
                toyTrigger.OnItemInteracted();
            }
        }
        else
        {
            if (_selectedGameObject.TryGetComponent<BoundBox>(out var boundingBox))
            {
                boundingBox.lineColor.a = 200;
                boundingBox.enabled = true;
            }
            BiomeEditingEvents.ObjectSelectedEvent(_selectedGameObject);
        }

    }
    
}