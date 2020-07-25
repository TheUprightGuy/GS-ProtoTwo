using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject target;

    public bool invertX = false;
    public bool invertY = false;

    public float rotateSpeed = 5;
    public float zoomSpeed = 5;

    public float minYAngle = 0.0f;
    public float maxYAngle = 45.0f;

    public float camCollisionRadius = 1.0f;

    public bool ThirdPersonOnlyWhenLocked = true;
    [Tooltip("Press ` to enable/disable cursor locking.")]
    public bool UnlockCameraKeyBind = true;
    Vector3 offset;

    float storeYPos = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        offset = target.transform.position - transform.position;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
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
    }

    void LateUpdate()
    {
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
	        target.transform.Rotate(0, horizontal, 0);
	
	        
	        float desiredAngleY = target.transform.eulerAngles.y;
	
	        //Get the current stored Y angle and add mouse axis to it
	        storeYPos += vertical;
	        storeYPos = Mathf.Clamp(storeYPos, minYAngle, maxYAngle);
	
	        
	        Quaternion rotation = Quaternion.Euler(storeYPos, desiredAngleY, 0);
	
	        offset.z += zoom;
	
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
	        if (Physics.Raycast(target.transform.position, raydirection, out hit, rayDist))
	        {
	            transform.position = target.transform.position + (raydirection * (hit.distance - camCollisionRadius));
	        }
	        else //If nothing, just set the point to the newPos
	        {
	            transform.position = target.transform.position - (rotation * offset);
	        }
	
	        //Finally, rotate camera so its forward looks towards the target
	        transform.LookAt(target.transform);
        }
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
