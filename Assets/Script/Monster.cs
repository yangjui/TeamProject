using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float alphaCutoutValue = 1f; // ÃÊ±â Alpha Cutout °ª

    private Renderer monsterRenderer;

    private void Start()
    {
        monsterRenderer = GetComponent<Renderer>();
    }

    public float GetAlphaCutoutValue()
    {
        return alphaCutoutValue;
    }

    public void SetAlphaCutoutValue(float value)
    {
        alphaCutoutValue = value;
        monsterRenderer.material.SetFloat("_AlphaCutout", value);
    }
}
