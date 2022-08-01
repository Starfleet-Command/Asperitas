using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;


/// <summary>
/// This class controls the behaviour of the creature movement, using scripts from the external plugin<br/>
/// UnityMovementAI.
/// </summary>
[RequireComponent(typeof(SteeringBasics))]
[RequireComponent(typeof(FollowPath))]
[RequireComponent(typeof(Wander1))]
[RequireComponent(typeof(Collider))]
public class CreatureNavigationAI : MonoBehaviour
{
    [SerializeField] private bool canChangeAltitude;
    [SerializeField] private float timeBetweenPathAdditions;
    [SerializeField] private float initialPointQuantity;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float minDistanceFromPlayer;
    [SerializeField] private float maxDistanceFromPlayer;
    [SerializeField] private float minAltitude;
    [SerializeField] private float maxAltitude;
    public LinePath path;

    public List<Vector3> dynamicPath;
    private SteeringBasics steeringBasics;
    private FollowPath followPath;
    private Wander1 wander;

    private Vector3 currentPathNode;
    private bool canAddNewNode = true;
    private Vector2 screenBounds;

    private bool isBeingSummoned=false;
    public bool mustFacePlayer=false;
    private Vector3 summonCoords;

    private void OnEnable()
    {
        CreatureEvents.OnCreatureSummoned+=HandleSummoning;
        CreatureEvents.OnCreatureReleased+=ReleaseFromSummon;
        UiEvents.OnIsThrowingStatusChanged+=ToggleLookAt;
    }

    private void OnDisable()
    {
        CreatureEvents.OnCreatureSummoned-=HandleSummoning;
        CreatureEvents.OnCreatureReleased-=ReleaseFromSummon;
        UiEvents.OnIsThrowingStatusChanged-=ToggleLookAt;
    }

    void Start()
    {
        if(mainCamera==null)
        {
            mainCamera = Camera.main;
        }

        steeringBasics = GetComponent<SteeringBasics>();
        followPath = GetComponent<FollowPath>();
        wander = GetComponent<Wander1>();
        dynamicPath = new List<Vector3>();

        for (int i = 0; i < initialPointQuantity; i++)
        {
            dynamicPath.Add(GenerateNode());
        }

        path.nodes = dynamicPath.ToArray();
        
        path.CalcDistances();
       
    }

    void FixedUpdate()
    {

        Vector3 accel = followPath.GetSteering(path);


        if(canAddNewNode)
            StartCoroutine("AddToPath");


        steeringBasics.Steer(accel);

        if(!mustFacePlayer)
            steeringBasics.LookWhereYoureGoing();

        else
        {
            steeringBasics.LookAtDirection(mainCamera.transform.position-this.gameObject.transform.position);
        }
            

         path.Draw();
    }

    private Vector3 GenerateNode()
    {
        Vector3 newPoint;

        if(isBeingSummoned)
        {
            newPoint = summonCoords;
        }
        else
        {
            newPoint = wander.GenerateWanderPoint();

            /*while(newPoint.z < minZ)
            {
                newPoint = wander.GenerateWanderPoint();
            } */
            newPoint = PreventOutOfBounds(newPoint);
        }

        return newPoint;
    }

    private void HandleSummoning(Vector3 _location)
    {
        summonCoords= _location;
        isBeingSummoned=true;
    }

    private void ReleaseFromSummon()
    {
        isBeingSummoned=false;
    }

    private void ToggleLookAt(bool _toggleState)
    {
        
        if (!_toggleState)
        {
            StartCoroutine("WaitToReset");
        }
        
        else
            mustFacePlayer = _toggleState;
    }

    IEnumerator WaitToReset()
    {
        yield return new WaitForSeconds(2);
        mustFacePlayer = false;
    }


    private IEnumerator AddToPath()
    {
        canAddNewNode=false;
        yield return new WaitForSeconds(timeBetweenPathAdditions);
        dynamicPath.RemoveAt(0);
        
        dynamicPath.Add(GenerateNode());
        path.nodes = dynamicPath.ToArray();
        canAddNewNode=true;
    }

    /// <summary>
    /// This method prevents the wander point generation from exceeding the user-set bounds<br/>
    /// If the point is out of bounds it will mirror the point with the bound as the center<br/>
    /// And if the mirrored point is still out of bounds it will clamp it to them.
    /// </summary>
    /// <param name="navPoint"> The coordinates from the wander point generation </param>
    /// <returns> The adjusted coordinate </returns>
    private Vector3 PreventOutOfBounds(Vector3 navPoint)
    {
        Vector3 viewPos = navPoint;

        if(viewPos.x<mainCamera.transform.position.x+ minDistanceFromPlayer )
        {
            viewPos.x = viewPos.x + 2*((mainCamera.transform.position.x+ minDistanceFromPlayer)-viewPos.x);
        }

        else if(viewPos.x >mainCamera.transform.position.x+ maxDistanceFromPlayer)
        {
            viewPos.x = viewPos.x + 2*(viewPos.x-(mainCamera.transform.position.x+ minDistanceFromPlayer));
        }
        
        viewPos.x=Mathf.Clamp(viewPos.x, mainCamera.transform.position.x+ minDistanceFromPlayer,mainCamera.transform.position.x+ maxDistanceFromPlayer);

        if(canChangeAltitude)
            viewPos.y=Mathf.Clamp(navPoint.y,mainCamera.transform.position.y+minAltitude,mainCamera.transform.position.y+maxAltitude);
        else
            viewPos.y= maxAltitude;

        
        if(viewPos.z<mainCamera.transform.position.z+ minDistanceFromPlayer )
        {
            viewPos.z = viewPos.z + 2*((mainCamera.transform.position.z+ minDistanceFromPlayer)-viewPos.z);
        }

        else if(viewPos.z >mainCamera.transform.position.z+ maxDistanceFromPlayer)
        {
            viewPos.z = viewPos.z + 2*(viewPos.z-(mainCamera.transform.position.z+ minDistanceFromPlayer));
        }

        viewPos.z=Mathf.Clamp(viewPos.z, mainCamera.transform.position.z+ minDistanceFromPlayer,mainCamera.transform.position.z+ maxDistanceFromPlayer);
        
        return viewPos;
    }



}
