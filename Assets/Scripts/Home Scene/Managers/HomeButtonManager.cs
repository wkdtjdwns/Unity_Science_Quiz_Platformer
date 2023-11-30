using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButtonManager : ButtonManager
{
    HomeGameManager game_manager;
    Guide guide;

    private void Awake()
    {
        game_manager = GameObject.Find("GameManager").gameObject.GetComponent<HomeGameManager>();
        guide = GameObject.Find("Guide").gameObject.GetComponent<Guide>();
    }

    public void OpenShop() { SoundManager.instance.PlaySound("button"); game_manager.shop.SetActive(true); }

    public void CloseShop() { SoundManager.instance.PlaySound("button"); game_manager.shop.SetActive(false); }

    public void OpenSell() { SoundManager.instance.PlaySound("button"); game_manager.is_sell = true; }

    public void CloseSell() { SoundManager.instance.PlaySound("button"); game_manager.is_sell = false; }

    public void OpenGuide() { SoundManager.instance.PlaySound("button"); game_manager.guide.SetActive(true); }

    public void CloseGuide() { SoundManager.instance.PlaySound("button"); game_manager.guide.SetActive(false); }

    public void GoRunning() { SoundManager.instance.PlaySound("button"); game_manager.go_running_check_obj.SetActive(true); }

    public void GoRunningCheck() { SoundManager.instance.PlayBgm("running"); game_manager.ChangeScene("RunnigScene"); }

    public void NotGoRunning() { SoundManager.instance.PlaySound("button"); game_manager.go_running_check_obj.SetActive(false); }

    public void GoExperiment() { SoundManager.instance.PlaySound("button"); game_manager.go_experiment_check_obj.SetActive(true); }

    public void GoExperimentCheck() { SoundManager.instance.PlayBgm("experiment"); ; game_manager.ChangeScene("ExperimentScene"); }

    public void NotGoExperiment() { SoundManager.instance.PlaySound("button"); game_manager.go_experiment_check_obj.SetActive(false); }

    public void PageUp()
    {
        if (game_manager.page >= 2) { return; }

        ++game_manager.page;
        ChangePage();
    }

    public void PageDown()
    {
        if (game_manager.page <= 0) { return; }

        --game_manager.page;
        ChangePage();
    }

    private void ChangePage()
    {
        SoundManager.instance.PlaySound("button");

        game_manager.lock_solution_group.SetActive(!guide.solution_unlock_list[game_manager.page]);

        game_manager.page_text.text = string.Format("#{0:00}", (game_manager.page + 1));

        if (game_manager.lock_solution_group.activeSelf) { game_manager.lock_group_solution_img.sprite = game_manager.solution_sprite_list[game_manager.page]; }

        else
        {
            game_manager.unlock_group_solution_img.sprite = game_manager.solution_sprite_list[game_manager.page];
            game_manager.unlock_group_solution_name_text.text = game_manager.solution_name_list[game_manager.page];

            switch (game_manager.page)
            {
                case 0:
                    game_manager.solution_result_color_text.text = string.Format("산성 : {0}\n중성 : {1}\n염기성 : {2}", game_manager.btb_result_color_list[0], game_manager.btb_result_color_list[1], game_manager.btb_result_color_list[2]);
                    break;

                case 1:
                    game_manager.solution_result_color_text.text = string.Format("산성 : {0}\n중성 : {1}\n염기성 : {2}", game_manager.methyl_result_color_list[0], game_manager.methyl_result_color_list[1], game_manager.methyl_result_color_list[2]);
                    break;

                case 2:
                    game_manager.solution_result_color_text.text = string.Format("산성 : {0}\n중성 : {1}\n염기성 : {2}", game_manager.phenol_result_color_list[0], game_manager.phenol_result_color_list[1], game_manager.phenol_result_color_list[2]);
                    break;
            }
        }
    }
}
