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
                SoundManager.instance.PlayBGM("�����_�̾���");
                break;
            case 1:
                SoundManager.instance.PlayBGM("����_������� ���� ��ó��");
                break;
        }
    }

    private void StopBGM()
    {
        SoundManager.instance.StopBGM();
    }
}
