using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetTextFromParent : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TextMeshPro>().text = transform.parent.name;
    }
}
