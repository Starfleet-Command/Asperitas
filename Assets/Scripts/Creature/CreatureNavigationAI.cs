using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;

[RequireComponent(typeof(SteeringBasics))]
[RequireComponent(typeof(FollowPath))]
[RequireComponent(typeof(WallAvoidance))]
[RequireComponent(typeof(Wander1))]
public class CreatureNavigationAI : MonoBehaviour
{
    
    //1 means full vertical, 0 means full horizontal.
    [Range(0,1)] [SerializeField] private float verticalMovementBias;
    [SerializeField] private float timeBetweenPathAdditions;

    [SerializeField] private float minZ;
    public LinePath path;

    public List<Vector3> dynamicPath;
    private SteeringBasics steeringBasics;
    private FollowPath followPath;
    private WallAvoidance wallAvoidance;
    private Wander1 wander;

    private Vector3 currentPathNode;
    private bool canAddNewNode = true;

    void Start()
    {
        path.CalcDistances();

        steeringBasics = GetComponent<SteeringBasics>();
        followPath = GetComponent<FollowPath>();
        wallAvoidance = GetComponent<WallAvoidance>();
        wander = GetComponent<Wander1>();
        dynamicPath = new List<Vector3>(path.nodes);
       
    }

    void FixedUpdate()
    {

        Vector3 accel = wallAvoidance.GetSteering();
        
        if (accel.magnitude < 0.005f)
        {
            accel = followPath.GetSteering(path);
            
        }

        if(canAddNewNode)
            StartCoroutine("AddToPath");


        steeringBasics.Steer(accel);
        steeringBasics.LookWhereYoureGoing();

         path.Draw();
    }


    private IEnumerator AddToPath()
    {
        canAddNewNode=false;
        yield return new WaitForSeconds(timeBetweenPathAdditions);
        dynamicPath.RemoveAt(0);
        Vector3 newPoint = wander.GenerateWanderPoint();

        while(newPoint.z < minZ)
        {
            newPoint = wander.GenerateWanderPoint();
        }
        
        dynamicPath.Add(newPoint);
        path.nodes = dynamicPath.ToArray();
        canAddNewNode=true;
    }


}
