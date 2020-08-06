using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Flicker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GameObject objectToFlicker;
    bool enable = false;
    // Update is called once per frame

    void Update()
    {
        if (!Application.isPlaying)
        {
            enable = !enable;
            objectToFlicker.SetActive(enable);
        }
        else
        {
            objectToFlicker.SetActive(true);
        }
    }
}
