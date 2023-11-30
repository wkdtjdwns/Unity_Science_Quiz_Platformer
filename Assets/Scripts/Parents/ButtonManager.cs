using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void SoundHear() { SoundManager.instance.PlaySound("sound hear"); }

    public void GameExit()
    {
        SoundManager.instance.PlaySound("button");

        // 유니티에서만 나가지는 코드
        UnityEditor.EditorApplication.isPlaying = false;

        // 유니티가 아닌 모든 곳에서 나가지는 코드
        Application.Quit();
    }
}
