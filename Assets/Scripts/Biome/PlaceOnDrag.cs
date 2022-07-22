using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean.Common;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Niantic.ARDK.Extensions.Gameboard;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.Input.Legacy;
using UnityEngine.Serialization;

public class PlaceOnDrag : MonoBehaviour
{
    #pragma warning disable 0649
        [SerializeField]
        [Tooltip("The scenes ARCamera")]
        private Camera _arCamera;

        [SerializeField]
        [Tooltip("Button that spawns the agent object")]
        private Button[] _spawnButton;

        [SerializeField] [Tooltip("Button that places the agent object")]
        private Button _doneButton;

        [FormerlySerializedAs("_isFreeForm")]
        [SerializeField]
        [Tooltip("Free Form Toggle Button")]
        private Toggle _freeFormToggle;

        [FormerlySerializedAs("solidWhiteMaterial")]
        [SerializeField] 
        [Tooltip("placeable material")]
        private Material placeableMaterial;
        
        [SerializeField] 
        [Tooltip("Not placeable material")]
        private Material notPlaceableMaterial;

        [SerializeField]
        [Tooltip("Button that cancels placement of the object")]
        private Button _cancelButton;
        
    #pragma warning restore 0649

    private IGameboard _gameboard;
    public GameObject _selectedGameObject;
    private List<GameObject> _placedObjects = new List<GameObject>();
    private GameboardAgent _agent;
    private bool _isReplacing;
    private bool _arIsRunning;
    private bool _gameboardIsRunning;
    private Collider selectedObjCollider;
    private Material selectedObjMaterial;

    //[SerializeField] private int decimalToSnapTo =1;

    private Vector3 mouseReference;
    private Vector3 mouseOffset;
    private Vector3 rotation;
    private PlacedObjectAttributes ownAttributes;

    

            /// Inform about started ARSession.
        public void ARSessionStarted()
        {
            _arIsRunning = true;
        }

        /// Inform about stopped ARSession, update UI and clear Gameboard.
        public void ARSessionStopped()
        {
            Destroy(_selectedGameObject);
            _selectedGameObject = null;

            foreach (Button item in _spawnButton)
            {
             item.interactable = false;   
            }

            _isReplacing = false;
            _arIsRunning = false;

            _gameboard.Clear();
        }

        private void Awake()
        {
            GameboardFactory.GameboardInitialized += OnGameboardCreated;

            foreach (Button item in _spawnButton)
            {
             item.interactable = false;   
            }
        }

        private void OnEnable()
        {
            BiomeEditingEvents.OnItemGenerated+=OnInventoryItemSpawned;
        }

        private void OnDisable()
        {
            BiomeEditingEvents.OnItemGenerated-=OnInventoryItemSpawned;
        }

        private void OnGameboardCreated(GameboardCreatedArgs args)
        {
            _gameboard = args.Gameboard;
            _gameboardIsRunning = true;
            _gameboard.GameboardDestroyed += OnGameboardDestroyed;
        }

        private void OnGameboardDestroyed(IArdkEventArgs args)
        {
            _gameboard = null;
            _gameboardIsRunning = false;
        }

        private void Update()
        {
            if (!_gameboardIsRunning)
                return;

            if (_isReplacing && _selectedGameObject != null)
            {
                HandlePlacement();
            }
            else
            {
                foreach (Button item in _spawnButton)
                {
                    item.interactable = _gameboard.Area > 0;
                }
            }
        }

        private void HandlePlacement()
        {
            // Use this technique to place an object to a user-defined position.
          // Otherwise, use FindRandomPosition() to try to place the object automatically.

          // Get a ray pointing in the user's look direction
          if (_freeFormToggle.isOn)
          {
              FreeFormPlacementHandler();
              return;
          }

          var cameraTransform = _arCamera.transform;
          var ray = new Ray(cameraTransform.position, cameraTransform.forward);
          
          if(Physics.Raycast(ray, out RaycastHit normalRaycastHit))
          {
              PlacedObjectAttributes collidingAttributes=null;
              normalRaycastHit.collider.gameObject.TryGetComponent<PlacedObjectAttributes>(out collidingAttributes);
              if (normalRaycastHit.collider.gameObject != _selectedGameObject)
              {
                  if (collidingAttributes != null && ownAttributes != null)
                  {
                      if (ownAttributes.CanPlace(collidingAttributes.stackabilityType))
                      {
                          CreatePlacementGuide(cameraTransform, normalRaycastHit.point);
                      }
                  }

                  else if (_gameboard.RayCast(ray, out Vector3 hitPoint))
                  {
                      // Check whether the object can be fit in the resulting position
                      if (_gameboard.CheckFit(center: hitPoint, 0.4f) &&
                          ownAttributes.CanPlace(StackabilityType.Gameboard))
                      {
                          CreatePlacementGuide(cameraTransform, hitPoint);
                      }
                  }
              }
          }
        }

