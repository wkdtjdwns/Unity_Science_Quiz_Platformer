using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어에 관한 변수
    public float speed;
    public float jump_power;
    public float flip_ratio = 1f;
    public int life;

    public bool is_jump;
    public float jump_time;

    public bool is_fast;
    public bool is_high;

    // GameManager에 대한 변수
    public GameObject game_manager_obj;
    public GameManager game_manager;

    // 초기화 시켜야 하는 변수
    Rigidbody2D rigid;
    SpriteRenderer sprite_renderer;
    Animator anim;

    void Awake()
    {
        // 초기화
        rigid = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        game_manager_obj = GameObject.Find("GameManager").gameObject;
        game_manager = game_manager_obj.GetComponent<GameManager>();

        // z축 고정
        rigid.freezeRotation = true;
    }

    void Update()
    {
        Move();
        Jump();

        if (is_jump)
        {
            jump_time += Time.deltaTime;
        }

        if (game_manager.is_fall)
        {
            OnDamaged(this.transform.localPosition);

            Vector3 pos = this.transform.position;
            pos = new Vector3(0, 0, 0);

            this.transform.localPosition = pos;

            speed = 5;;
            jump_power = 5.5f;

            is_fast = false;
            is_high = false;
        }

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            OnDamaged(this.transform.localPosition);
        }
    }

    void FixedUpdate() // 일정한 간격마다
    {
        // 좌우 이동
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // 속도가 최대 속력을 넘지 못하게 함
        if (rigid.velocity.x > speed)
        {
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        }

        else if (rigid.velocity.x < speed * (-1))
        {
            rigid.velocity = new Vector2(speed * (-1), rigid.velocity.y);
        }

        // 플레이어가 벽에 끼는 것 방지
        if (jump_time > 2.5f)
        {
            is_jump = false;
            anim.SetBool("is_jumping", false);
        }

        // 무한 점프 방지
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        
        RaycastHit2D ray_hit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rigid.velocity.y < 0)
        {
            if (ray_hit.collider != null)
            {
                if (ray_hit.distance < 0.5f)
                {
                    is_jump = false;
                    anim.SetBool("is_jumping", false);
                }
            }
        }
    }

    void Move()
    {
        // 이동 방향에 따라 플레이어 방향 바꾸기
        sprite_renderer.flipX = Input.GetAxisRaw("Horizontal") == -1;


        // 멈췄을 때 속도 줄이기
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // 멈췄을 때 애니매이션 정지하기
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("is_walking", false);

        else
            anim.SetBool("is_walking", true);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !is_jump)
        {
            SoundManager.instance.PlaySound("Jump");

            is_jump = true;
            jump_time = 0f;

            rigid.velocity = new Vector2(rigid.velocity.x, jump_power);
            anim.SetBool("is_jumping", true);
        }
    }

    public void OnDamaged(Vector2 target_pos) // 플레이어가 데미지를 입었을 때 (무적 상태로 만드는 함수)
    {
        if (gameObject.layer == 7)
        {
            return;
        }

        SoundManager.instance.PlaySound("Damaged");

        if (life > 1)
        {
            life -= 1;

            game_manager.UpdateLifeIcon(life);

            // 무적 판정
            gameObject.layer = 7;                                        // gameObject의 Layer 중 7번째 Layer(PlayerDamaged)로 바꿔줌

            sprite_renderer.color = new Color(1, 1, 1, 0.4f);            // 색상을 바꿔줌
                                                                         // 매개변수 -> 순서대로 R, G, B, 투명도를 뜻함

            int dirc = transform.position.x - target_pos.x > 0 ? 1 : -1; // 플레이어의 x좌표 -  맞은 적의 x좌표가 0보다 크면 1을, 아니면 -1을 dirc변수에 대입함 (삼항연산자)
            rigid.AddForce(new Vector2(dirc, 1) * 4.5f, ForceMode2D.Impulse); // 위에서 대입한 방향대로 튕겨 나감

            Invoke("OffDamaged", 1.25f);                                  // 1.25초가 지나면 무적 상태가 해제됨
        }

        else
        {
            game_manager.GameOver("Die");
        }
    }

    void OffDamaged() // 무적 상태를 해제하는 함수
    {
        gameObject.layer = 6;                          // gameObject의 Layer 중 6번째 Layer(Player)로 바꿔줌 -> 원래대로 돌려놓음

        sprite_renderer.color = new Color(1, 1, 1, 1); // 색상을 원래대로 돌려놓음
    }

    public void VelocityZero() // 플레이어의 낙하 속도를 0으로 만드는 함수
    {
        rigid.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Flag")
        {
            game_manager.NextStage();

            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.tag == "Slow")
        {
            SoundManager.instance.PlaySound("Slow Platform");

            speed *= 0.5f;
        }

        else if (collision.gameObject.tag == "Fast")
        {
            if (!is_fast)
            {
                SoundManager.instance.PlaySound("Fast Platform");

                is_fast = true;

                speed *= 2.5f;
            }
        }

        else if (collision.gameObject.tag == "Higher")
        {
            if (!is_high)
            {
                SoundManager.instance.PlaySound("Higher Platform");

                is_high = true;

                jump_power *= 1.5f;
            }
        }
    }
}