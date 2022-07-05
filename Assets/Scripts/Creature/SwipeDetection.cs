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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CatchSwipeStart(Vector3 startPos)
    {
         
        swipeStartPosition = startPos;
    }

    public void CatchSwipeDelta(Vector3 _delta)
    {
        
        swipeDelta = _delta;
        TryHitCreature();
    }

    public void TryHitCreature()
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
             Debug.Log("Raycast sent at"+trajectory.ToString()+ " hit "+normalRaycastHit.collider.gameObject.name);
            if(normalRaycastHit.collider.gameObject.TryGetComponent<CreatureInteractionPoint>(out CreatureInteractionPoint interactScript))
            {
                if(interactScript.socketType==InteractionSocketType.Petting)
                {
                    HandleSwipeHit();
                    return;
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
