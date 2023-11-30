using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private float offset;
    [SerializeField]
    private float speed;

    private MeshRenderer render;

    private void Awake()
    {
        speed = 0.25f;

        render = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        BackGroundMove();
    }

    void BackGroundMove()
    {
        offset += Time.deltaTime * speed;

        render.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
