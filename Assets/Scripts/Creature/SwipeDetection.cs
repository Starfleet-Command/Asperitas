using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public Vector3 swipeStartPosition;

    public Vector3 swipeDelta;
    public int currentSwipes;
    [SerializeField] private float distancePerRaycast;
    [SerializeField] private int swipesRequired;
    [SerializeField] private Camera levelCamera;

    private bool canSwipe = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        UiEvents.OnIsThrowingStatusChanged+=ToggleSwiping;
    }

        private void OnDisable()
    {
        UiEvents.OnIsThrowingStatusChanged-=ToggleSwiping;
    }

    private void ToggleSwiping(bool _status)
    {
        canSwipe = !_status;
    }

    public void CatchSwipeStart(Vector3 startPos)
    {
        if(canSwipe)
            swipeStartPosition = startPos;
    }

    public void CatchSwipeDelta(Vector3 _delta)
    {
        if(canSwipe)
        {
            swipeDelta = _delta;
            RaycastOnSwipePath();
        }

    }

    public void RaycastOnSwipePath()
    {
        Vector3 trajectory = swipeStartPosition;
        var cameraTransform = levelCamera.transform;
        Ray _ray;

        float highestDirection = Mathf.Max(Mathf.Abs(swipeDelta.x),Mathf.Abs(swipeDelta.y));
        float noOfRaycasts = highestDirection/distancePerRaycast;
        

        for (int i = 0; i < noOfRaycasts; i++)
        {
            trajectory += new Vector3(swipeDelta.x/noOfRaycasts,swipeDelta.y/noOfRaycasts,0f);

          _ray = new Ray(new Vector3(trajectory.x, trajectory.y,cameraTransform.position.z), cameraTransform.forward);
          Debug.DrawRay(_ray.origin,_ray.direction,Color.red);
          if(Physics.Raycast(_ray, out RaycastHit normalRaycastHit,Mathf.Infinity))
          {
            if(normalRaycastHit.collider.gameObject.TryGetComponent<CreatureInteractionPoint>(out CreatureInteractionPoint interactScript))
            {
                if(interactScript.socketType==InteractionSocketType.Petting)
                {
                    HandleSwipeHit();
                    return;
                }
            }

            else if(swipeDelta.y>0)
            {
                if(normalRaycastHit.collider.gameObject.TryGetComponent<ToyBehaviour>(out ToyBehaviour _toyTrigger))
                {
                    _toyTrigger.OnItemInteracted();
                }
            }

          }
        }
        currentSwipes=0;
    }

    public void HandleSwipeHit()
    {
        currentSwipes++;
        if(currentSwipes>=swipesRequired)
        {
            CreatureEvents.InteractionTriggeredEvent(InteractionSocketType.Petting);
            currentSwipes = 0;
        }
    }
}
