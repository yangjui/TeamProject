using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private Targets[] targets;

    private void Start()
    {
        for (int i = 0; i < targets.Length; ++i)
        {
            targets[i].GetComponent<Targets>().OnTriggerDelegate(SetNextNav);
            targets[i].gameObject.SetActive(false);
        }
        targets[0].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            targets[1].gameObject.SetActive(false);
            targets[2].gameObject.SetActive(true);
        }
    }

    private void SetNextNav(string _name)
    {
        if (_name == "Quest1")
        {
            targets[0].gameObject.SetActive(false);
            targets[1].gameObject.SetActive(true);
            StartCoroutine(StartRoundCoroutine());
            Invoke(nameof(OffTarget2), 8f);
        }
        else if (_name == "Quest3")
        {
            targets[2].gameObject.SetActive(false);
            targets[3].gameObject.SetActive(true);
            targets[4].gameObject.SetActive(true);
            StartCoroutine(StartRoundCoroutine());
            Invoke(nameof(OffTarget4n5), 8f);
        }
    }

    private IEnumerator StartRoundCoroutine()
    {
        SoundManager.instance.Play2DSFX("ZombieCorps01");
        yield return new WaitForSeconds(1f);
        SoundManager.instance.Play2DSFX("ZombieCorps02");
        yield return new WaitForSeconds(1f);
        SoundManager.instance.Play2DSFX("ZombieCorps03");
    }

    private void OffTarget2()
    {
        targets[1].gameObject.SetActive(false);
    }
    private void OffTarget4n5()
    {
        targets[3].gameObject.SetActive(false);
        targets[4].gameObject.SetActive(false);
    }
}
