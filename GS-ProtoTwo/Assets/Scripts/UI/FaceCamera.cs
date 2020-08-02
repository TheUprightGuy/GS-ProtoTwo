using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public TMPro.TextMeshPro text;

    public bool active = false;
    public int layer;

    public GameObject mouseOverTip;
    public TMPro.TextMeshPro tooltipText;

    private void Start()
    {
        if (mouseOverTip)
        {
            tooltipText = mouseOverTip.GetComponent<TMPro.TextMeshPro>();
        }
        text = GetComponent<TMPro.TextMeshPro>();
        SkillTreeManager.instance.toggleLayers += SetActive;
        ShowTooltip(false);

        SetActive(0);
    }
    private void OnDestroy()
    {
        SkillTreeManager.instance.toggleLayers -= SetActive;
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        if (tooltipText)
        {
            tooltipText.transform.localRotation = transform.localRotation;
        }
    }

    public void SetText(int _links)
    {
        if (tooltipText)
        {
            // Multiply by Stat Bonus
            tooltipText.SetText("+" + _links.ToString());
        }
    }

    public void ShowTooltip(bool _toggle)
    {
        if (mouseOverTip)
        {
            mouseOverTip.SetActive(_toggle);
        }
    }

    public void SetActive(int _layer)
    {
        if (layer == _layer)
        {
            text.enabled = true;
        }
        else
        {
            text.enabled = false;
        }
    }
}
