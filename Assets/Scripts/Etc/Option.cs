using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField]
    private GameObject option;

    [SerializeField]
    private Slider bgm_slider;
    [SerializeField]
    private Slider sfx_slider;

    public bool is_option;

    private void Start()
    {
        bgm_slider.value = SoundManager.instance.bgm_volume;
        sfx_slider.value = SoundManager.instance.sfx_volume;

        is_option = false;

        bgm_slider.onValueChanged.AddListener(ChangeBgmSound);
        sfx_slider.onValueChanged.AddListener(ChangeSfxSound);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) { OptionOnOff(); }
    }

    public void OptionOnOff()
    {
        SoundManager.instance.PlaySound("button");

        is_option = !is_option;

        option.SetActive(is_option);

        Time.timeScale = is_option ? 0 : 1;
    }

    private void ChangeBgmSound(float value) { SoundManager.instance.bgm_volume = value; }

    private void ChangeSfxSound(float value) { SoundManager.instance.sfx_volume = value; }
}
