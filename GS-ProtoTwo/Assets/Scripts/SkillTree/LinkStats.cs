using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkStats : MonoBehaviour
{
    [Header("Required Fields")]
    public Stats actualStats;
    [Header("Debug")]
    public Stats statsProvided;

    private void Start()
    {
        statsProvided = Instantiate<Stats>(actualStats);
        SkillTreeManager.instance.updateStats += UpdateStats;
    }

    private void OnDestroy()
    {
        SkillTreeManager.instance.updateStats -= UpdateStats;
    }

    public void GetVal(int _links)
    {
        statsProvided.Multiply(actualStats, _links);
    }

    public void UpdateStats()
    {
        SkillTreeManager.instance.stats.AddStats(statsProvided);
        Debug.Log("Updating Stats");
        TabMenu.instance.LoadTabMenuScreenData();
    }
}
