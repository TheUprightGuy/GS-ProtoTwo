using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSwitcher : MonoBehaviour
{
    [HideInInspector] public Material nodeMat;
    public ParticleSystem ps;
    // Me being Lazy
    public void Setup()
    {
        ps.startColor = nodeMat.color;
    }
}
