using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupEnemies : MonoBehaviour
{
    public GameObject enemyButtonPrefab;

    #region Callbacks
    private void Awake()
    {
        CombatController.instance.addEnemyButton += AddEnemyButton;
    }

    private void OnDestroy()
    {
        CombatController.instance.addEnemyButton -= AddEnemyButton;
    }

    public void AddEnemyButton(EnemyController _enemy)
    {
        TargetScript temp = Instantiate(enemyButtonPrefab, this.transform).GetComponent<TargetScript>();
        temp.Setup(_enemy);
        GameplayUIScript.instance.targets.Add(temp);
    }
    #endregion Callbacks
}
