using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private float timer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaySounds();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayBGM();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            StopBGM();
        }
    }

    private void PlaySounds()
    {
        SoundManager.instance.Play2DSFX("ShotSound", transform.position);
    }
    private void PlayBGM()
    {
        int random = Random.Range(0, 2);
        switch (random)
        {
            case 0:
                SoundManager.instance.PlayBGM("양다일_미안해");
                break;
            case 1:
                SoundManager.instance.PlayBGM("버즈_사랑하지 않은 것처럼");
                break;
        }
    }

    private void StopBGM()
    {
        SoundManager.instance.StopBGM();
    }
}
