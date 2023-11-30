using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : GameManager
{
    [SerializeField]
    private Slider health_slider;

    RunningGameManager game_manager;
    RunningPlayer player;

    private void Awake()
    {
        health_slider.interactable = false;

        game_manager = GameObject.Find("GameManager").gameObject.GetComponent<RunningGameManager>();
        player = GameObject.Find("Player").gameObject.GetComponent<RunningPlayer>();
    }

    private void OnEnable()
    {
        health_slider.maxValue = player.health;
        health_slider.value = health_slider.maxValue;
    }

    private void Update() { health_slider.value = player.health; }

    void SliderUpdate() { health_slider.value = player.health; }
}
