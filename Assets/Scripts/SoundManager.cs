using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] audio_clips;
    public AudioClip[] bgm_clips;

    // SoundManager�� �ִ� 2���� player ������Ʈ ��ü
    AudioSource bgm_player;
    AudioSource sfx_player;

    // �ɼǿ��� ������ ���� ������
    public Slider bgm_slider;
    public Slider sfx_slider;

    public static SoundManager instance;

    void Awake() // �������� ����
    {
        instance = this; // instance ������ this�� ������

        bgm_player = GameObject.Find("Bgm Player").GetComponent<AudioSource>();
        sfx_player = GameObject.Find("Sfx Player").GetComponent<AudioSource>();

        bgm_slider = bgm_slider.GetComponent<Slider>();
        sfx_slider = sfx_slider.GetComponent<Slider>();

        // onValueChanged�� ���� �����̴��� ���� ����Ǿ��� �� �߻��� �̺�Ʈ�� ���� �� �� �ְ� ��
        bgm_slider.onValueChanged.AddListener(ChangeBgmSound);
        sfx_slider.onValueChanged.AddListener(ChangeSfxSound);

        bgm_player.volume = 0.2f;
        sfx_player.volume = 0.2f;
    }

    public void PlaySound(string type)
    {
        int index = 0;

        // �� ��Ȳ�� �´� ���� ���
        switch (type)
        {
            case "Jump": index = 0; break;
            case "Correct": index = 1; break;
            case "Wrong": index = 2; break;
            case "Damaged": index = 3; break;
            case "Clear": index = 4; break;
            case "Game Over": index = 5; break;
            case "Quit": index = 6; break;
            case "Slow Platform": index = 7; break;
            case "Fast Platform": index = 8; break;
            case "Higher Platform": index = 9; break;
            case "Button": index = 10; break;
            case "Option": index = 11; break;
            case "Text": index = 12; break;
        }

        sfx_player.clip = audio_clips[index];
        sfx_player.PlayOneShot(sfx_player.clip);
    }

    void ChangeBgmSound(float value)
    {
        bgm_player.volume = value;
    }

    void ChangeSfxSound(float value)
    {
        sfx_player.volume = value;
    }
}
