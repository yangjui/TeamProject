using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlackHole : MonoBehaviour
{
    [SerializeField] private GameObject bombEffect = null;

    private List<GameObject> agents = new List<GameObject>();
    private float destroyTime = 3f;

    private float scale = 0.0f;
    private bool isGrowingFast = true;
    private bool isGrowingSlow = false;

    private void Start()
    {
        StartCoroutine(DestroyCoroutine());
    }
    
    private void Update()
    {
        SetSize();
        Absorb();
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(destroyTime);
        isGrowingSlow = false;
        isGrowingFast = false;
        while (true)
        {
            scale = Mathf.SmoothStep(scale, 0.05f, Time.deltaTime * 20.0f);
            yield return null;
            if (scale <= 0.1f)
            {
                for (int i = agents.Count - 1; i >= 0; --i)
                {
                    if (agents[i] == null) continue;
                    if (i < 30)
                    {
                        agents[i].GetComponent<BakeZombie>().TakeDamage(100);
                        agents.RemoveAt(i);
                    }
                    else
                    {
                        agents[i].GetComponent<BakeZombie>().DeadInBlackHole();
                        agents.RemoveAt(i);
                    }
                }
                GameObject go = Instantiate(bombEffect, transform.position, Quaternion.identity);
                Destroy(go, 2f);
                Destroy(gameObject);
                break;
            }
        }
    }

    private void SetSize()
    {
        if (isGrowingFast)
        {
            // 0���� 0.2���� ������ Ŀ���� �κ�
            scale = Mathf.SmoothStep(scale, 0.05f, Time.deltaTime * 10.0f);
            if (scale >= 0.04f)
            {
                isGrowingFast = false;
                isGrowingSlow = true;
            }
        }
        else if (isGrowingSlow)
        {
            // 0.2���� 0.8���� ������ Ŀ���� �κ�
            scale = Mathf.Lerp(scale, 0.4f, Time.deltaTime * 10.0f);
            if (scale >= 0.39f)
            {
                isGrowingSlow = false;
            }
        }
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private void Absorb()
    {
        if (agents.Count == 0) return;

        for (int i = 0; i < agents.Count; ++i)
        {
            if (agents[i] == null) continue;
            
            if (agents[i].CompareTag("Zombie"))
            {
                BakeZombie bakeZombie = agents[i].GetComponent<BakeZombie>();
                bakeZombie.navAgent.enabled = false;
                bakeZombie.BlackHole();

                Vector3 dir = transform.position - agents[i].transform.position;
                agents[i].transform.position += dir * 1f * Time.deltaTime;

                Vector3 rotationDir = Quaternion.Euler(90f, 0f, 0f) * dir.normalized;
                Vector3 rotationCenter = transform.position + Vector3.up * 0.5f;
                agents[i].transform.RotateAround(rotationCenter, rotationDir, 270f * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Zombie"))
        {
            agents.Add(_other.gameObject);
        }
    }
}