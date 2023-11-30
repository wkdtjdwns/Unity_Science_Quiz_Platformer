using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExperimentButtonManager : GameManager
{
    ExperimentGameManager game_manager;
    Guide guide;
    Animator goods_anim;
    Animator solutions_anim;
    Image image;

    [SerializeField]
    private Sprite normal_sprite;

    [SerializeField]
    private Sprite[] goods_sprites;
    [SerializeField]
    private Sprite[] btb_results;
    [SerializeField]
    private Sprite[] methyl_results;
    [SerializeField]
    private Sprite[] phenol_results;

    [SerializeField]
    private Button acid_button;
    [SerializeField]
    private Button neutrality_button;
    [SerializeField]
    private Button basic_button;

    [SerializeField]
    private Text question_text;

    [SerializeField]
    private bool is_water;
    [SerializeField]
    private bool is_vinegar;
    [SerializeField]
    private bool is_orange_juice;
    [SerializeField]
    private bool is_baking_soda;
    [SerializeField]
    private bool is_sparkling_water;

    private void Awake()
    {
        is_water = false;
        is_vinegar = false;
        is_orange_juice = false;
        is_baking_soda = false;
        is_sparkling_water = false;

        game_manager = GameObject.Find("GameManager").gameObject.GetComponent<ExperimentGameManager>();
        guide = GameObject.Find("Guide").gameObject.GetComponent<Guide>();
        goods_anim = game_manager.need_goods_obj.GetComponent<Animator>();
        solutions_anim = game_manager.need_solutions_obj.GetComponent<Animator>();
        image = game_manager.background.GetComponent<Image>();
    }

    public void BackHome() { SoundManager.instance.PlayBgm("home"); SceneManager.LoadScene("HomeScene"); }

    public void SoundHear() { SoundManager.instance.PlaySound("sound hear"); }

    public void UseGoods(string type)
    {
        switch (type)
        {
            case "water":
                if (water_num > 0)
                {
                    is_water = true;
                    game_manager.is_goods = true;

                    water_num--;
                    PutGoods(type);
                }

                else { StartCoroutine(NeedAnim("goods")); }
                break;

            case "vinegar":
                if (vinegar_num > 0)
                {
                    is_vinegar = true;
                    game_manager.is_goods = true;

                    vinegar_num--;
                    PutGoods(type);
                }

                else { StartCoroutine(NeedAnim("goods")); }
                break;

            case "orange juice":
                if (orange_juice_num > 0)
                {
                    is_orange_juice = true;
                    game_manager.is_goods = true;

                    orange_juice_num--;
                    PutGoods(type);
                }

                else { StartCoroutine(NeedAnim("goods")); }
                break;

            case "baking soda":
                if (baking_soda_num > 0)
                {
                    is_baking_soda = true;
                    game_manager.is_goods = true;

                    baking_soda_num--;
                    PutGoods(type);
                }

                else { StartCoroutine(NeedAnim("goods")); }
                break;

            case "sparkling water":
                if (sparkling_water_num > 0)
                {
                    is_sparkling_water = true;
                    game_manager.is_goods = true;

                    sparkling_water_num--;
                    PutGoods(type);
                }

                else { StartCoroutine(NeedAnim("goods")); }
                break;
        }
    }

    public void UseSolutions(string type)
    {
        switch (type)
        {
            case "btb":
                if (btb_num > 0)
                {
                    btb_num--;
                    solution_num--;
                    PutSolutions(type);
                }

                else { StartCoroutine(NeedAnim("solutions")); }
                break;

            case "methyl":
                if (methyl_num > 0)
                {
                    methyl_num--;
                    solution_num--;
                    PutSolutions(type);
                }

                else { StartCoroutine(NeedAnim("solutions")); }
                break;

            case "phenol":
                if (phenol_num > 0)
                {
                    phenol_num--;
                    solution_num--;
                    PutSolutions(type);
                }

                else { StartCoroutine(NeedAnim("solutions")); }
                break;
        }
    }

    private IEnumerator NeedAnim(string type)
    {
        SoundManager.instance.PlaySound("need");

        switch (type)
        {
            case "goods":
                goods_anim.SetBool("is_need", true);

                yield return new WaitForSeconds(0.25f);

                goods_anim.SetBool("is_need", false);
                break;

            case "solutions":

                solutions_anim.SetBool("is_need", true);

                yield return new WaitForSeconds(0.25f);

                solutions_anim.SetBool("is_need", false);
                break;
        }
    }

    private void PutGoods(string type)
    {
        SoundManager.instance.PlaySound("put goods");

        switch (type)
        {
            case "water":
                image.sprite = goods_sprites[0];
                break;

            case "vinegar":
                image.sprite = goods_sprites[1];
                break;

            case "orange juice":
                image.sprite = goods_sprites[2];
                break;

            case "baking soda":
                image.sprite = goods_sprites[3];
                break;

            case "sparkling water":
                image.sprite = goods_sprites[4];
                break;
        }
    }

    private void PutSolutions(string type)
    {
        SoundManager.instance.PlaySound("put solution");

        // 염기성 :베이킹 소다 -> result[0]
        // 중성 : 물 -> result[1]
        // 산성 : 식초, 오렌지 주스. 탄산수 -> result[2]

        switch (type)
        {
            case "btb":
                if (is_baking_soda) { image.sprite = btb_results[0]; }
                else if (is_water) { image.sprite = btb_results[1]; }
                else { image.sprite = btb_results[2]; }
                guide.is_btb = true;
                break;

            case "methyl":
                if (is_baking_soda) { image.sprite = methyl_results[0]; }
                else if (is_water) { image.sprite = methyl_results[1]; }
                else { image.sprite = methyl_results[2]; }
                guide.is_methyl = true;
                break;

            case "phenol":
                if (is_baking_soda) { image.sprite = phenol_results[0]; }
                else if (is_water) { image.sprite = phenol_results[1]; }
                else { image.sprite = phenol_results[2]; }
                break;
        }

        game_manager.is_solution = true;

        AskQuestion();
    }

    private void AskQuestion()
    {
        Invoke("PlayMixSolutionSound", 0.25f);

        game_manager.question_obj.SetActive(true);

        acid_button.interactable = true;
        basic_button.interactable = true;
        neutrality_button.interactable = true;

        if (is_water) { question_text.text = "물의 성질은?"; }
        else if (is_vinegar) { question_text.text = "식초의 성질은?"; }
        else if (is_orange_juice) { question_text.text = "오렌지 주스의 성질은?"; }
        else if (is_baking_soda) { question_text.text = "베이킹 소다의 성질은?"; }
        else if (is_sparkling_water) { question_text.text = "탄산수의 성질은?"; }
    }

    private void PlayMixSolutionSound() { SoundManager.instance.PlaySound("mix solution"); }

    public void Answer(string type)
    {
        if (is_vinegar || is_orange_juice || is_sparkling_water)
        {
            if (type.Equals("acid")) { game_manager.is_correct = true; }
            else if (type.Equals("basic")) { IsWrong(basic_button); }
            else { IsWrong(neutrality_button); }
        }

        else if (is_water)
        {
            if (type.Equals("neutrality")) { game_manager.is_correct = true; }
            else if (type.Equals("acid")) { IsWrong(acid_button); }
            else { IsWrong(basic_button); }
        }

        else if (is_baking_soda)
        {
            if (type.Equals("basic")) { game_manager.is_correct = true; }
            else if (type.Equals("acid")) { IsWrong(acid_button); }
            else { IsWrong(neutrality_button); }
        }

        CheckAnswer();
    }

    private void IsWrong(Button bnt)
    {
        bnt.interactable = false;
        game_manager.is_wrong = true;
    }

    private void CheckAnswer()
    {
        if (game_manager.is_correct)
        {
            SoundManager.instance.PlaySound("correct");

            game_manager.correct_obj.SetActive(true);
            Invoke("OffCorrectObj", 0.5f);

            guide.Unlock();

            is_water = false;
            is_vinegar = false;
            is_orange_juice = false;
            is_baking_soda = false;
            is_sparkling_water = false;

            guide.is_btb = false;
            guide.is_methyl = false;
        }

        else
        {
            SoundManager.instance.PlaySound("wrong");

            game_manager.wrong_obj.SetActive(true);
            Invoke("OffWrongObj", 0.5f);
            game_manager.is_wrong = false;
        }
    }

    private void OffCorrectObj() { game_manager.correct_obj.SetActive(false); image.sprite = normal_sprite; }

    private void OffWrongObj() { game_manager.wrong_obj.SetActive(false); }
}
