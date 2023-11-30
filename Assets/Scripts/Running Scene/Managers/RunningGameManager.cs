using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RunningGameManager : GameManager
{
    public float run_distance;
    public int solution_cnt;

    [SerializeField]
    private GameObject result_obj;
    [SerializeField]
    private GameObject three_days_obj;
    [SerializeField]
    private Text solution_result_text;
    [SerializeField]
    private Text distance_result_text;

    public bool is_run;
    public bool is_tired;

    private void OnEnable()
    {
        run_distance = 0;
        solution_cnt = 0;
    }
    private void OnDisable()
    {
        Time.timeScale = 1;

        run_distance = 0;
        solution_cnt = 0;
    }

    private void Update()
    {
        if (!is_tired) { run_distance += Time.deltaTime; }

        CheckDie();

        CheatKey();

        // 이스터 에그
        EasterEgg();
    }

    private void CheckDie()
    {
        if (is_tired)
        {
            is_tired = false;
            is_run = false;

            Time.timeScale = 0;

            solution_result_text.text = string.Format("{0}개", solution_cnt);
            distance_result_text.text = string.Format("{0}m", Mathf.Round(run_distance));

            result_obj.SetActive(true);
        }
    }

    private void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F2)) { run_distance += 100f; }
    }

    // 이스터 에그
    private void EasterEgg()
    {
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            three_days_obj.SetActive(true);

            Animator three_anim = three_days_obj.GetComponent<Animator>();
            three_anim.SetBool("is_three", true);

            StartCoroutine("PlayEasterSound");

            Invoke("OffEasterThree", 3.33f);
        }
    }

    private IEnumerator PlayEasterSound()
    {
        yield return new WaitForSeconds(0.5f);

        SoundManager.instance.PlaySound("easter 3");
    }

    private void OffEasterThree()
    {
        Animator three_anim = three_days_obj.GetComponent<Animator>();
        three_anim.SetBool("is_three", false);

        three_days_obj.SetActive(false);
    }
}
