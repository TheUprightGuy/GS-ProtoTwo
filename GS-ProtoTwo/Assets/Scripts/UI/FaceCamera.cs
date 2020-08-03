using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public TMPro.TextMeshPro text;
    public string baseString;

    public bool active = true;
    public int layer;

    private void Start()
    {
        text = GetComponent<TMPro.TextMeshPro>();
        baseString = text.text;
        SkillTreeManager.instance.toggleLayers += SetActive;
        ShowTooltip(false);

        SetActive(0);
    }
    private void OnDestroy()
    {
        SkillTreeManager.instance.toggleLayers -= SetActive;
    }


    public void SetText(int _links)
    {
        text.SetText(baseString + "\n" + _links.ToString());
    }

    public void ShowTooltip(bool _toggle)
    {
        if (text && active)
        {
            text.enabled = _toggle;
        }
    }

    public void SetActive(int _layer)
    {
        if (layer == _layer)
        {
            active = true;
        }
        else
        {
            active = false;
            ShowTooltip(false);
        }
    }
}
