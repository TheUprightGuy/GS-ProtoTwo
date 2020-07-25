using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool active;

    #region Setup
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
    #endregion Setup

    void Update()
    {
        if (active)
            transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
    }

    public void Toggle(bool _toggle)
    {
        active = _toggle;
        spriteRenderer.enabled = active;
    }
}