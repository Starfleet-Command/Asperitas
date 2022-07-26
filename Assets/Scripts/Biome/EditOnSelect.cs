using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [FormerlySerializedAs("_selectedObjectMaterial")]
    [SerializeField] 
    [Tooltip("Selected material")]
    private Material selectedObjectMaterial;

    private Material _previousObjectMaterial;

    private GameObject _selectedGameObject;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // LeanSelectableByFinger leanSelectableByFinger;
        // // leanSelectableByFinger.OnSelected
        // LeanSelectByFinger leanSelectByFinger;
        // leanSelectByFinger.ons
    }
    
    public void ObjectSelected(LeanSelect selectedObject)
    {
        _selectedGameObject = gameObject;
        if (_selectedGameObject.CompareTag("Creature"))
        {
            // var childObject = _selectedGameObject.transform.GetChild(0).gameObject;
            // if (childObject)
            return;
        }

        if (_selectedGameObject.TryGetComponent<Renderer>(out var objectRenderer))
        {
            _previousObjectMaterial = objectRenderer.material;
            objectRenderer.material = selectedObjectMaterial;
        }
        BiomeEditingEvents.ObjectSelectedEvent(_selectedGameObject);
    }
    
    public void ObjectDeselected(LeanSelect deselectedObject)
    {
        if (_selectedGameObject == null)
            return;
        if (_selectedGameObject.CompareTag("Creature"))
        {
            // var childObject = _selectedGameObject.transform.GetChild(0).gameObject;
            // if (childObject)
            return;
        }
        if (_selectedGameObject.TryGetComponent<Renderer>(out var objectRenderer))
        {
            objectRenderer.material = _previousObjectMaterial;
        }
        BiomeEditingEvents.ObjectDeselectedEvent(_selectedGameObject);
        _selectedGameObject = null;
    }
    
    public void FunctionalObjectSelected(LeanSelectByFinger selectedObject, LeanFinger leanFinger)
    {
        if (leanFinger.SnapshotDuration > 0.5f)
        {
            if(gameObject.TryGetComponent<ToyBehaviour>(out ToyBehaviour toyTrigger))
            {
                toyTrigger.OnItemInteracted();
            }
            return;
        }

        _selectedGameObject = gameObject;
        if (_selectedGameObject.TryGetComponent<Renderer>(out var objectRenderer))
        {
            _previousObjectMaterial = objectRenderer.material;
            objectRenderer.material = selectedObjectMaterial;
        }
        BiomeEditingEvents.ObjectSelectedEvent(_selectedGameObject);
    }
}