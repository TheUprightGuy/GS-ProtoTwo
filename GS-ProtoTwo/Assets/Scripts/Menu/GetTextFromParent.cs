using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTextFromParent : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Text>().text = transform.parent.name;
    }
}
