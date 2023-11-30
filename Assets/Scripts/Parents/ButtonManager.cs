using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void SoundHear() { SoundManager.instance.PlaySound("sound hear"); }

    public void GameExit()
    {
        SoundManager.instance.PlaySound("button");

        // ����Ƽ������ �������� �ڵ�
        UnityEditor.EditorApplication.isPlaying = false;

        // ����Ƽ�� �ƴ� ��� ������ �������� �ڵ�
        Application.Quit();
    }
}
