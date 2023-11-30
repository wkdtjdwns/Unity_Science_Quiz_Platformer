using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeGameManager : GameManager
{
    [SerializeField]
    private Text gold_text;
    [SerializeField]
    private Text solution_text;

    [SerializeField]
    private Text add_health_price_text;
    [SerializeField]
    private Text water_price_text;
    [SerializeField]
    private Text vinegar_price_text;
    [SerializeField]
    private Text orange_juice_text;
    [SerializeField]
    private Text baking_soda_price_text;
    [SerializeField]
    private Text sparkling_water_price_text;
    [SerializeField]
    private Text btb_price_text;
    [SerializeField]
    private Text methyl_price_text;
    [SerializeField]
    private Text phenol_price_text;

    public GameObject shop_btn_obj;
    public GameObject sell_btn_obj;

    public Sprite[] solution_sprite_list;
    public string[] solution_name_list;

    public string[] btb_result_color_list;
    public string[] methyl_result_color_list;
    public string[] phenol_result_color_list;

    public GameObject guide;

    public int page;
    public Text page_text;

    public Image unlock_group_solution_img;
    public Text unlock_group_solution_name_text;
    public Text solution_result_color_text;

    public GameObject lock_solution_group;
    public Image lock_group_solution_img;

    public int add_health_price;
    public int water_price;
    public int vinegar_price;
    public int orange_juice_price;
    public int baking_soda_price;
    public int sparkling_water_price;

    public int btb_price;
    public int methyl_price;
    public int phenol_price;

    // 순서대로
    // 0. add_health_price
    // 1. water_price
    // 2. vinegar_price
    // 3. orange_juice_price
    // 4. baking_soda_price
    // 5. sparkling_water_price
    // 6. btb_price
    // 7. methyl_price
    // 8. phenol_price
    public int[] add_prices;

    public bool is_sell;

    public GameObject shop;
    public GameObject goods;
    public GameObject sell;
    public GameObject need_money_obj;
    public GameObject need_solution_obj;
    public GameObject go_running_check_obj;
    public GameObject go_experiment_check_obj;

    [SerializeField]
    private GameObject three_days_obj;

    private void Awake()
    {
        SetPrice();

        add_prices = new int[9];
    }

    private void SetPrice()
    {
        add_health_price = 150;
        water_price = 700;
        vinegar_price = 2000;
        orange_juice_price = 1600;
        baking_soda_price = 3000;
        sparkling_water_price = 1300;

        btb_price = 100;
        methyl_price = 125;
        phenol_price = 150;
    }

    private void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 검
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출됨
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "HomeScene") { DecideAddPrices(); }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        gold_text.text =  gold.ToString();
        solution_text.text = solution_num.ToString();

        goods.SetActive(!is_sell);
        shop_btn_obj.SetActive(is_sell);

        sell.SetActive(is_sell);
        sell_btn_obj.SetActive(!is_sell);

        SetGoodsPrice();

        // 이스터 에그
        EasterEgg();
    }

    private void SetGoodsPrice()
    {
        add_health_price_text.text = string.Format("{0}원", add_health_price + add_prices[0]);
        water_price_text.text = string.Format("{0}원", water_price + add_prices[1]);
        vinegar_price_text.text = string.Format("{0}원", vinegar_price + add_prices[2]);
        orange_juice_text.text = string.Format("{0}원", orange_juice_price + add_prices[3]);
        baking_soda_price_text.text = string.Format("{0}원", baking_soda_price + add_prices[4]);
        sparkling_water_price_text.text = string.Format("{0}원", sparkling_water_price + add_prices[5]);
        btb_price_text.text = string.Format("{0}원", btb_price + add_prices[6]);
        methyl_price_text.text = string.Format("{0}원", methyl_price + add_prices[7]);
        phenol_price_text.text = string.Format("{0}원", phenol_price + add_prices[8]);
    }

    public void ChangeScene(string type)
    {
        SceneManager.LoadScene(type);

        if (type == "RunningScene") { DontDestroyOnLoad(this); }
    }

    public void DecideAddPrices()
    {
        // n / 10 * 10 : 10의 자리 올림 (Round() 함수는 float형임)
        add_prices[0] += SetAddPrices(add_health_price / 10, add_health_price / 2) / 10 * 10;
        add_prices[1] += SetAddPrices(water_price / 10, water_price / 2) / 10 * 10;
        add_prices[2] += SetAddPrices(vinegar_price / 10, vinegar_price / 2) / 10 * 10;
        add_prices[3] += SetAddPrices(orange_juice_price / 10, orange_juice_price / 2) / 10 * 10;
        add_prices[4] += SetAddPrices(baking_soda_price / 10, baking_soda_price / 2) / 10 * 10;
        add_prices[5] += SetAddPrices(sparkling_water_price / 10, sparkling_water_price / 2) / 10 * 10;
        add_prices[6] += SetAddPrices(btb_price / 10, btb_price / 2) / 10 * 10;
        add_prices[7] += SetAddPrices(methyl_price / 10, methyl_price / 2) / 10 * 10;
        add_prices[8] += SetAddPrices(add_health_price / 10, phenol_price / 2) / 10 * 10;
    }

    private int SetAddPrices(int start, int end) { return Random.Range(start, end + 1); }

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
