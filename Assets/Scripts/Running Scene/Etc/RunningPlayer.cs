using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningPlayer : GameManager
{
    [SerializeField]
    private int jump_cnt;
    private bool is_jumping;

    public int max_jump_cnt;
    public float jump_power;

    public float health;

    [SerializeField]
    private GameObject option_obj;
    Option option;

    Rigidbody2D rigid;
    Animator anim;

    RunningGameManager game_manager;

    private void Awake()
    {
        Setting();

        option = option_obj.GetComponent<Option>();

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        game_manager = GameObject.Find("GameManager").gameObject.GetComponent<RunningGameManager>();
    }

    private void Setting()
    {
        jump_cnt = 0;
        max_jump_cnt = 2;
        jump_power = 850f;
        health = 10f + add_health;
    }

    private void OnDisable() { anim.SetBool("is_running", false); }

    private void Update()
    {
        Move();

        Jump();

        CheatKey();
    }

    private void Move()
    {
        HealthDown();

        if (!is_jumping) { anim.SetBool("is_running", true); }
        else { anim.SetBool("is_running", false); }
    }

    private void HealthDown()
    {
        if (health > 0) { health -= Time.deltaTime; }

        else { game_manager.is_tired = true; }
    }

    private void Jump()
    {
        if (Input.GetMouseButtonDown(0) && jump_cnt < max_jump_cnt && !option.is_option)
        {
            SoundManager.instance.PlaySound("jump");

            jump_cnt++;

            rigid.velocity = Vector2.zero;

            rigid.AddForce(new Vector2(0, jump_power));

            is_jumping = true;
            anim.SetBool("is_jumping", true);
        }

        else if (Input.GetMouseButtonUp(0) && rigid.velocity.y > 0) { rigid.velocity = rigid.velocity * 0.5f; }

    }

    private void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1)) { health = 1f; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            jump_cnt = 0;

            is_jumping = false;
            anim.SetBool("is_jumping", false);
        }
    }
}
