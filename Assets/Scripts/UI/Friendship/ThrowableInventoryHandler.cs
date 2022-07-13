using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableInventoryHandler : MonoBehaviour
{
    public ThrowableObject[] throwableInventory;
    [SerializeField] private GameObject throwableUiCanvas;
    [SerializeField] private GameObject sceneCamera;
    private bool isInThrowingMode= false;

    private Vector3 swipeStartPos;
    private Vector3 swipeEndPos;
    private ThrowableObject selectedObject;
    private GameObject throwableInstance;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartThrowMode(int _indexOfObject)
    {
        isInThrowingMode=true;
        UiEvents.IsThrowingStatusChangedEvent(isInThrowingMode);
        throwableUiCanvas.SetActive(false);
        
        Vector3 spawnPos = sceneCamera.transform.forward;
        throwableInstance = Instantiate(throwableInventory[_indexOfObject].objectModel);
        selectedObject = throwableInventory[_indexOfObject];
        throwableInstance.transform.SetParent(sceneCamera.transform);
        throwableInstance.transform.localPosition = new Vector3(0,-0.25f,1);
    }

    public void StopThrowMode()
    {
        isInThrowingMode=false;
        
        UiEvents.IsThrowingStatusChangedEvent(isInThrowingMode);
    }


    public void CatchSwipeStart(Vector3 startPos)
    {
        if(isInThrowingMode)
            swipeStartPos=startPos;
    }

    public void HandleSwipe(Vector3 endPos)
    {
        if(isInThrowingMode)
        {
            swipeEndPos=endPos;

            if(swipeStartPos.y < swipeEndPos.y)
            {

                if(throwableInstance.GetComponent<Rigidbody>())
                {
                    Vector3 swipeDelta = swipeEndPos-swipeStartPos;

                    throwableInstance.transform.SetParent(null);

                    Vector3 forwardImpulse = sceneCamera.transform.forward;
                    Vector3 upwardImpulse = sceneCamera.transform.up;
                    

                    forwardImpulse= forwardImpulse*selectedObject.forwardMagnitude*swipeDelta.magnitude;
                    upwardImpulse = upwardImpulse * selectedObject.upwardMagnitude*swipeDelta.magnitude;

                    throwableInstance.GetComponent<Rigidbody>().AddForce(forwardImpulse,ForceMode.Impulse);
                    throwableInstance.GetComponent<Rigidbody>().AddForce(upwardImpulse,ForceMode.Impulse);
                    throwableInstance.GetComponent<Rigidbody>().useGravity= true;
                    
                }

                if(throwableInstance.TryGetComponent<DespawnAfterSeconds>(out DespawnAfterSeconds despawnScript))
                {
                    despawnScript.StartCountdown();
                }

                StopThrowMode();
                    
            }
        }
            
    }

    
}

[System.Serializable]
public class ThrowableObject
{
    public GameObject objectModel;
    public float forwardMagnitude;
    public float upwardMagnitude;

}
