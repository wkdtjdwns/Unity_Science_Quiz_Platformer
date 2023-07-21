using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameManager game_manager;

    Animator anim;

    void Awake() // �������� ����
    {
        // �ʱ�ȭ ��
        anim = GetComponent<Animator>();
    }

    void Update() // �� �����Ӹ���
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
