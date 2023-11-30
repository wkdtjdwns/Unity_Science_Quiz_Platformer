using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // �÷��̾ ���� ����
    public float speed;
    public float jump_power;
    public float flip_ratio = 1f;
    public int life;

    public bool is_jump;
    public float jump_time;

    public bool is_fast;
    public bool is_high;

    // GameManager�� ���� ����
    public GameObject game_manager_obj;
    public GameManager game_manager;

    // �ʱ�ȭ ���Ѿ� �ϴ� ����
    Rigidbody2D rigid;
    SpriteRenderer sprite_renderer;
    Animator anim;

    void Awake()
    {
        // �ʱ�ȭ
        rigid = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        game_manager_obj = GameObject.Find("GameManager").gameObject;
        game_manager = game_manager_obj.GetComponent<GameManager>();

        // z�� ����
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

    void FixedUpdate() // ������ ���ݸ���
    {
        // �¿� �̵�
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // �ӵ��� �ִ� �ӷ��� ���� ���ϰ� ��
        if (rigid.velocity.x > speed)
        {
            rigid.velocity = new Vector2(speed, rigid.velocity.y);
        }

        else if (rigid.velocity.x < speed * (-1))
        {
            rigid.velocity = new Vector2(speed * (-1), rigid.velocity.y);
        }

        // �÷��̾ ���� ���� �� ����
        if (jump_time > 2.5f)
        {
            is_jump = false;
            anim.SetBool("is_jumping", false);
        }

        // ���� ���� ����
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
        // �̵� ���⿡ ���� �÷��̾� ���� �ٲٱ�
        sprite_renderer.flipX = Input.GetAxisRaw("Horizontal") == -1;


        // ������ �� �ӵ� ���̱�
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // ������ �� �ִϸ��̼� �����ϱ�
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

    public void OnDamaged(Vector2 target_pos) // �÷��̾ �������� �Ծ��� �� (���� ���·� ����� �Լ�)
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

            // ���� ����
            gameObject.layer = 7;                                        // gameObject�� Layer �� 7��° Layer(PlayerDamaged)�� �ٲ���

            sprite_renderer.color = new Color(1, 1, 1, 0.4f);            // ������ �ٲ���
                                                                         // �Ű����� -> ������� R, G, B, ������ ����

            int dirc = transform.position.x - target_pos.x > 0 ? 1 : -1; // �÷��̾��� x��ǥ -  ���� ���� x��ǥ�� 0���� ũ�� 1��, �ƴϸ� -1�� dirc������ ������ (���׿�����)
            rigid.AddForce(new Vector2(dirc, 1) * 4.5f, ForceMode2D.Impulse); // ������ ������ ������ ƨ�� ����

            Invoke("OffDamaged", 1.25f);                                  // 1.25�ʰ� ������ ���� ���°� ������
        }

        else
        {
            game_manager.GameOver("Die");
        }
    }

    void OffDamaged() // ���� ���¸� �����ϴ� �Լ�
    {
        gameObject.layer = 6;                          // gameObject�� Layer �� 6��° Layer(Player)�� �ٲ��� -> ������� ��������

        sprite_renderer.color = new Color(1, 1, 1, 1); // ������ ������� ��������
    }

    public void VelocityZero() // �÷��̾��� ���� �ӵ��� 0���� ����� �Լ�
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