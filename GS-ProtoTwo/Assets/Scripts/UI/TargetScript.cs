using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetScript : MonoBehaviour, ISelectHandler
{
    public EnemyController enemy;
    public PlayerController player;
    public int id;

    public TMPro.TextMeshProUGUI text;

    public void Setup(EnemyController _enemy)
    {
        enemy = _enemy;
        id = enemy.id;
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        text.SetText(enemy.stats.name);
    }

    public void Setup(PlayerController _player)
    {
        player = _player;
        id = player.id;
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        text.SetText(player.stats.name);
    }

    public void OnSelect(BaseEventData eventData)
    {
        CombatController.instance.SetTarget(id);
    }

    public void OnUse()
    {
        CombatController.instance.Submit(id);
        CombatController.instance.SetTarget(-1);
        CombatController.instance.ChangeState(CombatState.ACTION);
    }

}
