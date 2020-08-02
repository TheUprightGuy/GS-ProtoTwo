using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsTable : MonoBehaviour
{
    public static StatsTable instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one StatsTable exists!");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public TMPro.TextMeshProUGUI attackText;
    public TMPro.TextMeshProUGUI magicText;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI manaText;
    public TMPro.TextMeshProUGUI defenseText;
    public TMPro.TextMeshProUGUI speedText;

    public void UpdateText(Stats _stats)
    {
        attackText.SetText(_stats.attack.ToString());
        magicText.SetText(_stats.magic.ToString());
        healthText.SetText(_stats.hp.ToString());
        manaText.SetText(_stats.mp.ToString());
        defenseText.SetText(_stats.defense.ToString());
        speedText.SetText(_stats.speed.ToString());
    }
}
