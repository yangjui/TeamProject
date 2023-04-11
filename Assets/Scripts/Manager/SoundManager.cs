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

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] Sound[] sfx2D = null;
    [SerializeField] Sound[] sfx3D = null;
    [SerializeField] Sound[] bgm = null;
    [SerializeField] private AudioMixer mixer = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] GameObject sfx2DPrefab = null;
    [SerializeField] GameObject sfx3DPrefab = null;
    private List<GameObject> sfx2DList = new List<GameObject>();
    private List<GameObject> sfx3DList = new List<GameObject>();
    private float timer;

    private void Start()
    {
        instance = this;
        // 믹서 볼륨과 UI 설정 값 동기화
        mixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("sfx")) * 20); 
        mixer.SetFloat("BGM", Mathf.Log10(PlayerPrefs.GetFloat("bgm")) * 20);
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

    public void StopSFX(string _sfxName)
    {
        for (int i = 0; i < sfx2DList.Count; ++i)
        {
            AudioSource sfxI = sfx2DList[i].GetComponent<AudioSource>();
            if (sfxI.clip.name == _sfxName && sfxI.isPlaying)
            {
                sfxI.Stop();
            }
        }
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

    public void Play2DSFX(string p_sfxName)
    {
        if (sfx2DList.Count == 0)
        {
            Add2DList();
        }
        
        for (int i = 0; i < sfx2D.Length; ++i) // 넣어둔 음원 갯수만큼 반복문 동작
        {
            if (p_sfxName == sfx2D[i].name) // 해당 음원 찾았다면 아래 반복문 동작
            {
                for (int j = 0; j < sfx2DList.Count; j++) // 음원을 재생할 List의 갯수만큼 반복문 동작
                {
                    AudioSource sfxJ = sfx2DList[j].GetComponent<AudioSource>(); // 캐싱
                    // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!sfxJ.isPlaying)
                    {
                        sfxJ.clip = sfx2D[i].clip;
                        sfxJ.Play();
                        return;
                    }
                }
                Add2DList();

                AudioSource sfxI = sfx2DList[i].GetComponent<AudioSource>(); // 캐싱
                sfxI.clip = sfx2D[i].clip;
                sfxI.Play();
                return;
            }
        }
        Debug.Log(p_sfxName + " 이름의 효과음이 없습니다.");
        return;
    }

    private void Add2DList()
    {
        GameObject go = Instantiate(sfx2DPrefab, transform);
        sfx2DPrefab.GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        sfx2DList.Add(go);
    }

    public void Play3DSFX(string p_sfxName, Vector3 _position)
    {
        if (sfx3DList.Count == 0)
        {
            Add3DList();
        }

        for (int i = 0; i < sfx3D.Length; ++i)
        {
            if (p_sfxName == sfx3D[i].name)
            {
                for (int j = 0; j < sfx3DList.Count; j++)
                {
                    AudioSource sfxJ = sfx3DList[j].GetComponent<AudioSource>();
                    // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!sfxJ.isPlaying)
                    {
                        sfxJ.clip = sfx3D[i].clip;
                        sfxJ.transform.position = _position;
                        sfxJ.Play();
                        return;
                    }
                }
                Add3DList();

                AudioSource sfxI = sfx3DList[i].GetComponent<AudioSource>();
                sfxI.clip = sfx3D[i].clip;
                sfxI.transform.position = _position;
                sfxI.Play();
                return;
            }
        }
        Debug.Log(p_sfxName + " 이름의 효과음이 없습니다.");
        return;
    }

    private void Add3DList()
    {
        for (int i = 0; i < 5; ++i)
        {
            GameObject go = Instantiate(sfx3DPrefab, transform);
            sfx3DPrefab.GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            sfx3DList.Add(go);
        }
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