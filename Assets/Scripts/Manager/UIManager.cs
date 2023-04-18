using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("# ReStart")]
    [SerializeField] private Image reStartPanel = null;

    [Header("# EnemyCount")]
    [SerializeField] private TextMeshProUGUI groupACount;
    [SerializeField] private TextMeshProUGUI groupBCount;
    [SerializeField] private TextMeshProUGUI groupCCount;

    [Header("# RecordPanel")]
    [SerializeField] private TextMeshProUGUI totalCount;
    [SerializeField] private TextMeshProUGUI killCount;
    [SerializeField] private TextMeshProUGUI maxKillCount;
    [SerializeField] private TextMeshProUGUI playTime;
    [SerializeField] private PlayTime playTimeScript;

    [Header("# MaxKillCount")]
    [SerializeField] private TextMeshProUGUI maxKill;

    private int groupA = 0;
    private int groupB = 0;
    private int groupC = 0;
    private int currentGroupA;
    private int currentGroupB;
    private int currentGroupC;

    public void OnReStartImage()
    {
        reStartPanel.gameObject.SetActive(true);
        TotalCount();
        KillCount();
        PlayTime();
    }

    public void ChangePlayScene()
    {
        SoundManager.instance.StopBGM();
        LoadingSceneController.LoadScene("PlayScene");
    }

    public void ChangeTitleScene()
    {
        SoundManager.instance.StopBGM();
        LoadingSceneController.LoadScene("TitleScene");
    }

    public void GroupACount(int _count)
    {
        if (groupA == 0)
        {
            Debug.Log("A");
            groupA = _count;
            currentGroupA = _count;
        }
        else currentGroupA = _count;
        groupACount.text = "GroupA : " + (currentGroupA + " / " + groupA).ToString();
    }

    public void GroupBCount(int _count)
    {
        Debug.Log("B");
        if (groupB == 0)
        {
            groupB = _count;
            currentGroupB = _count;
        }
        else currentGroupB = _count;
        groupBCount.text = "GroupB : " + (currentGroupB + " / " + groupB).ToString();
    }

    public void GroupCCount(int _count)
    {
        Debug.Log("C");
        if (groupC == 0)
        {
            groupC = _count;
            currentGroupC = _count;
        }
        else currentGroupC = _count;
        groupCCount.text = "GroupC : " + (currentGroupC + " / " + groupC).ToString();
    }

    public void TotalCount()
    {
        totalCount.text = "TotalCount        " + (groupA + groupB + groupC);
    }

    public void KillCount()
    {
        killCount.text = "KillCount          " + (groupA + groupB + groupC - currentGroupA - currentGroupB - currentGroupC);
    }

    public void MaxKillCount(int _count)
    {
        maxKillCount.text = "MaxKillCount    " + _count;
    }

    public void PlayTime()
    {
        playTime.text = "PlayTime            " + playTimeScript.GetMinute() + " : " + playTimeScript.GetSecond();
    }

    public void MaxKill(int _count)
    {
        StartCoroutine(MaxKillCoroutine(_count));
    }

    private IEnumerator MaxKillCoroutine(int _count)
    {
        maxKill.gameObject.SetActive(true);
        maxKill.text = _count + "Kill";
        yield return new WaitForSeconds(2);
        maxKill.gameObject.SetActive(false);
    }
}
