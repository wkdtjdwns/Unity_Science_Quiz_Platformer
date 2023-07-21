using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KoreanTyper; // Ÿ���� �ϴ� ���� �����̽� �߰�

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject map;
    public GameObject hearts;
    public GameObject room;

    // ���丮 ���� ����
    public Text story_text;
    public bool is_story;

    // Ÿ���� ���� ����
    public float next_typing;
    public bool is_typing;

    // ���� ���� ����
    public Text quiz_text;
    public string[] quizs;
    public bool[] answer_o; // O ��ư�� ������ �� ������ ���� �迭
    public bool[] answer_x; // X ��ư�� ������ �� ������ ���� �迭

    // ��ư ���� ����
    public GameObject exam_btn;
    public GameObject o_btn;
    public GameObject x_btn;

    // ���� ���� ����
    public bool is_quiz;
    public bool is_correct;
    int quiz_index;
    bool is_wrong;

    int wrong_answer;
    int right_answer;
    public GameObject result_text_obj;
    public Text result_text;

    // ���� ���� ����
    public bool is_fall;
    public Image[] life_images; // �÷��̾��� ���� UI �迭
    public GameObject main_camera;
    public GameObject die_group;
    public GameObject clear_group;
    public Image option_group;
    bool is_option;

    // �ʱ�ȭ ���Ѿ� �ϴ� ����
    Player player_logic;
    AudioSource bgm_player;

    // �������� ���� ����
    public GameObject[] stages;
    public int stage_index; 

    void Awake() // �������� ����
    {
        // ���� ������Ʈ ��Ȱ��ȭ (���丮 �ؽ�Ʈ�� �����ֱ� ����)
        map.SetActive(false);
        player.SetActive(false);
        hearts.SetActive(false);

        // �ʱ�ȭ
        is_story = true;
        is_quiz = false;
        stage_index = 0;

        right_answer = 0;
        wrong_answer = 0;

        result_text.text = "";

        player_logic = player.GetComponent<Player>();
        bgm_player = GameObject.Find("Bgm Player").GetComponent<AudioSource>();

        // ���丮�� Ÿ���� �ϴ� �ڷ�ƾ �Լ� ����
        StartCoroutine(TypingCoroutine("�� �ӿ��� ������ �����ϰ� �� �츮�� ������� ���� �����Ͱ� �Ǳ� ���ؼ�\n���õ� �Ӹ��ӿ��� �������� ���� �̷е��� �����ϸ�\n�߷°� ������ �ý����� �����ϰ� �Ǵµ�...", 0.05f, story_text, 4, 0.4f, false));

        // ������ ������ŭ �迭 ����
        answer_o = new bool[quizs.Length];
        answer_x = new bool[quizs.Length];

        // ���� �迭 �� �Ҵ�
        for (int index = 0; index < quizs.Length; index++)
        {
            if (index % 2 == 0) 
            {
                answer_o[index] = true;
            }

            else
            {
                answer_x[index] = true;
            }
        }
    }

    void Start()
    {
        ChangeBgm(0);
    }

    void Update() // �� �����Ӹ���
    {
        CheckFall();

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextStage();
        }

        else if (Input.GetButtonDown("Cancel"))
        {
            Option();
        }
    }

    public void Option()
    {
        SoundManager.instance.PlaySound("Option");

        is_option = !is_option;

        option_group.gameObject.SetActive(is_option);

        Time.timeScale = is_option == true ? 0 : 1; 
    }

    void FixedUpdate() // ������ ���ݸ���
    {
        // ī�޶� �÷��̾ ����ٴϰ� ��
        main_camera.transform.position = player.transform.position + new Vector3(0, 0, -10);

        // ���丮 Ÿ���� ���� ���� ��Ȱ��ȭ, ���丮 Ÿ������ ������ Ȱ��ȭ ��Ŵ
        map.SetActive(!is_story);
        player.SetActive(!is_story);
        hearts.SetActive(!is_story);

        exam_btn.SetActive(!is_story);

        if (!is_story)
        {
            exam_btn.SetActive(!is_quiz);
        }

        o_btn.SetActive(is_quiz);
        x_btn.SetActive(is_quiz);

        if (!is_quiz) // ���� ���� ���� �ƴϸ�
        {
            quiz_text.text = ""; // ���� �ؽ�Ʈ �ʱ�ȭ
        }

        ShowStage();

        result_text.text = string.Format("���� : {0}\t���� : {1}", right_answer, wrong_answer);
    }

    // �÷��̾ ���������� Ȯ���ϴ� �Լ�
    public bool CheckFall()
    {
        Vector3 pos = player.transform.position;

        if (pos.y < -7.5f)
        {
            is_fall = true;
        }
         

        else
        {
            is_fall = false;
        }

        return is_fall;
    }

    public void MakeExam() // ������ �����ϴ� �Լ�
    {
        SoundManager.instance.PlaySound("Button");

        StopAllCoroutines();

        is_quiz = true;

        // ���� �����ϱ�
        quiz_index = Random.Range(0, quizs.Length);                                       // �������� ���� �ε����� �����ϰ�
        StartCoroutine(TypingCoroutine(quizs[quiz_index], 0.065f, quiz_text, 0, 0, true)); // �ش� �ε����� ���� Ÿ����

        if (is_correct)
        {
            is_quiz = !is_quiz;

            is_correct = !is_correct;
        }

        else if (is_wrong)
        {
            is_quiz = !is_quiz;

            is_wrong = !is_wrong;
        }
    }

    public void CheckAnswer(int ans_btn)
    {
        switch (ans_btn)
        {
            case 0: // O ��ư
                is_correct = answer_o[quiz_index];
                break;

            case 1: // X ��ư
                is_correct = answer_x[quiz_index];
                break;
        }

        if (is_correct)
        {
            SoundManager.instance.PlaySound("Correct");

            StopAllCoroutines();

            if (player_logic.life < 3)
            {
                player_logic.life += 1;
                UpdateLifeIcon(player_logic.life);
            }

            player_logic.speed = 5;
            player_logic.jump_power = 5.5f;

            player_logic.is_fast = false;
            player_logic.is_high = false;

            right_answer++;
            Invoke("ReCorrect", 0.5f);
            Debug.Log("����! " + player_logic.life);
        }

        else if (!is_correct)
        {
            SoundManager.instance.PlaySound("Wrong");

            StopAllCoroutines();

            is_wrong = true;

            player_logic.OnDamaged(player.transform.position);
            wrong_answer++;
            UpdateLifeIcon(player_logic.life);

            Debug.Log("����! " + player_logic.life);
        }

        is_quiz = !is_quiz;
    }

    void ReCorrect()
    {
        is_correct = !is_correct;
    }

    public void UpdateLifeIcon(int life) // �÷��̾��� ���� ���� �������� ������Ʈ ���ִ� �Լ�  /   �Ű����� -> �÷��̾��� ����
    {
        // ��� ���� UI�� ������ ������ ���� ����
        for (int index = 0; index < life_images.Length; index++) // �ִ� ������ �� ��ŭ �ݺ�
        {
            life_images[index].color = new Color(1, 1, 1, 0);
        }

        // ���� ����� ������� ���������� �ٲ���
        for (int index = 0; index < life; index++) // ���� ������ �ִ� ������ �� ��ŭ �ݺ�
        {
            // Image �����̱� ������ SetActive()�� ������� �ʰ� ������ �ٸ��� �ٲ���
            life_images[index].color = new Color(1, 1, 1, 1);
        }
    }

    // Ÿ���� �ִϸ��̼��� ���� ���� �Լ� (�ڷ�ƾ �Լ�)
    public IEnumerator TypingCoroutine(string str, float next_typing, Text text, int slow_down, float text_slow, bool is_quiz_text) // �Ķ���� -> Ÿ������ �ؽ�Ʈ, ���� Ÿ���α��� ��ٸ� �ð�, Ÿ������ �ؽ�Ʈ�� UI, Ÿ������ ������ �� ����, Ÿ������ ������ �ϴ� ����, �� �ؽ�Ʈ�� ���� �ؽ�Ʈ ���� ����
    {
        text.text = "";                                   // Ÿ������ �ؽ�Ʈ �ʱ�ȭ
        is_typing = true;                                 // Ÿ������ �Ѵٴ� ����� �˷���
        yield return new WaitForSeconds(1f);              // 1�� ���

        int strTypingLength = str.GetTypingLength();      // �ִ� Ÿ���� ���� ���ϰ�

        for (int i = 0; i <= strTypingLength; i++)        // �ִ� Ÿ���� �� ��ŭ �ݺ�
        {
            text.text = str.Typing(i);                    // �ؽ�Ʈ Ÿ����
            SoundManager.instance.PlaySound("Text");      // Ÿ���� �Ҹ� ���

            if (i > strTypingLength - slow_down)          // Ÿ������ ������ �� �������� ������� ��
            {
                next_typing = text_slow;                  // ��ٸ��� �ð��� �÷���
            }

            yield return new WaitForSeconds(next_typing); // ���Ƿ� ���� �ð� ��ŭ ��ٸ�
        }

        // Ÿ������ ������ ��
        if (!is_quiz_text) // ���� �ؽ�Ʈ�� �ƴϿ��ٸ�
        {
            // �� ��ٷȴٰ� �ؽ�Ʈ�� �ʱ�ȭ��
            yield return new WaitForSeconds(0.5f);
            is_typing = false;
            text.text = "";

            if (is_story)
            {
                is_story = false;
            }

        }

        else // ���� �ؽ�Ʈ���ٸ�
        {
            // �׳� Ÿ������ �����ٰ� �˷���
            is_typing = false;
        }
    }

    void ShowStage()
    {
        for (int index = 0; index < stages.Length; index++)
        {
            if (stage_index == index)
            {
                stages[index].SetActive(true);
            }

            else
            {
                stages[index].SetActive(false);
            }
        }
    }

    public void NextStage() // ���������� ��ȯ��Ű�� �Լ�
    {
        if (stage_index < stages.Length - 1) // ���� �������� ��ȣ�� ���� �������� ���� ��
        {
            SoundManager.instance.PlaySound("Clear");

            stage_index++;                   // ���������� ��ȣ�� 1 ���ѵ�

            PlayerReposition();              // �÷��̾ �������� �ǵ���

            switch (stage_index)             // ���������� ���� ����� ����
            {
                case 1:
                    ChangeBgm(1);
                    break;

                case 2:
                case 3:
                    ChangeBgm(2);
                    break;
            }
        }

        else
        {
            GameOver("Clear");
        }
    }

    void PlayerReposition() // �÷��̾��� ��ġ�� �������� �ǵ����� �Լ�
    {
        player_logic.VelocityZero();                          // ���� �ӵ��� 0���� �����
        player.transform.position = new Vector3(0, -1.5f, 0); // �÷��̾��� ��ġ�� �������� �ǵ���
    }

    public void GameOver(string situ) // ��Ȳ�� ���� �׾��� �� �����ϴ� �ڵ尡 �޶����� ��
    {
        switch (situ)
        {
            case "Die":
                SoundManager.instance.PlaySound("Game Over");

                Time.timeScale = 0;

                map.SetActive(false);
                player.SetActive(false);
                hearts.SetActive(false);

                exam_btn.SetActive(false);
                o_btn.SetActive(false);
                x_btn.SetActive(false);

                quiz_text.text = "";
                AnswerCheck();

                die_group.SetActive(true);
                Debug.Log("����!");
                break;

            case "Clear":
                ChangeBgm(3);

                is_story = true;
                room.SetActive(true);

                quiz_text.text = "";
                Debug.Log("Ŭ����!");

                StartCoroutine(TypingCoroutine("�쿩���� ���� ���� ������ �츮�� �������\n���� ������ �ź��� �ϵ��� ���� \"��\"�̶�� �����ϸ�\n������ �ٽ� ����ϴ� ���� ���� �ʾҽ��ϴ�...", 0.05f, story_text, 4, 0.4f, false));

                Invoke("GameClear", 15);
                Invoke("AnswerCheck", 15);
                break;
        }
    }

    void GameClear()
    {
        Time.timeScale = 0;

        clear_group.SetActive(true);
    }

    void AnswerCheck()
    {
        result_text_obj.SetActive(true);
    }

    public void Retry()
    {
        die_group.SetActive(false);
        clear_group.SetActive(false);

        Time.timeScale = 1;

        SceneManager.LoadScene(0);
    }

    public void ChangeBgm(int bgm_index)
    {
        bgm_player.clip = SoundManager.instance.bgm_clips[bgm_index];
        bgm_player.Play();
    }

    public void Exit()
    {
        SoundManager.instance.PlaySound("Quit");

        Application.Quit();
    }
}
