using System.Collections;
using System.Collections.Generic;
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
        
        [FormerlySerializedAs("_isFreeForm")]
        [SerializeField]
        [Tooltip("Free Form Toggle Button")]
        private Toggle _freeFormToggle;
    
    #pragma warning restore 0649

    private IGameboard _gameboard;
    private GameObject _selectedGameObject;
    private GameboardAgent _agent;
    private bool _isReplacing;
    private bool _arIsRunning;
    private bool _gameboardIsRunning;

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
            Debug.Log("Is FreeForm: " + _freeFormToggle.isOn);
            if (!_gameboardIsRunning)
                return;

            if (_isReplacing)
            {
                HandlePlacement();
            }
            else
            {
                foreach (Button item in _spawnButton)
                {
                    item.interactable = _gameboard.Area > 0;
                }
                // Only allow placing the actor if at least one surface is discovered
                HandleTouch();
            }
        }

            private void HandleTouch()
        {
            //if there is a touch call our function
            if (PlatformAgnosticInput.touchCount <= 0)
                return;

            var touch = PlatformAgnosticInput.GetTouch(0);

            //if there is no touch or touch selects UI element
            if (PlatformAgnosticInput.touchCount <= 0 || EventSystem.current.currentSelectedGameObject != null)
                return;

            if (touch.phase == TouchPhase.Began)
            {
                TouchBegan(touch);
            }
        }

        private void TouchBegan(Touch touch)
        {
            if (!_arIsRunning || _agent == null || _arCamera == null)
                return;

            //as we are using meshing we can use a standard ray cast
            Ray ray = _arCamera.ScreenPointToRay(touch.position);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                /* if(hit.collider.gameObject.tag=="Prop")
                {
                // offset
                mouseOffset = (Input.mousePosition - mouseReference);
                
                // apply rotation
                rotation.y = -(mouseOffset.x + mouseOffset.y) * _sensitivity;
                
                // rotate
                hit.collider.gameObject.transform.Rotate(rotation);
                
                // store mouse
                mouseReference = Input.mousePosition;
                //_agent.SetDestination(hit.point);
                } */
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
            Vector3 followPosition = _arCamera.transform.position + _arCamera.transform.forward * 2;
            _selectedGameObject.transform.position = followPosition;
            
            // if (!colide with anythig) {
            //     place based on higherarchy
            // }
        }

        private void CreatePlacementGuide(Transform cameraTransform,Vector3 hitPoint)
        {
            _selectedGameObject.SetActive(true);
            // add offset so object spawns at correct height
            hitPoint.y = hitPoint.y +_selectedGameObject.transform.localScale.y/2;

            // If in Snap mode, snap placement to closest decimal position
            // hitPoint = new Vector3(RoundToDecimal(hitPoint.x,decimalToSnapTo),RoundToDecimal(hitPoint.y,decimalToSnapTo+1), RoundToDecimal(hitPoint.z,decimalToSnapTo));


            //All 
            _selectedGameObject.transform.position = hitPoint+ownAttributes.offsetAfterPlacement;

            var rotation = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            _selectedGameObject.transform.rotation = Quaternion.LookRotation(-rotation);

            
            
        }

        public void SpawnButtonOnClick(GameObject spawnObject)
        {
            
            _selectedGameObject = Instantiate(spawnObject);
            ownAttributes = null;
            _selectedGameObject.TryGetComponent<PlacedObjectAttributes>(out ownAttributes);
            _agent = _selectedGameObject.GetComponent<GameboardAgent>();
            _agent.State = GameboardAgent.AgentNavigationState.Paused;
            if (_freeFormToggle.isOn)
            {
                Vector3 followPosition = _arCamera.transform.position + _arCamera.transform.forward * 2;
                _selectedGameObject.transform.position = followPosition;
            }
            _isReplacing = !_isReplacing;
        }

        //Todo: hook this up to OnItemGeneratedEvent
        public void OnInventoryItemSpawned(GameObject spawnedItem)
        {
            _selectedGameObject = spawnedItem;
            _selectedGameObject.TryGetComponent<PlacedObjectAttributes>(out ownAttributes);
            _agent = _selectedGameObject.GetComponent<GameboardAgent>();
            _agent.State = GameboardAgent.AgentNavigationState.Paused;
            _isReplacing = !_isReplacing;
            if (_freeFormToggle.isOn)
            {
                Vector3 followPosition = _arCamera.transform.position + _arCamera.transform.forward * 2;
                _selectedGameObject.transform.position = followPosition;
            }
            HandlePlacement();
        }

        public void DoneButtonOnClick()
        {
            _isReplacing = !_isReplacing;
            BiomeEditingEvents.ItemPlacedEvent(_selectedGameObject);

            if(ownAttributes.biomeEffect!=null)
                BiomeEditingEvents.BiomeHabitabilityModifiedEvent(ownAttributes.biomeEffect);
        }


        public float RoundToDecimal(float number, int decPoints)
        {          
            float pow = Mathf.Pow(10,decPoints);
            return Mathf.Ceil(number * pow) / pow;

        }

}
