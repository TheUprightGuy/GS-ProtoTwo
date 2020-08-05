using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject statsTable;
    public GameObject tipsTable;
    public GameObject tipsButton;
    public GameObject[] tips;
    public int tipsShown = 0;
    

    public void ToggleStats()
    {
        statsTable.SetActive(!statsTable.activeSelf);
    }

    public void ToggleTips()
    {
        tipsTable.SetActive(!tipsTable.activeSelf);
        tipsButton.SetActive(tipsTable.activeSelf);
    }

    public void NextTip()
    {
        tips[tipsShown].SetActive(false);
        tipsShown = (tipsShown + 1) % 2;
        tips[tipsShown].SetActive(true);
    }
}
