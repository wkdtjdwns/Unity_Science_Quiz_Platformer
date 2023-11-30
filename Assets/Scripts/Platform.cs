using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameManager game_manager;

    Animator anim;

    void Awake() // 시작하자 마자
    {
        // 초기화 함
        anim = GetComponent<Animator>();
    }

    void Update() // 매 프레임마다
    {
        Plat();
    }

    void Plat()
    {
        if (this.gameObject.tag == "Move")
        {
            if (game_manager.is_correct)
            {
                anim.SetBool("is_correct", true);
            }

            if (game_manager.is_quiz)
            {
                anim.SetBool("is_correct", false);
            }
        }

        else if (this.gameObject.tag == "Delete")
        {
            if (game_manager.is_correct)
            {
                gameObject.SetActive(false);

                Invoke("RePlat", 3);
            }

            if (game_manager.is_quiz)
            {
                RePlat();
            }
        }

    }

    void RePlat()
    {
        gameObject.SetActive(true);
    }
}
