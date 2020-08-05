using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetTextFromParent : MonoBehaviour
{
    private void Start()
    {
        if (GetComponent<Text>())
        {
            GetComponent<Text>().text = transform.parent.name;
        }
        else if (GetComponent<TextMeshProUGUI>())
        {
            GetComponent<TextMeshProUGUI>().SetText(transform.parent.name);
        }
    }
}
