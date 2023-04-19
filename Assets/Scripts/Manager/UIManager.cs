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
    [SerializeField] private TextMeshProUGUI currentMaxKill;

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
            groupA = _count;
            currentGroupA = _count;
        }
        else currentGroupA = _count;
        groupACount.text = "GroupA : " + (currentGroupA + " / " + groupA).ToString();
    }

    public void GroupBCount(int _count)
    {
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
        totalCount.text = (groupA + groupB + groupC).ToString();
    }

    public void KillCount()
    {
        killCount.text = (groupA + groupB + groupC - currentGroupA - currentGroupB - currentGroupC).ToString();
    }

    public void MaxKillCount(int _count) // 겜끝났을때 알려주는거
    {
        maxKillCount.text = _count.ToString();
    }

    public void MaxKill(int _count) // 30마리이상죽일때 알려주는거
    {
        StartCoroutine(MaxKillCoroutine(_count));
    }

    public void PlayTime()
    {
        playTime.text = playTimeScript.GetMinute() + " : " + playTimeScript.GetSecond();
    }

    private IEnumerator MaxKillCoroutine(int _count)
    {
        currentMaxKill.gameObject.SetActive(true);
        currentMaxKill.text = _count + "Kill";
        yield return new WaitForSeconds(2);
        currentMaxKill.gameObject.SetActive(false);
    }
}