        private void FreeFormPlacementHandler()
        {
            PlaceObjectWithFixedDistance();
            Ray ray = new Ray(_selectedGameObject.transform.position, Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit downwardHit;
            // _selectedGameObject.GetComponent<Collider>().enabled = false;
            if (Physics.Raycast(ray, out downwardHit))
            {
                PlacedObjectAttributes objectUnderneathAttributes=null;
                downwardHit.collider.gameObject.TryGetComponent<PlacedObjectAttributes>(out objectUnderneathAttributes);
                if (objectUnderneathAttributes == null && ownAttributes.stackabilityType == StackabilityType.Foundation)
                {
                    _doneButton.interactable = true;
                    
                } else if (objectUnderneathAttributes == null)
                {
                    _doneButton.interactable = false;
                } else if (downwardHit.collider.gameObject != _selectedGameObject)
                {
                    Debug.Log("Object below is: " + objectUnderneathAttributes.stackabilityType);
                    Debug.Log(ownAttributes.CanPlace(objectUnderneathAttributes.stackabilityType));
                    if (objectUnderneathAttributes != null && ownAttributes != null)
                    {
                        _doneButton.interactable = ownAttributes.CanPlace(objectUnderneathAttributes.stackabilityType);
                        
                    }
                }
            }
            else
            {
                _doneButton.interactable = false;
            }
            if (_selectedGameObject.TryGetComponent<Renderer>(out var objectRenderer))
            {
                objectRenderer.material = _doneButton.interactable ? placeableMaterial : notPlaceableMaterial;
            }
        }

        private void CreatePlacementGuide(Transform cameraTransform,Vector3 hitPoint)
        {
            _selectedGameObject.SetActive(true);
            // add offset so object spawns at correct height
            hitPoint.y = hitPoint.y +selectedObjCollider.bounds.extents.y;
            // If in Snap mode, snap placement to closest decimal position
            // hitPoint = new Vector3(RoundToDecimal(hitPoint.x,decimalToSnapTo),RoundToDecimal(hitPoint.y,decimalToSnapTo+1), RoundToDecimal(hitPoint.z,decimalToSnapTo));
            
            //All 
            _selectedGameObject.transform.position = hitPoint+ownAttributes.offsetAfterPlacement;

            var rotation = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            _selectedGameObject.transform.rotation = Quaternion.LookRotation(-rotation);
        }

        public void SpawnButtonOnClick(GameObject spawnObject)
        {
            if (_selectedGameObject != null)
            {
                Destroy(_selectedGameObject);
                _selectedGameObject = null;
            }

            _selectedGameObject = Instantiate(spawnObject);
            selectedObjCollider = _selectedGameObject.GetComponent<Collider>();
            ownAttributes = null;
            _selectedGameObject.TryGetComponent<PlacedObjectAttributes>(out ownAttributes);
            if (_freeFormToggle.isOn)
            {
                PlaceObjectWithFixedDistance();
            }
            _isReplacing = true;
        }
        
        public void OnInventoryItemSpawned(GameObject spawnedItem)
        {
            if (_selectedGameObject != null)
            {
                Destroy(_selectedGameObject);
                _selectedGameObject = null;
            }
            _selectedGameObject = spawnedItem;
            if (_selectedGameObject.TryGetComponent<Renderer>(out var objectRenderer))
            {
                selectedObjMaterial = objectRenderer.material;
                objectRenderer.material = notPlaceableMaterial;
            }
            selectedObjCollider = _selectedGameObject.GetComponent<Collider>();
            _selectedGameObject.TryGetComponent<PlacedObjectAttributes>(out ownAttributes);
            _isReplacing = !_isReplacing;
            if (_freeFormToggle.isOn)
            {
                PlaceObjectWithFixedDistance();
            }
            _doneButton.gameObject.SetActive(true);
            _cancelButton.gameObject.SetActive(true);
            HandlePlacement();
            _isReplacing = true;
        }
        
        /**
         * <summmery> Places _selectedGameObject with a fixed distance from the camera.</summmery>
         */
        private void PlaceObjectWithFixedDistance()
        {
            float distanceMultiplyer = 1f;
            switch (ownAttributes.stackabilityType)
            {
                case StackabilityType.Foundation:
                    distanceMultiplyer = 2f;
                    break;
                case StackabilityType.Stackable:
                    distanceMultiplyer = 1.5f;
                    break;
                case StackabilityType.Nonstackable:
                    distanceMultiplyer = 1f;
                    break;
                default:
                    distanceMultiplyer = 5f;
                    break;
            }
            Vector3 followPosition = _arCamera.transform.position + _arCamera.transform.forward * distanceMultiplyer;
            _selectedGameObject.transform.position = followPosition;
        }

        public void DoneButtonOnClick()
        {
            _isReplacing = !_isReplacing;
            BiomeEditingEvents.ItemPlacedEvent(_selectedGameObject);
            if (_selectedGameObject.TryGetComponent<Renderer>(out var objectRenderer))
            {
                objectRenderer.material = selectedObjMaterial;
            }
            
            _placedObjects.Add(_selectedGameObject);

            var objSelectableComponent = _selectedGameObject.GetComponent<LeanSelectable>();
            if (objSelectableComponent != null)
            {
                objSelectableComponent.enabled = true;

            }

            if(ownAttributes.biomeEffect!=null)
                BiomeEditingEvents.BiomeHabitabilityModifiedEvent(ownAttributes.biomeEffect);
            _selectedGameObject = null;
        }

        public void CancelButtonOnClick()
        {
            _isReplacing = false;
            InventoryEvents.ItemCheckedInEvent(ownAttributes.sourceItem);
            Destroy(_selectedGameObject);
            _selectedGameObject = null;
        }
        
        public float RoundToDecimal(float number, int decPoints)
        {          
            float pow = Mathf.Pow(10,decPoints);
            return Mathf.Ceil(number * pow) / pow;

        }

}
