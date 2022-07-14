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
        var objectRenderer = _selectedGameObject.GetComponent<Renderer>();
        _previousObjectMaterial = objectRenderer.material;
        objectRenderer.material = selectedObjectMaterial;
        BiomeEditingEvents.ObjectSelectedEvent(_selectedGameObject);
    }
    
    public void ObjectDeselected(LeanSelect deselectedObject)
    {
        var objectRenderer = _selectedGameObject.GetComponent<Renderer>();
        objectRenderer.material = _previousObjectMaterial;
        BiomeEditingEvents.ObjectDeselectedEvent(_selectedGameObject);
        _selectedGameObject = null;
    }
}