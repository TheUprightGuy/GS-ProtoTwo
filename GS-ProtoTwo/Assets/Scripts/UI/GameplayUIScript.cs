using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIScript : MonoBehaviour
{
    public GameObject actionCanvasPrefab;
    public SetupStatusBar statusBars;
    public List<ActionScript> playerUI;

    private void Start()
    {
        CombatController.instance.toggleActionCanvas += ToggleActionCanvas;
        CombatController.instance.setupCanvas += SetupCanvas;
    }

    private void OnDestroy()
    {
        CombatController.instance.toggleActionCanvas -= ToggleActionCanvas;
        CombatController.instance.setupCanvas -= SetupCanvas;
    }

    public void SetupCanvas(PlayerController _player)
    {
        ActionScript temp = Instantiate(actionCanvasPrefab, this.transform).GetComponent<ActionScript>();
        temp.PlayerRef(_player);
        playerUI.Add(temp);

        foreach (Ability n in _player.stats.abilities)
        {
            temp.abilityMenu.GetComponent<SetupAbility>().AddAbilityButton(_player, n);
        }
        foreach (Magic n in _player.stats.spells)
        {
            temp.magicMenu.GetComponent<SetupMagic>().AddMagicButton(_player, n);
        }

        statusBars.Setup(_player);
    }
       
    public void ToggleActionCanvas(BaseCharacterClass _player, bool _toggle)
    {
        foreach (ActionScript n in playerUI)
        {
            if (n.player == _player)
            {
                n.gameObject.SetActive(_toggle);
            }
            else
            {
                n.gameObject.SetActive(false);
            }
        }
    }
}
