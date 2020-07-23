using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private int id;

    #region Setup
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    void Start()
    {
        id = GetComponentInParent<PlayerController>().id;
        transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
        EventHandler.instance.toggleTurn += ToggleTurn;
    }

    private void OnDestroy()
    {
        EventHandler.instance.toggleTurn -= ToggleTurn;
    }
    #endregion Setup

    public void ToggleTurn(int _id)
    {
        spriteRenderer.enabled = (id == _id);
    }

}
