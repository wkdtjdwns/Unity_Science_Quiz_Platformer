using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] item_prefabs;
    [SerializeField]
    private Transform[] spawn_points;

    private bool is_spawn;
    private GameObject cur_item;

    private void Awake()
    {
        is_spawn = false;
    }

    private void Update()
    {
        if (!is_spawn) { StartCoroutine("SpawnItem"); }
    }

    private GameObject DecideItem()
    {
        int ran_index = Random.Range(0, item_prefabs.Length);

        return item_prefabs[ran_index];
    }

    private IEnumerator SpawnItem()
    {
        is_spawn = true;

        float ran_time = Random.Range(5f, 7.5f);

        yield return new WaitForSeconds(ran_time);

        int ran_index = Random.Range(0, spawn_points.Length);

        is_spawn = false;
        Instantiate(DecideItem(), spawn_points[ran_index].position, Quaternion.identity);
    }
}
