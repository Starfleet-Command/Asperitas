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

/// <summary>
/// This Class is responsible for handling the selected object attributes and functionalities. 
/// </summary>
public class EditOnSelect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Minimum touch threshold")]
    private float minThreshold;
    
    [SerializeField]
    [Tooltip("Minimum Long Press threshold")]
    private float minLongPressThreshold;
    
    private GameObject _selectedGameObject;
    
    /// <summary>
    /// This function will check for the duration of touch.
    /// Then set the following selected game object's components settings:
    /// - Enable LeanDragTranslate from the LeanTouch plugin
    /// - Enable the bounding box for the object from the BoundBox plugin
    ///
    /// It will also enable delete button for all none creature objects.
    /// </summary>
    /// <param name="selectedObject"> The selected object (current object this script is attached to)</param>
    /// <param name="leanFinger"> A variable containing the current touch information form LeanFinger class</param>
    public void ObjectSelected(LeanSelectByFinger selectedObject, LeanFinger leanFinger)
    {
        
        if (leanFinger.SnapshotDuration < minThreshold)
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
    
    /// <summary>
    /// This function will reset the following game object's components settings:
    /// - Disable LeanDragTranslate from the LeanTouch plugin
    /// - Disable the bounding box for the object from the BoundBox plugin
    ///
    /// It will also disable delete button for all deselected objects.
    /// </summary>
    /// <param name="deselectedObject"> the object that has been deselected </param>
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

    /// <summary>
    /// This function will check for the duration of touch.
    /// If it's a long touch, activate the toy and trigger the OnItemInteracted event.
    /// Otherwise, set the following selected game object's components settings:
    /// - Enable LeanDragTranslate from the LeanTouch plugin
    /// - Enable the bounding box for the object from the BoundBox plugin
    ///
    /// It will also enable delete button for the object.
    /// </summary>
    /// <param name="selectedObject"></param>
    /// <param name="leanFinger"></param>
    public void FunctionalObjectSelected(LeanSelectByFinger selectedObject, LeanFinger leanFinger)
    {
        if (leanFinger.SnapshotDuration < minThreshold)
            return;
        _selectedGameObject = gameObject;
        if (leanFinger.SnapshotDuration > minLongPressThreshold)
        {
            if(gameObject.TryGetComponent<ToyBehaviour>(out ToyBehaviour toyTrigger))
            {
                toyTrigger.OnItemInteracted();
            }
        }
        else
        {
            if (_selectedGameObject.TryGetComponent<LeanDragTranslate>(out var dragTranslate)) 
                dragTranslate.enabled = true;

            if (_selectedGameObject.TryGetComponent<BoundBox>(out var boundingBox))
            {
                boundingBox.lineColor.a = 200;
                boundingBox.enabled = true;
            }
            BiomeEditingEvents.ObjectSelectedEvent(_selectedGameObject);
        }

    }
    
}