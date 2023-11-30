using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentGameManager : GameManager
{
    [SerializeField]
    private GameObject goods_group;
    [SerializeField]
    private GameObject solutions_group;
    [SerializeField]
    private GameObject three_days_obj;

    public GameObject background;
    public GameObject need_goods_obj;
    public GameObject need_solutions_obj;
    public GameObject question_obj;
    public GameObject correct_obj;
    public GameObject wrong_obj;

    [SerializeField]
    private Text remaining_water_text;
    [SerializeField]
    private Text remaining_vinegar_text;
    [SerializeField]
    private Text remaining_orange_juice_text;
    [SerializeField]
    private Text remaining_baking_soda_text;
    [SerializeField]
    private Text remaining_sparkling_water_text;

    [SerializeField]
    private Text remaining_btb_text;
    [SerializeField]
    private Text remaining_methyl_text;
    [SerializeField]
    private Text remaining_phenol_text;

    public bool is_goods;
    public bool is_solution;

    public bool is_correct;
    public bool is_wrong;

    private void Update()
    {
        SetGroup();
        SetRemainingNumber();

        // 이스터 에그
        EasterEgg();
    }

    private void SetRemainingNumber()
    {
        remaining_water_text.text = string.Format("{0}개", water_num);
        remaining_vinegar_text.text = string.Format("{0}개", vinegar_num);
        remaining_orange_juice_text.text = string.Format("{0}개", orange_juice_num);
        remaining_baking_soda_text.text = string.Format("{0}개", baking_soda_num);
        remaining_sparkling_water_text.text = string.Format("{0}개", sparkling_water_num);

        remaining_btb_text.text = string.Format("{0}개", btb_num);
        remaining_methyl_text.text = string.Format("{0}개", methyl_num);
        remaining_phenol_text.text = string.Format("{0}개", phenol_num);
    }

    private void SetGroup()
    {
        if (!is_goods && !is_solution)
        {
            goods_group.SetActive(true);
            solutions_group.SetActive(false);
        }

        else if (is_goods && !is_solution)
        {
            goods_group.SetActive(false);
            solutions_group.SetActive(true);
        }

        else if (is_correct)
        {
            Invoke("IsCorrect", 0.5f);

            is_goods = false;
            is_solution = false;

            is_correct = false;
        }
    }

    private void IsCorrect() { question_obj.SetActive(false); }

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
