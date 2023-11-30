using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public Sprite[] backgrounds;

    public GameObject game_manager_obj;
    public GameManager game_manager;

    SpriteRenderer sprite_renderer;

    void Awake() // �������� ����
    {
        game_manager_obj = GameObject.Find("GameManager").gameObject;
        game_manager = game_manager_obj.GetComponent<GameManager>();

        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Update() // �� �����Ӹ���
    {
        UpdateBackground();
    }

    void UpdateBackground()
    {
        switch (game_manager.stage_index)
        {
            case 0:
                sprite_renderer.sprite = backgrounds[0];
                game_manager.quiz_text.color = new Color(0, 0, 0);
                break;

            case 1:
                sprite_renderer.sprite = backgrounds[2];
                game_manager.quiz_text.color = new Color(1, 1, 1);
                break;

            case 2:
            case 3:
                sprite_renderer.sprite = backgrounds[1];
                game_manager.quiz_text.color = new Color(0.575f, 0.575f, 0.575f);
                break;
        }
    }
}
