using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Vector3 To = Vector3.one;
    public Vector3 From = -Vector3.one;

    public bool lockY = true;

    public float MoveSpeed = 1.0f;

    public List<Vector3> Targets = new List<Vector3>();
    int MoveToIndex = 0;
    bool bMoving = true;


    public Vector3 storedHandlePos;
    void Start()
    {
        //Targets = new List<Vector3>();
        //Targets.Add(To + transform.position);
        //Targets.Add(From + transform.position);

        storedHandlePos = transform.position;

        for (int i = 0; i < Targets.Count; i++)
        {
            Targets[i] += transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, Targets[MoveToIndex], MoveSpeed);
            if (Vector3.Distance(transform.position, Targets[MoveToIndex]) < 0.001f)
            {
                
                MoveToIndex++;
                MoveToIndex = (MoveToIndex == Targets.Count) ? (0) : (MoveToIndex);
            }

        }
    }

}
