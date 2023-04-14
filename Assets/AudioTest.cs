using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> audioClips;

    private AudioSource audioSource;
    private int currentClipIndex = 0;
    private float timePressedC = 0f;
    private bool isPlaying = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isPlaying = true;
            currentClipIndex = 0;
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.Play();
        }

        if (Input.GetKey(KeyCode.C))
        {
            timePressedC += Time.deltaTime;
            if (timePressedC >= 1.48f && currentClipIndex == 0)
            {
                currentClipIndex = 1;
                audioSource.clip = audioClips[currentClipIndex];
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            isPlaying = false;
            timePressedC = 0f;
            audioSource.Stop();
        }
    }
}


