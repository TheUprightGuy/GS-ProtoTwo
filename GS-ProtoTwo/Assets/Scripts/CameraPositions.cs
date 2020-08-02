using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositions : MonoBehaviour
{
    [Header("Setup Fields")]
    public List<Vector3> Positions;
    public float lerpSpeed;
    // Private Variables
    private Vector3 lerpTarget;
    private bool lerping = false;

    public void Update()
    {
        if (lerping)
        {
            LerpTo();
        }

        if (Input.GetAxis("Mouse ScrollWheel") >= 0.1f) // forward
        {
            BeginLerpTo(Positions[0]);
            SkillTreeManager.instance.ToggleLayers(0);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") <= -0.1f) // backwards
        {
            BeginLerpTo(Positions[1]);
            SkillTreeManager.instance.ToggleLayers(1);
        }
    }

    public void BeginLerpTo(Vector3 _pos)
    {
        lerpTarget = _pos;
        lerping = true;
    }


    public void LerpTo()
    {
        transform.position = Vector3.Lerp(transform.position, lerpTarget, lerpSpeed * Time.deltaTime);
        if (transform.position == lerpTarget)
        {
            lerping = false;
        }
    }
}
