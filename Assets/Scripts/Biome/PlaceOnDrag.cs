using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField]
        [Tooltip("Button that places the agent object")]
        private Button _doneButton;
        
        [SerializeField]
        [Tooltip("Undo button")]
        private Button _undoButton;
        
        [FormerlySerializedAs("_isFreeForm")]
        [SerializeField]
        [Tooltip("Free Form Toggle Button")]
        private Toggle _freeFormToggle;

        [FormerlySerializedAs("solidWhiteMaterial")]
        [SerializeField] 
        [Tooltip("Solid White Material")]
        private Material foundationMaterial;
        
        [SerializeField] 
        [Tooltip("Solid White Material")]
        private Material stackableMaterial;
        
        [SerializeField] 
        [Tooltip("Solid White Material")]
        private Material noneStackableMaterial;
        
    #pragma warning restore 0649

    private IGameboard _gameboard;
    private GameObject _selectedGameObject;
    private List<GameObject> _placedObjects = new List<GameObject>();
    private GameboardAgent _agent;
    private bool _isReplacing;
    private bool _arIsRunning;
    private bool _gameboardIsRunning;
    private Collider selectedObjCollider;

    //[SerializeField] private int decimalToSnapTo =1;
    
    private PlacedObjectAttributes _ownAttributes;
    private Transform _currentBaseTranform;
        
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
                  if (collidingAttributes != null && _ownAttributes != null)
                  {
                      if (_ownAttributes.CanPlace(collidingAttributes.stackabilityType))
                      {
                          CreatePlacementGuide(cameraTransform, normalRaycastHit.point);
                      }
                  }

                  else if (_gameboard.RayCast(ray, out Vector3 hitPoint))
                  {
                      // Check whether the object can be fit in the resulting position
                      if (_gameboard.CheckFit(center: hitPoint, 0.4f) &&
                          _ownAttributes.CanPlace(StackabilityType.Gameboard))
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
            Ray ray = new Ray(_selectedGameObject.transform.position, -1.0f * _selectedGameObject.transform.up);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit downwardHit;
            // _selectedGameObject.GetComponent<Collider>().enabled = false;
            if (Physics.Raycast(ray, out downwardHit))
            {
                PlacedObjectAttributes objectUnderneathAttributes=null;
                downwardHit.collider.gameObject.TryGetComponent<PlacedObjectAttributes>(out objectUnderneathAttributes);
                if (objectUnderneathAttributes == null && _ownAttributes.stackabilityType == StackabilityType.Foundation)
                {
                    _doneButton.interactable = true;
                } else if (objectUnderneathAttributes == null)
                {
                    _doneButton.interactable = false;
                } else if (downwardHit.collider.gameObject != _selectedGameObject)
                {
                    if (objectUnderneathAttributes.stackabilityType == StackabilityType.Foundation)
                    {
                        _currentBaseTranform = downwardHit.collider.gameObject.transform;
                    } else if (objectUnderneathAttributes.stackabilityType == StackabilityType.Stackable)
                    {
                        _currentBaseTranform = downwardHit.collider.gameObject.transform.parent;
                    }

                    Debug.Log("Object below is: " + objectUnderneathAttributes.stackabilityType);
                    Debug.Log(_ownAttributes.CanPlace(objectUnderneathAttributes.stackabilityType));
                    if (objectUnderneathAttributes != null && _ownAttributes != null)
                    {
                        _doneButton.interactable = _ownAttributes.CanPlace(objectUnderneathAttributes.stackabilityType);
                    }
                }
            }
            else
            {
                _doneButton.interactable = false;
            }

            // if (!colide with anythig) {
            //     place based on higherarchy
            // }
        }

        private void CreatePlacementGuide(Transform cameraTransform,Vector3 hitPoint)
        {
            _selectedGameObject.SetActive(true);
            // add offset so object spawns at correct height
            hitPoint.y = hitPoint.y +selectedObjCollider.bounds.extents.y;
            // If in Snap mode, snap placement to closest decimal position
            // hitPoint = new Vector3(RoundToDecimal(hitPoint.x,decimalToSnapTo),RoundToDecimal(hitPoint.y,decimalToSnapTo+1), RoundToDecimal(hitPoint.z,decimalToSnapTo));
            
            //All 
            _selectedGameObject.transform.position = hitPoint+_ownAttributes.offsetAfterPlacement;

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
            _ownAttributes = null;
            _selectedGameObject.TryGetComponent<PlacedObjectAttributes>(out _ownAttributes);
            _agent = _selectedGameObject.GetComponent<GameboardAgent>();
            _agent.State = GameboardAgent.AgentNavigationState.Paused;
            if (_freeFormToggle.isOn)
            {
                PlaceObjectWithFixedDistance();
            }
            _isReplacing = true;
        }

        //Todo: hook this up to OnItemGeneratedEvent
        public void OnInventoryItemSpawned(GameObject spawnedItem)
        {
            if (_selectedGameObject != null)
            {
                Destroy(_selectedGameObject);
                _selectedGameObject = null;
            }
            _selectedGameObject = spawnedItem;
            selectedObjCollider = _selectedGameObject.GetComponent<Collider>();
            _selectedGameObject.TryGetComponent<PlacedObjectAttributes>(out _ownAttributes);
            _agent = _selectedGameObject.GetComponent<GameboardAgent>();
            _agent.State = GameboardAgent.AgentNavigationState.Paused;
            _isReplacing = !_isReplacing;
            if (_freeFormToggle.isOn)
            {
                PlaceObjectWithFixedDistance();
            }
            HandlePlacement();
        }
        
        /**
         * <summmary> Places _selectedGameObject with a fixed distance from the camera.</summmary>
         */
        private void PlaceObjectWithFixedDistance()
        {
            float distanceMultiplyer = 1f;
            switch (_ownAttributes.stackabilityType)
            {
                case StackabilityType.Foundation:
                    distanceMultiplyer = 1.5f;
                    break;
                case StackabilityType.Stackable:
                    distanceMultiplyer = 1.25f;
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
            var objectRenderer = _selectedGameObject.GetComponent<Renderer>();
            
            switch (_ownAttributes.stackabilityType)
            {
                case StackabilityType.Foundation:
                    objectRenderer.material = foundationMaterial;
                    break;
                case StackabilityType.Stackable:
                    objectRenderer.material = stackableMaterial;
                    _selectedGameObject.transform.SetParent(_currentBaseTranform);
                    break;
                case StackabilityType.Nonstackable:
                    objectRenderer.material = noneStackableMaterial;
                    _selectedGameObject.transform.SetParent(_currentBaseTranform);
                    break;
            }
            
            _placedObjects.Add(_selectedGameObject);
            if (_placedObjects.Count > 0)
            {
                _undoButton.interactable = true;
            }

            if(_ownAttributes.biomeEffect!=null)
                BiomeEditingEvents.BiomeHabitabilityModifiedEvent(_ownAttributes.biomeEffect);
            _selectedGameObject = null;
        }

        public void UndoButtonOnClick()
        {
            if (_placedObjects.Count < 1)
                return;
            
            Destroy(_placedObjects.Last());
            _placedObjects.RemoveAt(_placedObjects.Count - 1);

            if (_placedObjects.Count < 1)
            {
                _undoButton.interactable = false;
            }
        }
        
        public float RoundToDecimal(float number, int decPoints)
        {          
            float pow = Mathf.Pow(10,decPoints);
            return Mathf.Ceil(number * pow) / pow;

        }

}
