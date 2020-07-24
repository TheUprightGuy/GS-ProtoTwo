using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIScript : MonoBehaviour
{
    public GameObject actionCanvas;

    private void Start()
    {
        CombatController.instance.toggleActionCanvas += ToggleActionCanvas;
    }

    private void OnDestroy()
    {
        CombatController.instance.toggleActionCanvas -= ToggleActionCanvas;
    }

    public void ToggleActionCanvas(bool _toggle)
    {
        actionCanvas.SetActive(_toggle);
    }
}
