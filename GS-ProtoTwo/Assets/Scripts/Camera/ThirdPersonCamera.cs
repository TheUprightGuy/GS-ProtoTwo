﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject target;
    public GameObject PlayerObj;
    
    public bool invertX = false;
    public bool invertY = false;

    public bool ThirdPersonOnlyWhenLocked = true;
    [Tooltip("Press ` to enable/disable cursor locking.")]
    public bool UnlockCameraKeyBind = true;

    public float rotateSpeed = 5;
    public float zoomSpeed = 5;

    [Range(0.0f, 0.25f), Tooltip("0.0f == Off")]
    public float CamSmoothing = 0.5f;
    [Range(0.0f, 1.0f)]
    public float camCollisionRadius = 1.0f;

    [HideInInspector]
    public float minYAngle = 0.0f;
    [HideInInspector]
    public float maxYAngle = 45.0f;

    [HideInInspector]
    public float minZoomDist = 10.0f;
    [HideInInspector]
    public float maxZoomDist = 50.0f;


    Vector3 offset;

    float storeYPos = 0.0f;
    float storedXPos = 0.0f;
    // Start is called before the first frame update

    private Vector3 velocity = Vector3.zero;
    public GameInfo gameInfo;

    void Start()
    {
        offset = target.transform.position - transform.position;
        storedXPos = PlayerObj.transform.eulerAngles.y;
        storeYPos = PlayerObj.transform.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
	    if (gameInfo.paused) return;
         if (Input.GetKeyDown("`"))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (!ThirdPersonOnlyWhenLocked || Cursor.lockState == CursorLockMode.Locked)
        {
	        //Get Mouse Axis
	        /*********************/
	        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
	        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
	        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
	
	        //Invert if setting applied
	        if (invertX) { horizontal *= -1; }
	        if (invertY) { vertical *= -1; }
	        /*********************/
	
	        //Rotate Target
	
	        float desiredAngleY = target.transform.eulerAngles.y;
	
	        //Get the current stored Y angle and add mouse axis to it
	        storeYPos += vertical;
	        storeYPos = Mathf.Clamp(storeYPos, minYAngle, maxYAngle);

            storedXPos += horizontal;

            if (!Input.GetKey(KeyCode.Mouse1)) //right mouseclick
            {
	            PlayerObj.transform.Rotate(0, horizontal, 0);
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                storedXPos = target.transform.eulerAngles.y;
            }

            Quaternion rotation = Quaternion.Euler(storeYPos, storedXPos, 0);
	

	        offset.z -= zoom;
            offset.z = Mathf.Clamp(offset.z, minZoomDist, maxZoomDist);

	        //Get a new position based on target rotation, offset, and stored y rotation
	        Vector3 newPos = target.transform.position - (rotation * offset);
	
	        //RAYCASTING
	        /************************/
	
	        //Setup Raycast from target to camera
	        Vector3 raydirection = Vector3.Normalize(newPos - target.transform.position);
	        float rayDist = Vector3.Distance(newPos, target.transform.position);
	        RaycastHit hit;

            /*If there is an object between the target and the camera, 
	         set the position to the closest point with nothing between the two*/

            Vector3 nextPos;
	        if (Physics.Raycast(target.transform.position, raydirection, out hit, rayDist))
	        {
                nextPos = target.transform.position + (raydirection * (hit.distance - camCollisionRadius));
	        }
	        else //If nothing, just set the point to the newPos
	        {
                nextPos = target.transform.position - (rotation * offset);
	        }

            //Smooth the camera movement
            transform.position = Vector3.SmoothDamp(transform.position, nextPos, ref velocity, CamSmoothing);
            //Finally, rotate camera so its forward looks towards the target
            transform.LookAt(target.transform);
        }
    }

    private void OnGUI() 
    {
        
    }
    
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
