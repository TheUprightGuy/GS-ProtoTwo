using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerSpeed = 1.0f;
    [Range(0.0f, 0.25f), Tooltip("0.0f == Off")]
    public float MoveSmoothing = 0.1f;

    public float CheckToGroundDistance = 1.0f;
    public float GroundPoundForce = 50.0f;
    public bool SpideyPowers = true;
    private Vector3 velocity;
    public GameInfo gameInfo;

    private Rigidbody rb;
    private Animator playerAnims;
    // Start is called before the first frame update
    void Start()
    {
        playerAnims = GetComponentInChildren<Animator>();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameInfo.paused) MovePlayer();
    }


    private void FixedUpdate()
    {
        RaycastHit raycastHit;

        if (Physics.Raycast(transform.position, -transform.up, out raycastHit, Mathf.Infinity ))
        {
            if ((raycastHit.distance > CheckToGroundDistance))
            {
                SpideyPowers = true;
            }
            else
            {
                SpideyPowers = false;
            }
        }
    }
    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveVec = Vector3.Normalize((((transform.forward) * vertical)
            + ((transform.right) * horizontal))) * PlayerSpeed;

        if (moveVec != Vector3.zero)
        {
            playerAnims.SetBool("Walk", true);
        }
        else
        {
            playerAnims.SetBool("Walk", false);
        }
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
        //if (collision.transform.tag == "Ground")
        //{
        //    SpideyPowers = false;
        //}

        GetComponent<EncounterStart>().EncounterOnCollisionEnter(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (collision.transform.tag == "Ground")
        //{
        //    SpideyPowers = true;
        //}
    }

}
