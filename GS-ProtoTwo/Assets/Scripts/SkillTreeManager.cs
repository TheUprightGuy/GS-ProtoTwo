using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    #region Singleton
    public static SkillTreeManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Manager exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    [Header("Setup Requirements")]
    public GameObject linkPrefab;
    public Stats stats;

    public int pointsToSpend;

    private Quaternion target;
    [HideInInspector] public bool rotating = false;

    public List<Node> nodes;

    public bool SpendPoint()
    {
        if (pointsToSpend > 0)
        {
            pointsToSpend--;
            return true;
        }
        return false;
    }

    public void AddNode(Node _node)
    {
        if (!nodes.Contains(_node))
        {
            nodes.Add(_node);
        }
    }

    public bool NodeCompleted(Node _node)
    {
        return (nodes.Contains(_node));
    }

    private void Start()
    {
        // Clears Previous Stats & Start Anew
        stats.Clear();
        stats.Setup();
    }

    public void Update()
    {
        if (rotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 2.5f);
            UpdateLinks();

            if (Quaternion.Angle(transform.rotation, target) <= 0.5f)
            {
                rotating = false;
            }
        }
    }

    public void SetTarget(Quaternion _target)
    {
        target = _target;
        rotating = true;
    }

    #region Callbacks
    public event Action updateLinks;
    public void UpdateLinks()
    {
        if (updateLinks != null)
        {
            updateLinks();
        }
    }

    public event Action<Node> toggleLinks;
    public void ToggleLinks(Node _link)
    {
        if (toggleLinks != null)
        {
            toggleLinks(_link);
        }
    }

    public event Action checkLinks;
    public void CheckLinks()
    {
        if (checkLinks != null)
        {
            checkLinks();
        }
    }

    public event Action updateStats;
    public void UpdateStats()
    {
        if (updateStats != null)
        {
            stats.Clear();
            updateStats();
            stats.SetupHPMP();
            StatsTable.instance.UpdateText(stats);
        }
    }

    public event Action<int> toggleLayers;
    public void ToggleLayers(int _layer)
    {
        if (toggleLayers != null)
        {
            toggleLayers(_layer);
        }
    }
    #endregion Callbacks
}
