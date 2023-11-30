using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        GuideDontDestroyOnLoad();
        SoundManagerDontDestroyOnLoad();
    }

    private void GuideDontDestroyOnLoad()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Guide");

        if (objs.Length == 1) { DontDestroyOnLoad(objs[0]); }
        else
        {
            for (int index = 1; index < objs.Length; index++)
            {
                Destroy(objs[index]);
            }
        }
    }

    private void SoundManagerDontDestroyOnLoad()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundManager");

        if (objs.Length == 1) { DontDestroyOnLoad(objs[0]); }
        else
        {
            for (int index = 1; index < objs.Length; index++)
            {
                Destroy(objs[index]);
            }
        }
    }
}
