using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetScript : MonoBehaviour, ISelectHandler
{
    public int id;

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
