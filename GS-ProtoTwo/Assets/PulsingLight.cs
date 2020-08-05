using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingLight : MonoBehaviour
{
    public float intensity;
    public float startIntensity;
    public float endIntensity;
    public Light light;


    private void Start()
    {
        light = GetComponent<Light>();
        light.enabled = false;
    }

    private void Update()
    {
        if (enabled)
        {
            intensity = Mathf.PingPong(Time.time * 150000, endIntensity) + startIntensity;
            light.intensity = intensity;
        }
    }

    public void ChangeColor(Element _element)
    {
        light.enabled = true;

        switch(_element)
        {
            case Element.Fire:
            {
                light.enabled = true;
                light.color = Color.red;
                break;
            }
            case Element.Water:
            {
                light.enabled = true;
                light.color = Color.cyan;
                break;
            }
            case Element.Earth:
            {
                light.enabled = true;
                light.color = Color.green;
                break;
            }
            case Element.Lightning:
            {
                light.enabled = true;
                light.color = Color.yellow;
                break;
            }
            case Element.Holy:
            {
                light.enabled = false;
                light.color = Color.white;
                break;
            }
        }
    }
}
