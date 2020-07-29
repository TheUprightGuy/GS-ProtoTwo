using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerSpeed = 1.0f;
    [Range(0.0f, 0.25f), Tooltip("0.0f == Off")]
    public float MoveSmoothing = 0.1f;

    public float GroundPoundForce = 50.0f;
    public bool SpideyPowers = true;
    private Vector3 velocity;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveVec = (((transform.forward) * vertical)
            + ((transform.right) * horizontal)) * PlayerSpeed;
        

        rb.AddForce(moveVec);
        if (SpideyPowers)
        {
            StickToGround();
        }

    }

    void StickToGround()
    {
        rb.AddForce(-transform.up * GroundPoundForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            SpideyPowers = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            SpideyPowers = true;
        }
    }

}
