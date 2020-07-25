using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupStatusBar : MonoBehaviour
{
    public GameObject playerStatusBarPrefab;

    public void Setup(PlayerController _player)
    {
        PlayerStatusBar temp = Instantiate(playerStatusBarPrefab, this.transform).GetComponent<PlayerStatusBar>();
        temp.Setup(_player);       
    }
}
