using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Setup Fields")]
    public List<Node> neighbours;
    public LinkStats stats;
    [HideInInspector] public int activeLinks = 0;

    private float count = 0;
    private bool counting = false;
    private bool completed = false;
    [HideInInspector] public Material defaultMat;
    [HideInInspector] public List<Link> links;
    [HideInInspector] public FaceCamera statsText;
    private MeshRenderer meshRenderer;
    [HideInInspector] public bool currentSelected = false;

    public Camera skillTreeCamera;

    public List<GameObject> vfx;

    #region Setup
    private void Awake()
    {
        stats = GetComponent<LinkStats>();
        statsText = GetComponentInChildren<FaceCamera>();
        defaultMat = GetComponent<MeshRenderer>().material;
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        // Add Callback
        SkillTreeManager.instance.checkLinks += GetNumLinks;

        // Add a Link for each Neighbour
        GameObject linkPrefab = SkillTreeManager.instance.linkPrefab;
        foreach (Node n in neighbours)
        {
            // Check if Link already Exists
            if (!n.CheckForLink(this))
            {
                // If not Create & Link
                Link temp = Instantiate(linkPrefab, this.transform).GetComponent<Link>();
                temp.Setup(this, n);
                links.Add(temp);
            }
        }

        // Hide Tooltip
        if (statsText)
        {
            statsText.ShowTooltip(false);
        }

        // Just me being Lazy
        VFXSwitcher vfxboi = GetComponentInChildren<VFXSwitcher>();
        MeshRenderer meshyboy = GetComponent<MeshRenderer>();
        vfxboi.nodeMat = meshyboy.material;
        meshyboy.enabled = false;
        vfxboi.Setup();


        // meh
        if (vfx.Count > 0)
        {
            vfx[1].SetActive(false);
            vfx[0].SetActive(true);
        }
    }
    private void OnDestroy()
    {
        // Remove Callback
        SkillTreeManager.instance.checkLinks -= GetNumLinks;
    }
    #endregion Setup
    #region LinkFuncs
    // Pass in Self to test if you have a link already.
    public bool CheckForLink(Node _link)
    {
        // Check each link to see if it goes to that one
        foreach(Link n in links)
        {
            if(n.Check(_link))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckLinkCompleted(Node _link)
    {
        foreach (Link n in links)
        {
            if (n.Check(_link) && n.completed)
            {
                return true;
            }
        }
        return false;
    }

    public void ToggleLinks(Node _link)
    {
        // Check each link to see if it goes to that one
        foreach (Link n in links)
        {
            if (n.Check(_link))
            {
                n.Toggle(true);
            }
        }
    }

    // Get Number of Links to this Node
    public void GetNumLinks()
    {
        activeLinks = 0;

        foreach(Node n in neighbours)
        {
            if (n.CheckLinkCompleted(this) || CheckLinkCompleted(n))
            {
                activeLinks++;
            }
        }

        // Set StatsText to Num of Links
        stats.GetVal(activeLinks);
        if (statsText)
        {
            statsText.SetText(activeLinks);
        }
    }
    public void GetLineFromRoot()
    {
        // Cache List
        List<Link> temp = PositionTracker.instance.currentPosition.links;

        foreach (Link n in temp)
        {
            if (n.links[1] == this)
            {
                n.lineRenderer.material = defaultMat;
                PositionTracker.instance.AddLine(n);
            }
        }

        GetRootFromLine();
    }
    public void GetRootFromLine()
    {
        foreach (Link n in links)
        {
            if (n.links[1] == PositionTracker.instance.currentPosition)
            {
                n.lineRenderer.material = defaultMat;
                PositionTracker.instance.AddLine(n);
            }
        }
    }
    #endregion LinkFuncs
    #region MouseEvents
    private void OnMouseDown()
    {
        // Cache Current Node & Link Status
        Node temp = PositionTracker.instance.currentPosition;
        bool isLinked = PositionTracker.instance.IsThisLinked(this);

        if (isLinked && (!temp.CheckLinkCompleted(this) && !this.CheckLinkCompleted(temp)))
        {
            counting = true;
        }
        else if (isLinked && (temp.CheckLinkCompleted(this) || this.CheckLinkCompleted(temp)))
        {
            counting = false;
        }

        if (SkillTreeManager.instance.NodeCompleted(this))
        {
            counting = true;
        }
    }
    private void OnMouseUp()
    {
        if (!currentSelected)
        {
            Clear();
        }
    }
    private void OnMouseExit()
    {
        Clear();

        // Hide Tooltip
        if (statsText && !currentSelected)
        {
            statsText.ShowTooltip(false);
        }
    }
    private void OnMouseEnter()
    {
        // Show Tooltip
        if (statsText)
        {
            statsText.ShowTooltip(true);
        }
    }
    public void Clear()
    {
        // Reset Tracking Variables
        counting = false;
        completed = false;
        count = 0;
    }
    #endregion MouseEvents

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - skillTreeCamera.transform.position);

        if (counting && !completed)
        {
            count += Time.deltaTime;
            if (count >= 1.0f)
            {
                if (!SkillTreeManager.instance.NodeCompleted(this))
                {
                    if (SkillTreeManager.instance.SpendPoint())
                    {
                        completed = true;
                        GetComponent<MeshRenderer>().material = defaultMat;
                        GetLineFromRoot();
                        PositionTracker.instance.SetPosition(this);
                        SkillTreeManager.instance.AddNode(this);

                        // meh
                        if (vfx.Count > 0)
                        {
                            vfx[0].SetActive(false);
                            vfx[1].SetActive(true);
                        }
                    }
                }
                else
                {
                    completed = true;
                    PositionTracker.instance.SetPosition(this); 
                }
            }
        }
    }

    public void ResetMat()
    {
        meshRenderer.material = defaultMat;
    }

    public void SetMat(Material _mat)
    {
        meshRenderer.material = _mat;
    }
}
