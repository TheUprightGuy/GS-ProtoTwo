using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTracker : MonoBehaviour
{
    #region Singleton
    public static PositionTracker instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple Position Trackers Exist!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion Singleton

    [Header("Setup Fields")]
    public Node currentPosition;
    public Material currentMat;
    public Material completedMat;
    public Camera skillTreeCamera;

    // List of Links so far (used for toggling on).
    [HideInInspector] public List<Link> links;
    private SkillTreeManager skillTreeManager;

    #region Setup
    private void Start()
    {
        // Cache reference to Singleton
        skillTreeManager = SkillTreeManager.instance;
        Invoke("DelayedStart", 0.02f);
    }

    public void DelayedStart()
    {
        SetPosition(currentPosition);
    }
    #endregion Setup

    public bool IsThisLinked(Node _link)
    {
        if (_link == currentPosition)
        {
            return false;
        }

        if (currentPosition.CheckForLink(_link) || _link.CheckForLink(currentPosition))
        {
            return true;
        }

        return false;      
    }

    public void SetPosition(Node _position)
    {      
        currentPosition.ResetMat();
        currentPosition.currentSelected = false;
        currentPosition = _position;
        currentPosition.currentSelected = true;
        // Turns on Links to Neighbours
        skillTreeManager.ToggleLinks(currentPosition);

        // Draw Path so far
        foreach (Link n in links)
        {
            n.Toggle(true);
        }
        // Set Current Position - Use a Particle Effect or Something Later
        currentPosition.SetMat(currentMat);
        // Updates # of Links & Stats they Provide - Probs best to only update those connected to latest link?
        skillTreeManager.CheckLinks();
        skillTreeManager.UpdateStats();

        Quaternion target;
        // Get Rotation for SkillTree
        Quaternion toCam = Quaternion.LookRotation(skillTreeCamera.transform.position - skillTreeManager.transform.position);
        Quaternion toNode = Quaternion.LookRotation(currentPosition.transform.localPosition);
        Quaternion fromNode = Quaternion.Inverse(toNode);
        target = toCam * fromNode;

        skillTreeManager.SetTarget(target);

        if (currentPosition.statsText)
        {
            currentPosition.statsText.ShowTooltip(true);
        }
    }


    public void AddLine(Link _line)
    {
        _line.Complete();
        links.Add(_line);
    }

}
