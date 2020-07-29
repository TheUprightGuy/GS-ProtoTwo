using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "GameState/GameInfo", order = 1)]
public class GameInfo : ScriptableObject
{
   public bool paused = false;
   public bool pauseMenuOpen = false;
}
