using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Stats")]


public class GameStats : ScriptableObject
{

    public EnumBus.GAME_STATE currentGameState = EnumBus.GAME_STATE.START_MENU;
    public string currentSceneName = "";
    public int currentSpawnPointId = -1;
    public int wealth = 0;

}
