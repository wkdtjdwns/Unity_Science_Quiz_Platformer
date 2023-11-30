using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonManager : ButtonManager
{
    StartGameManger game_manger;

    private void Awake() { game_manger = GameObject.Find("GameManager").gameObject.GetComponent<StartGameManger>(); }

    public void GameStart() { SoundManager.instance.PlayBgm("home"); game_manger.GameStart(); }
}