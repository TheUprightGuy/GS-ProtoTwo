using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{

    public enum BehaviourType
    {
        NONE,

        TO_FROM,
        TO_FROM_NAVMESH,

        WANDER
    }
    // Start is called before the first frame update

    public BehaviourType Behaviour;

    public float MoveSpeed = 1.0f;

    public float pauseTime = 2.0f;


    public bool lockY = true;

    public Vector3 storedHandlePos;
    
    public GameInfo gameInfo;

    //Pathing
    /*******************************/

    public List<Vector3> Targets = new List<Vector3>();

    int MoveToIndex = 0;

    bool bMoving = true;

    /*******************************/


    //Wander
    /*******************************/
    private NavMeshAgent NMA;

    public Vector3 nextPoint = Vector3.zero;
    public float WanderRadius = 10.0f;

    /*******************************/



    //Enemy Awareness
    /*******************************/
    public float DetectRange = 100.0f;
    public float EnemyFov = 0.0f;

    /*******************************/
    private void OnDestroy()
    {
        EventHandler.Instance.onPauseToggled -= OnPauseToggled;
    }

    void Start()
    {
        EventHandler.Instance.onPauseToggled += OnPauseToggled;
        gameInfo = GameObject.Find("EventHandler").GetComponent<EventHandler>().gameInfo;
        //Targets = new List<Vector3>();
        //Targets.Add(To + transform.position);
        //Targets.Add(From + transform.position);

        NMA = GetComponent<NavMeshAgent>();
        NMA.autoBraking = false;
        storedHandlePos = transform.position;

        for (int i = 0; i < Targets.Count; i++)
        {
            Targets[i] += transform.position;
        }

        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    float timer = 0.0f;
    private Vector3 prevForward = Vector3.zero;
    private Vector3 nextForward = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (gameInfo.paused || gameInfo.worldPaused) return;
        switch (Behaviour)
        {
            case BehaviourType.NONE:
                break;
            case BehaviourType.TO_FROM:
                NMA.enabled = false;
                AIPath();
                break;
            case BehaviourType.TO_FROM_NAVMESH:
                break;
            case BehaviourType.WANDER:
                NMA.enabled = true;
                AIWander();
                break;
            default:
                break;
        }
    }

    void AIPath()
    {
        if (bMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, Targets[MoveToIndex], MoveSpeed);
            if (Vector3.Distance(transform.position, Targets[MoveToIndex]) < 0.001f)
            {
                bMoving = false;
                prevForward = transform.forward;

                Vector3 temp = Targets[MoveToIndex]; //moved to

                
                MoveToIndex = (MoveToIndex + 1) % (Targets.Count);

                nextForward = (Targets[MoveToIndex] - temp);

                timer = 0;
            }

        }
        else
        {
            if (timer >= pauseTime)
            {
                bMoving = true;

            }
            else
            {
                timer += Time.deltaTime;
                transform.forward = Vector3.RotateTowards(transform.forward, nextForward, pauseTime * Time.deltaTime, 0.0f);
            }
        }
    }

    void AIWander()
    {
        if (!NMA.pathPending && NMA.remainingDistance < 0.5f)
        {
            NMA.destination = AIWander_NewDest();
        }
    }

    Vector3 AIWander_NewDest()
    {
        Vector3 newPos = transform.position;
        NavMeshPath temp = new NavMeshPath();
        do
        {
            newPos = Random.insideUnitCircle * WanderRadius;
            newPos = new Vector3(storedHandlePos.x + newPos.x, storedHandlePos.y, storedHandlePos.z + newPos.y);
            
        } while (!NMA.CalculatePath(newPos, temp));
        
        return newPos;
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    public Transform PlayerObj = null;
    void FindVisibleTargets()
    {
        PlayerObj = null;
        
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, DetectRange, LayerMask.GetMask("Player"));

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) <  EnemyFov / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (Physics.Raycast(transform.position, dirToTarget, dstToTarget, LayerMask.GetMask("Player")))
                {
                    PlayerObj = target;
                }
                else
                {
                    PlayerObj = null;
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void OnDrawGizmos()
    {

        switch (Behaviour)
        {
            case BehaviourType.NONE:
                break;
            case BehaviourType.TO_FROM:


                break;
            case BehaviourType.TO_FROM_NAVMESH:
                break;
            case BehaviourType.WANDER:
                if (Application.isPlaying)
                {
                    Gizmos.DrawSphere(NMA.destination, 0.2f);
                }
                break;
            default:
                break;
        }
    }

    private void OnPauseToggled(bool obj)
    {
        NMA.isStopped = (gameInfo.paused || gameInfo.worldPaused);
    }
}
