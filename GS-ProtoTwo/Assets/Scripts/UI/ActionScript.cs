using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionScript : MonoBehaviour
{
    // temp
    public GameObject actionMenu;
    public GameObject allyTargetMenu;
    public GameObject enemyTargetMenu;
    public GameObject abilityMenu;
    public GameObject magicMenu;
    public GameObject itemMenu;

    // temp
    public TMPro.TextMeshProUGUI playerName;
    // temp
    public GameObject curMenu;
    public GameObject prevMenu;

    public BaseCharacterClass player;

    private void Start()
    {
        curMenu = actionMenu;
        prevMenu = curMenu;
        ToggleMenu();

        CombatController.instance.startTurn += ResetMenu;
        CombatController.instance.playerRef += PlayerRef;
        CombatController.instance.chooseTarget += ChooseTarget;
    }

    private void OnDestroy()
    {
        CombatController.instance.startTurn -= ResetMenu;
        CombatController.instance.playerRef -= PlayerRef;
        CombatController.instance.chooseTarget -= ChooseTarget;
    }

    public void PlayerRef(BaseCharacterClass _player)
    {
        player = _player;
        playerName.SetText(player.stats.name);
    }

    // Menu Function Calls Go Here
    public void Attack()
    {
        prevMenu = curMenu;
        curMenu = enemyTargetMenu;
        player.actionDelegate = player.Attack;
    }

    public void Ability()
    {
        prevMenu = curMenu;
        curMenu = abilityMenu;
        player.actionDelegate = player.Ability;
    }

    public void Magic()
    {
        prevMenu = curMenu;
        curMenu = magicMenu;
        player.actionDelegate = player.Magic;
    }

    public void Item()
    {
        prevMenu = curMenu;
        curMenu = itemMenu;
        //curMenu = enemyTargetMenu;
        player.actionDelegate = player.Item;
    }

    public void Back()
    {
        curMenu = prevMenu;
        prevMenu = actionMenu;
        CombatController.instance.SetTarget(-1);
    }

    public void ChooseTarget(PlayerController _player, ActionDelegate _action, bool _offensive)
    {
        if (player == _player)
        {
            player.actionDelegate = _action;
            prevMenu = curMenu;
            curMenu = (_offensive) ? enemyTargetMenu : allyTargetMenu;
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        actionMenu.SetActive(false);
        allyTargetMenu.SetActive(false);
        enemyTargetMenu.SetActive(false);

        abilityMenu.SetActive(false);
        magicMenu.SetActive(false);
        itemMenu.SetActive(false);

        curMenu.SetActive(true);
        Invoke("SelectFirst", 0.01f);
    }
    public void SelectFirst()
    {
        if (curMenu == enemyTargetMenu && CombatController.instance.CheckEnemies())
        {
            Debug.Log("No more enemies");
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(curMenu.GetComponentInChildren<Button>().gameObject);
        }
    }

    public void ResetMenu(PlayerController _player)
    {
        if (player == _player)
        {
            curMenu = actionMenu;
            prevMenu = curMenu;
            ToggleMenu();
        }
    }
}
