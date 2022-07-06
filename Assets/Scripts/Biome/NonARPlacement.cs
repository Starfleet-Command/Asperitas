using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NonARPlacement : MonoBehaviour
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
    [Tooltip("Button that cancels the placement of the agent object")]
    private Button _cancelButton;
    
    #pragma warning restore 0649

    private bool _isReplacing=false;
    [SerializeField] private GameObject _selectedGameObject;
    private PlacedObjectAttributes ownAttributes;
    private GameboardAgent _agent;

    private void OnEnable()
    {
        BiomeEditingEvents.OnItemGenerated+=OnInventoryItemSpawned;
    }

    private void OnDisable()
    {
        BiomeEditingEvents.OnItemGenerated-=OnInventoryItemSpawned;
    }

    private void Update()
    {
        if (_isReplacing && _selectedGameObject!=null)
            {
                HandlePlacement();
            }
    }
    private void HandlePlacement()
        {
            // Use this technique to place an object to a user-defined position.
          // Otherwise, use FindRandomPosition() to try to place the object automatically.

          // Get a ray pointing in the user's look direction
          var cameraTransform = _arCamera.transform;
          var ray = new Ray(cameraTransform.position, cameraTransform.forward);
          
          if(Physics.Raycast(ray, out RaycastHit normalRaycastHit))
          {
              PlacedObjectAttributes collidingAttributes=null;
              normalRaycastHit.collider.gameObject.TryGetComponent<PlacedObjectAttributes>(out collidingAttributes);
              if(normalRaycastHit.collider.gameObject != _selectedGameObject)
              {
                if(collidingAttributes!=null && ownAttributes!=null)
                {
                    if( ownAttributes.CanPlace(collidingAttributes.stackabilityType))
                    {
                        CreatePlacementGuide(cameraTransform, normalRaycastHit.point);
                    }  
                }

              }
              

          }
        }

        private void CreatePlacementGuide(Transform cameraTransform,Vector3 hitPoint)
        {
            if(!_selectedGameObject.activeSelf)
                _selectedGameObject.SetActive(true);

            // add offset so object spawns at correct height
            hitPoint.y = hitPoint.y +_selectedGameObject.GetComponent<Collider>().bounds.extents.y;

            // If in Snap mode, snap placement to closest decimal position
            // hitPoint = new Vector3(RoundToDecimal(hitPoint.x,decimalToSnapTo),RoundToDecimal(hitPoint.y,decimalToSnapTo+1), RoundToDecimal(hitPoint.z,decimalToSnapTo));


            //All 
            _selectedGameObject.transform.position = hitPoint+ownAttributes.offsetAfterPlacement;


            
            
        }

        public void SpawnButtonOnClick(GameObject spawnObject)
        {
            
            _selectedGameObject = Instantiate(spawnObject);
            ownAttributes = null;
            _selectedGameObject.TryGetComponent<PlacedObjectAttributes>(out ownAttributes);
            _agent = _selectedGameObject.GetComponent<GameboardAgent>();
            _agent.State = GameboardAgent.AgentNavigationState.Paused;
            

            _isReplacing = true;
        }

        public void OnInventoryItemSpawned(GameObject spawnedItem)
        {
            _selectedGameObject = spawnedItem;
            _selectedGameObject.TryGetComponent<PlacedObjectAttributes>(out ownAttributes);
            _agent = _selectedGameObject.GetComponent<GameboardAgent>();
            _agent.State = GameboardAgent.AgentNavigationState.Paused;
            _isReplacing = true;
            _doneButton.gameObject.SetActive(true);
            _cancelButton.gameObject.SetActive(true);
            HandlePlacement();
        }

        public void DoneButtonOnClick()
        {
            _isReplacing = false;
            BiomeEditingEvents.ItemPlacedEvent(_selectedGameObject);

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
}
