using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionScript : MonoBehaviour
{
    // temp
    public GameObject actionMenu;
    public GameObject targetMenu;
    public GameObject abilityMenu;
    public GameObject magicMenu;
    public GameObject itemMenu;

    // temp
    public GameObject curMenu;
    public GameObject prevMenu;

    private void Start()
    {
        curMenu = actionMenu;
        prevMenu = curMenu;
        ToggleMenu();

        CombatController.instance.startTurn += ResetMenu;
    }

    private void OnDestroy()
    {
        CombatController.instance.startTurn -= ResetMenu;
    }

    // Menu Function Calls Go Here
    public void Attack()
    {
        prevMenu = curMenu;
        curMenu = targetMenu;
    }

    public void Ability()
    {
        prevMenu = curMenu;
        curMenu = abilityMenu;
    }

    public void Magic()
    {
        prevMenu = curMenu;
        curMenu = magicMenu;
    }

    public void Item()
    {
        prevMenu = curMenu;
        curMenu = itemMenu;
    }

    public void Back()
    {
        curMenu = prevMenu;
    }

    public void ToggleMenu()
    {
        actionMenu.SetActive(false);
        targetMenu.SetActive(false);

        abilityMenu.SetActive(false);
        magicMenu.SetActive(false);
        itemMenu.SetActive(false);

        curMenu.SetActive(true);
        Invoke("SelectFirst", 0.01f);
    }
    public void SelectFirst()
    {
        EventSystem.current.SetSelectedGameObject(curMenu.GetComponentInChildren<Button>().gameObject);
    }

    public void ResetMenu()
    {
        curMenu = actionMenu;
        prevMenu = curMenu;
        ToggleMenu();
    }
}
