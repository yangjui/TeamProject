using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    public delegate void WaveTriggerDelegate();
    private WaveTriggerDelegate waveTriggerCallback = null;

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            waveTriggerCallback?.Invoke();
        }
    }

    public void WaveChangeDelegate(WaveTriggerDelegate _waveTriggerCallback)
    {
        waveTriggerCallback = _waveTriggerCallback;
    }
}
