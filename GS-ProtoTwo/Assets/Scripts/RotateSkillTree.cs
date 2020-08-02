using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkillTree : MonoBehaviour
{
    public float rotSpeed = 10;
    #region Setup
    // Cache Skill Tree Reference
    [HideInInspector] public SkillTreeManager skillTreeManager;
    private void Start()
    {
        skillTreeManager = SkillTreeManager.instance;
    }
    #endregion Setup

    float rotX, rotY;
    private void OnMouseDrag()
    {
        rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        skillTreeManager.transform.Rotate(Vector3.up, -rotX, Space.World);
        skillTreeManager.transform.Rotate(Vector3.right, rotY, Space.World);

        skillTreeManager.UpdateLinks();

        skillTreeManager.rotating = false;
    }
}
