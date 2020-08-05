using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public List<Node> links;
    public LineRenderer lineRenderer;

    public bool completed = false;
    public Material completedMat;

    
    // Check if a line exists back to the object
    public bool Check(Node _link)
    {
        foreach(Node n in links)
        {
            if (n == _link)
            {
                return true;
            }
        }
        return false;
    }

    public void Complete()
    {
        completed = true;
        lineRenderer.material = completedMat;
    }

    public void ToggleLink(Node _link)
    {
        lineRenderer.enabled = (Check(_link));   
    }


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        SkillTreeManager.instance.updateLinks += Arc;
        SkillTreeManager.instance.toggleLinks += ToggleLink;

        lineRenderer.startColor = links[0].GetComponent<MeshRenderer>().material.color;
        lineRenderer.endColor = links[1].GetComponent<MeshRenderer>().material.color;
    }

    private void OnDestroy()
    {
        SkillTreeManager.instance.updateLinks -= Arc;
        SkillTreeManager.instance.toggleLinks -= ToggleLink;
    }

    public void Setup(Node _parent, Node _link)
    {
        links.Add(_parent);
        links.Add(_link);
        Arc();
    }

    public void Arc()
    {
        int angleBetween = (int)Vector3.Angle(transform.position, links[1].transform.position);

        if (angleBetween == 0)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, links[1].transform.position);
        }
        else
        {
            lineRenderer.positionCount = angleBetween;

            Vector3 lastPosition = transform.position;

            for (int angle = 0; angle < lineRenderer.positionCount; angle++)
            {
                Vector3 temp = Vector3.RotateTowards(lastPosition, links[1].transform.position, DegToRad(1.0f), 0.0f);
                lineRenderer.SetPosition(angle, temp);
                lastPosition = temp;
            }
        }
    }


    public float DegToRad(float _angle)
    {
        return (_angle / 180 * Mathf.PI);
    }

    public void Toggle(bool _toggle)
    {
        lineRenderer.enabled = _toggle;
    }
}
