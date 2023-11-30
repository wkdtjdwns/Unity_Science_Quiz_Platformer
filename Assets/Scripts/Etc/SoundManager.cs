using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private AudioClip[] bgm_clips;
    [SerializeField]
    private AudioClip[] audio_clips;

    public float bgm_volume;
    public float sfx_volume;

    public AudioSource bgm_player;
    public AudioSource sfx_player;

    private void Awake()
    {
        instance = this;

        bgm_volume = 0.5f;
        sfx_volume = 0.5f;

        bgm_player = GameObject.Find("Bgm Player").gameObject.GetComponent<AudioSource>();
        sfx_player = GameObject.Find("Sfx Player").gameObject.GetComponent<AudioSource>();

        PlayBgm("start");
    }

    private void Update()
    {
        ChangeBgmSound();
        ChangeSfxSound();
    }

    public void PlaySound(string type)
    {
        int index = 0;

        switch (type)
        {
            case "button":  index = 0; break;
            case "get solution": index = 1; break;
            case "sell": index = 2; break;
            case "easter 3": index = 3; break;
            case "correct": index = 4; break;
            case "wrong": index = 5; break;
            case "purchase": index = 6; break;
            case "jump": index = 7; break;
            case "need": index = 8; break;
            case "talk": index = 9; break;
            case "sound hear": index = 10; break;
            case "mix solution": index = 11; break;
            case "put solution": index = 12; break;
            case "put goods": index = 13; break;
        }

        sfx_player.clip = audio_clips[index];
        sfx_player.PlayOneShot(sfx_player.clip);
    }

    public void PlayBgm(string type)
    {
        int index = 0;

        switch (type)
        {
            case "start": index = 0; break;
            case "home": index = 1; break;
            case "running": index = 2; break;
            case "experiment": index = 3; break;
        }

        bgm_player.clip = bgm_clips[index];
        bgm_player.Play();
    }

    private void ChangeBgmSound() { bgm_player.volume = bgm_volume * 0.15f; }

    private void ChangeSfxSound() { sfx_player.volume = sfx_volume; }
}
