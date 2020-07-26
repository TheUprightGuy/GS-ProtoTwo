using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerSpeed = 1.0f;
    public float PlayerCollisionRad = 1.0f;

    [Range(0.0f, 0.25f), Tooltip("0.0f == Off")]
    public float MoveSmoothing = 0.1f;
    private Vector3 velocity;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 newPos = transform.position
            + ((transform.forward * PlayerSpeed) * vertical)
            + ((transform.right * PlayerSpeed) * horizontal);
        
        transform.position = Vector3.SmoothDamp(transform.position, CheckCollisions(newPos), ref velocity, MoveSmoothing);
    }
    Vector3 CheckCollisions(Vector3 nextPos)
    {
        Vector3 raydirection = Vector3.Normalize(nextPos - transform.position);
        float rayDist = Vector3.Distance(nextPos, transform.position) + PlayerCollisionRad;
        RaycastHit hit;

        
        if (Physics.SphereCast(transform.position, PlayerCollisionRad, raydirection, out hit, rayDist))
        {
            return (transform.position);
        }
        return (nextPos);

    }
}
