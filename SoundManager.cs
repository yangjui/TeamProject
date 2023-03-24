using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
// 조금바꼇슴 '-'


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] Sound[] sfx = null;
    [SerializeField] Sound[] bgm = null;
    [SerializeField] private Transform playerPos = null;
    [SerializeField] private AudioMixer mixer = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] GameObject sfxPrefab = null;
    private List<GameObject> sfxList = new List<GameObject>();
    private float timer;

    private void Start()
    {
        instance = this;
    }

    public void PlayBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName == bgm[i].name)
            {
                if (!bgmPlayer.isPlaying)
                {
                    bgmPlayer.clip = bgm[i].clip;
                    bgmPlayer.Play();
                }
            }
        }
    }

    public void StopBGM()
    {
        StartCoroutine(StopBGMCoroutine());
    }

    private IEnumerator StopBGMCoroutine()
    {
        float curVolume = bgmPlayer.volume;
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            bgmPlayer.volume = Mathf.Lerp(0.1f, 0f, timer);
            yield return null;
        }
        bgmPlayer.Stop();
        bgmPlayer.volume = curVolume;
        timer = 0f;
    }

    public void PlaySFX(string p_sfxName, Vector3 _position)
    {
        if (sfxList.Count == 0)
        {
            AddList();
        }
        
        for (int i = 0; i < sfx.Length; ++i)
        {
            if (p_sfxName == sfx[i].name)
            {
                for (int j = 0; j < sfxList.Count; j++)
                {
                    // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!sfxList[j].GetComponent<AudioSource>().isPlaying)
                    {
                        sfxList[j].GetComponent<AudioSource>().clip = sfx[i].clip;
                        sfxList[j].GetComponent<AudioSource>().transform.position = _position;
                        sfxList[j].GetComponent<AudioSource>().Play();
                        return;
                    }
                }
                AddList();

                sfxList[i].GetComponent<AudioSource>().clip = sfx[i].clip;
                sfxList[i].GetComponent<AudioSource>().transform.position = _position;
                sfxList[i].GetComponent<AudioSource>().Play();
                return;
            }
        }
        Debug.Log(p_sfxName + " 이름의 효과음이 없습니다.");
        return;
    }

    private void AddList()
    {
        GameObject go = Instantiate(sfxPrefab, transform);
        sfxPrefab.GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        sfxList.Add(go);
    }

    public void BGMVolume(float _val)
    {
        mixer.SetFloat("BGM", Mathf.Log10(_val) * 20);
    }

    public void SFXVolume(float _val)
    {
        mixer.SetFloat("SFX", Mathf.Log10(_val) * 20);
    }
}