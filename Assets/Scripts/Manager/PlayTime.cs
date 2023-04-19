using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTime : MonoBehaviour
{
    private int hour = 0;
    private int minute = 0;
    private int second = 0;

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

    public int GetHour()
    {
        return hour;
    }

    public int GetMinute()
    {
        return minute;
    }

    public int GetSecond()
    {
        return second;
    }
}
