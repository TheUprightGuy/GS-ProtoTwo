using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupAllies : MonoBehaviour
{
    public GameObject allyButtonPrefab;

    #region Callbacks
    private void Awake()
    {
        CombatController.instance.addAllyButton += AddAllyButton;
    }

    private void OnDestroy()
    {
        CombatController.instance.addAllyButton -= AddAllyButton;
    }

    public void AddAllyButton(PlayerController _player)
    {
        TargetScript temp = Instantiate(allyButtonPrefab, this.transform).GetComponent<TargetScript>();
        temp.Setup(_player);
        GameplayUIScript.instance.targets.Add(temp);
    }
    #endregion Callbacks
}
