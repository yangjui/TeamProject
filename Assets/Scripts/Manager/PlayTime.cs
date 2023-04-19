using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTime : MonoBehaviour
{
    private int hour = 0;
    private int minute = 0;
    private int second = 0;

    private string hourText;
    private string minuteText;
    private string secondText;

    private void Start()
    {
        StartCoroutine(TimerCoroutine());
    }

    private void Update()
    {
        if (second == 60) 
        {
            second = 0;
            minute++;
        }
        if (minute == 60)
        {
            hour++;
        }
    }

    private IEnumerator TimerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            second++;
        }
    }

    public string GetHour()
    {
        if (hour < 10)
        {
            hourText = "0" + hour;
        }
        else if (hour >= 10)
        {
            hourText = hour.ToString();
        }
        return hourText;
    }

    public string GetMinute()
    {
        if (minute < 10)
        {
            minuteText = "0" + minute;
        }
        else if (minute >= 10)
        {
            minuteText = minute.ToString();
        }
        return minuteText;
    }

    public string GetSecond()
    {
        if (second < 10)
        {
            secondText = "0" + second;
        }
        else if (second >= 10)
        {
            secondText = second.ToString();
        }
        return secondText;
    }
}
