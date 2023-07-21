using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KoreanTyper; // 타이핑 하는 네임 스페이스 추가

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject map;
    public GameObject hearts;
    public GameObject room;

    // 스토리 관련 변수
    public Text story_text;
    public bool is_story;

    // 타이핑 관련 변수
    public float next_typing;
    public bool is_typing;

    // 문제 관련 변수
    public Text quiz_text;
    public string[] quizs;
    public bool[] answer_o; // O 버튼을 눌렀을 때 정답인 문제 배열
    public bool[] answer_x; // X 버튼을 눌렀을 때 정답인 문제 배열

    // 버튼 관련 변수
    public GameObject exam_btn;
    public GameObject o_btn;
    public GameObject x_btn;

    // 퀴즈 관련 변수
    public bool is_quiz;
    public bool is_correct;
    int quiz_index;
    bool is_wrong;

    int wrong_answer;
    int right_answer;
    public GameObject result_text_obj;
    public Text result_text;

    // 게임 관련 변수
    public bool is_fall;
    public Image[] life_images; // 플레이어의 생명 UI 배열
    public GameObject main_camera;
    public GameObject die_group;
    public GameObject clear_group;
    public Image option_group;
    bool is_option;

    // 초기화 시켜야 하는 변수
    Player player_logic;
    AudioSource bgm_player;

    // 스테이지 관련 변수
    public GameObject[] stages;
    public int stage_index; 

    void Awake() // 시작하자 마자
    {
        // 여러 오브젝트 비활성화 (스토리 텍스트를 보여주기 위함)
        map.SetActive(false);
        player.SetActive(false);
        hearts.SetActive(false);

        // 초기화
        is_story = true;
        is_quiz = false;
        stage_index = 0;

        right_answer = 0;
        wrong_answer = 0;

        result_text.text = "";

        player_logic = player.GetComponent<Player>();
        bgm_player = GameObject.Find("Bgm Player").GetComponent<AudioSource>();

        // 스토리를 타이핑 하는 코루틴 함수 실행
        StartCoroutine(TypingCoroutine("숲 속에서 모험을 시작하게 된 우리의 김과학은 과학 마스터가 되기 위해서\n오늘도 머릿속에서 떠오르는 여러 이론들을 생각하며\n중력과 역학적 시스템을 공부하게 되는데...", 0.05f, story_text, 4, 0.4f, false));

        // 문제의 개수만큼 배열 생성
        answer_o = new bool[quizs.Length];
        answer_x = new bool[quizs.Length];

        // 정답 배열 값 할당
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

    void Update() // 매 프레임마다
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

    void FixedUpdate() // 일정한 간격마다
    {
        // 카메라가 플레이어를 따라다니게 함
        main_camera.transform.position = player.transform.position + new Vector3(0, 0, -10);

        // 스토리 타이핑 중일 때는 비활성화, 스토리 타이핑이 끝나면 활성화 시킴
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

        if (!is_quiz) // 문제 출제 중이 아니면
        {
            quiz_text.text = ""; // 문제 텍스트 초기화
        }

        ShowStage();

        result_text.text = string.Format("정답 : {0}\t오답 : {1}", right_answer, wrong_answer);
    }

    // 플레이어가 떨어졌는지 확인하는 함수
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

    public void MakeExam() // 문제를 출제하는 함수
    {
        SoundManager.instance.PlaySound("Button");

        StopAllCoroutines();

        is_quiz = true;

        // 문제 출제하기
        quiz_index = Random.Range(0, quizs.Length);                                       // 랜덤으로 문제 인덱스를 설정하고
        StartCoroutine(TypingCoroutine(quizs[quiz_index], 0.065f, quiz_text, 0, 0, true)); // 해당 인덱스의 문제 타이핑

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
            case 0: // O 버튼
                is_correct = answer_o[quiz_index];
                break;

            case 1: // X 버튼
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
            Debug.Log("정답! " + player_logic.life);
        }

        else if (!is_correct)
        {
            SoundManager.instance.PlaySound("Wrong");

            StopAllCoroutines();

            is_wrong = true;

            player_logic.OnDamaged(player.transform.position);
            wrong_answer++;
            UpdateLifeIcon(player_logic.life);

            Debug.Log("오답! " + player_logic.life);
        }

        is_quiz = !is_quiz;
    }

    void ReCorrect()
    {
        is_correct = !is_correct;
    }

    public void UpdateLifeIcon(int life) // 플레이어의 남은 생명 아이콘을 업데이트 해주는 함수  /   매개변수 -> 플레이어의 생명
    {
        // 모든 생명 UI를 투명한 색으로 만든 다음
        for (int index = 0; index < life_images.Length; index++) // 최대 생명의 수 만큼 반복
        {
            life_images[index].color = new Color(1, 1, 1, 0);
        }

        // 남은 목숨의 개수대로 반투명으로 바꿔줌
        for (int index = 0; index < life; index++) // 현재 가지고 있는 생명의 수 만큼 반복
        {
            // Image 변수이기 때문에 SetActive()를 사용하지 않고 색상을 다르게 바꿔줌
            life_images[index].color = new Color(1, 1, 1, 1);
        }
    }

    // 타이핑 애니매이션을 쓰기 위한 함수 (코루틴 함수)
    public IEnumerator TypingCoroutine(string str, float next_typing, Text text, int slow_down, float text_slow, bool is_quiz_text) // 파라미터 -> 타이핑할 텍스트, 다음 타이핑까지 기다릴 시간, 타이핑할 텍스트의 UI, 타이핑을 느리게 할 지점, 타이핑을 느리게 하는 정도, 이 텍스트가 퀴즈 텍스트 인지 여부
    {
        text.text = "";                                   // 타이핑할 텍스트 초기화
        is_typing = true;                                 // 타이핑을 한다는 사실을 알려줌
        yield return new WaitForSeconds(1f);              // 1초 대기

        int strTypingLength = str.GetTypingLength();      // 최대 타이핑 수를 구하고

        for (int i = 0; i <= strTypingLength; i++)        // 최대 타이핑 수 만큼 반복
        {
            text.text = str.Typing(i);                    // 텍스트 타이핑
            SoundManager.instance.PlaySound("Text");      // 타이핑 소리 출력

            if (i > strTypingLength - slow_down)          // 타이핑을 느리게 할 지점까지 출력했을 때
            {
                next_typing = text_slow;                  // 기다리는 시간을 늘려줌
            }

            yield return new WaitForSeconds(next_typing); // 임의로 정한 시간 만큼 기다림
        }

        // 타이핑이 끝났을 때
        if (!is_quiz_text) // 퀴즈 텍스트가 아니였다면
        {
            // 좀 기다렸다가 텍스트를 초기화함
            yield return new WaitForSeconds(0.5f);
            is_typing = false;
            text.text = "";

            if (is_story)
            {
                is_story = false;
            }

        }

        else // 퀴즈 텍스트였다면
        {
            // 그냥 타이핑이 끝났다고만 알려줌
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

    public void NextStage() // 스테이지를 전환시키는 함수
    {
        if (stage_index < stages.Length - 1) // 현재 스테이지 번호가 맵의 개수보다 작을 때
        {
            SoundManager.instance.PlaySound("Clear");

            stage_index++;                   // 스테이지의 번호를 1 더한뒤

            PlayerReposition();              // 플레이어를 원점으로 되돌림

            switch (stage_index)             // 스테이지에 따른 배경음 변경
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

    void PlayerReposition() // 플레이어의 위치를 원점으로 되돌리는 함수
    {
        player_logic.VelocityZero();                          // 낙하 속도를 0으로 만들고
        player.transform.position = new Vector3(0, -1.5f, 0); // 플레이어의 위치를 원점으로 되돌림
    }

    public void GameOver(string situ) // 상황에 따라 죽었을 때 실행하는 코드가 달라지게 함
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
                Debug.Log("죽음!");
                break;

            case "Clear":
                ChangeBgm(3);

                is_story = true;
                room.SetActive(true);

                quiz_text.text = "";
                Debug.Log("클리어!");

                StartCoroutine(TypingCoroutine("우여곡절 끝에 집에 도착한 우리의 김과학은\n오늘 경험한 신비한 일들을 그저 \"꿈\"이라고 생각하며\n오늘을 다시 기억하는 날은 오지 않았습니다...", 0.05f, story_text, 4, 0.4f, false));

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
