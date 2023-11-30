using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int solution_num = 0;
    // 용액
    public static int btb_num = 0;
    public static int methyl_num = 0;
    public static int phenol_num = 0;

    // 상점 아이템
    public static int water_num = 0;
    public static int vinegar_num = 0;
    public static int orange_juice_num = 0;
    public static int baking_soda_num = 0;
    public static int sparkling_water_num = 0;

    // 플레이어 재화
    public static int gold = 0;

    // 플레이어 체력 증가치
    public static float add_health = 0.0f;
}