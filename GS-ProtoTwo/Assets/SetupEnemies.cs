using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupEnemies : MonoBehaviour
{
    public GameObject enemyButtonPrefab;

    #region Callbacks
    private void Start()
    {
        CombatController.instance.addEnemyButton += AddEnemyButton;

    }

    private void OnDestroy()
    {
        CombatController.instance.addEnemyButton -= AddEnemyButton;
    }


    public void AddEnemyButton(int _id)//EnemyController _enemy)
    {
        TargetScript temp = Instantiate(enemyButtonPrefab, this.transform).GetComponent<TargetScript>();
        temp.id = _id;        
    }
    #endregion Callbacks
}
