using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject statsTable;
    public GameObject tipsTable;
    public GameObject tipsButton;
    public Text showAndHideText;
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
        if (tipsTable.activeSelf) showAndHideText.text = "HIDE";
        else showAndHideText.text = "SHOW";
    }

    public void NextTip()
    {
        tips[tipsShown].SetActive(false);
        tipsShown = (tipsShown + 1) % 2;
        tips[tipsShown].SetActive(true);
    }
}
