using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayScript : MonoBehaviour
{
    public Transform targetPos;
    float rot;
    Vector3 rotVector;

    public void Update()
    {
        transform.LookAt(targetPos);

        rot = Mathf.PingPong(Time.time / 2, 2) - 1;
        rotVector = new Vector3(rot, 0, 0);

        transform.Translate(rotVector * Time.deltaTime);
    }
}
