using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableInventoryHandler : MonoBehaviour
{
    public ThrowableObject[] throwableInventory;
    [SerializeField] private GameObject throwableUiCanvas;
    [SerializeField] private GameObject mainInventoryCanvas;
    [SerializeField] private GameObject sceneCamera;
    private bool isInThrowingMode= false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartThrowMode(int _indexOfObject)
    {
        isInThrowingMode=true;
        mainInventoryCanvas.SetActive(false);
        throwableUiCanvas.SetActive(false);
        
        Vector3 spawnPos = new Vector3(sceneCamera.transform.position.x, sceneCamera.transform.position.y,sceneCamera.transform.position.z);
        spawnPos += new Vector3(0,0,1);
        GameObject throwableInstance = Instantiate(throwableInventory[_indexOfObject].objectModel,spawnPos,Quaternion.identity);
        throwableInstance.transform.SetParent(sceneCamera.transform);
    }

    public void StopThrowMode()
    {
        isInThrowingMode=false;
        mainInventoryCanvas.SetActive(true);
    }

    
}


public class ThrowableObject
{
    public GameObject objectModel;
    public Vector3 impulseMagnitude;
    public float gravityMagnitude;
}
