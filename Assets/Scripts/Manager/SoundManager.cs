using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public enum Stage1_BGM { main, };
    public enum Stage2_BGM { main, main2, };
    public enum Stage3_BGM { main, };

    public static SoundManager instance;

    [SerializeField] Sound[] sfx2D = null;
    [SerializeField] Sound[] sfx3D = null;
    [SerializeField] private AudioMixer mixer = null;

    [SerializeField] private GameObject bgmPrefab = null;
    [SerializeField] GameObject sfx2DPrefab = null;
    [SerializeField] GameObject sfx3DPrefab = null;
    private List<GameObject> sfx2DList = new List<GameObject>();
    private List<GameObject> sfx3DList = new List<GameObject>();

    private AudioClip[] bgm;
    private GameObject bgmPlayer;
    private AudioSource bgmSource;

    private AudioSource sfx2DI;
    private AudioSource sfx3DI;

    private float timer;

    public SoundManager()
    {
        instance = this;
    }
    
    private void Start()
    {
        // 믹서 볼륨과 UI 설정 값 동기화
        mixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("sfx")) * 20); 
        mixer.SetFloat("BGM", Mathf.Log10(PlayerPrefs.GetFloat("bgm")) * 20);
    }

    public void Init()
    {
        bgmPlayer = Instantiate(bgmPrefab, transform);
        bgmSource = bgmPlayer.GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().buildIndex == 0) // 타이틀씬이라면
        {
            LoadTitleScene();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1) // 트레이닝씬이라면
        {
            LoadTrainingScene();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2) // 플레이씬이라면
        {
            LoadPlayScene();
        }
    }

    private void OnDestroy()
    {
        foreach (AudioClip clip in bgm)
        {
            Resources.UnloadAsset(clip);
        }
        bgm = null;
    }

    private void LoadTitleScene()
    {
        bgm = Resources.LoadAll<AudioClip>("Sounds\\Stage1\\BGM");
    }

    private void LoadTrainingScene()
    {
        bgm = Resources.LoadAll<AudioClip>("Sounds\\Stage2\\BGM");
    }

    private void LoadPlayScene()
    {
        bgm = Resources.LoadAll<AudioClip>("Sounds\\Stage3\\BGM");
    }

    public void PlayBGM(int _Enum)
    {
        if (!bgmSource.isPlaying)
        {
            bgmSource.clip = bgm[_Enum];
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        StartCoroutine(StopBGMCoroutine());
    }

    private IEnumerator StopBGMCoroutine()
    {
        float curVolume = bgmSource.volume;
        while (timer < 1.5f)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0.1f, 0f, timer);
            yield return null;
        }
        bgmSource.Stop();
        bgmSource.volume = curVolume;
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
                        sfxJ.volume = 0.15f;
                        sfxJ.Play();
                        return;
                    }
                }
                Add2DList();

                for (int j = 0; j < sfx2DList.Count; j++) // 음원을 재생할 List의 갯수만큼 반복문 동작
                {
                    AudioSource sfxJ = sfx2DList[j].GetComponent<AudioSource>(); // 캐싱
                    // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!sfxJ.isPlaying)
                    {
                        sfxJ.clip = sfx2D[i].clip;
                        sfxJ.volume = 0.15f;
                        sfxJ.Play();
                        return;
                    }
                }
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
                        sfxJ.volume = 0.15f;
                        sfxJ.Play();
                        return;
                    }
                }
                Add3DList();

                for (int j = 0; j < sfx3DList.Count; j++)
                {
                    AudioSource sfxJ = sfx3DList[j].GetComponent<AudioSource>();
                    // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!sfxJ.isPlaying)
                    {
                        sfxJ.clip = sfx3D[i].clip;
                        sfxJ.transform.position = _position;
                        sfxJ.volume = 0.15f;
                        sfxJ.Play();
                        return;
                    }
                }
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

    public void SFX2DVolumeControl(string _sfxName, float _volume)
    {
        if (sfx2DList.Count == 0) return;
        for (int i = 0; i < sfx2DList.Count; ++i)
        {
            sfx2DI = sfx2DList[i].GetComponent<AudioSource>();
            if (sfx2DI.clip != null && sfx2DI.clip.name == _sfxName && sfx2DI.isPlaying)
            {
                sfx2DI.volume = _volume;
                return;
            }
        }
    }

    public void SFX3DVolumeControl(string _sfxName, float _volume)
    {
        if (sfx3DList.Count == 0) return;
        for (int i = 0; i < sfx3DList.Count; ++i)
        {
            sfx3DI = sfx3DList[i].GetComponent<AudioSource>();
            if (sfx3DI.clip != null && sfx3DI.clip.name == _sfxName && sfx3DI.isPlaying)
            {
                sfx3DI.volume = _volume;
                return;
            }
        }
    }

    public void Stop2DSFX(string _sfxName)
    {
        if (sfx2DList.Count == 0) return;
        for (int i = 0; i < sfx2DList.Count; ++i)
        {
            Debug.Log(i);
            if (sfx2DList[i].GetComponent<AudioSource>() != null)
            {
                AudioSource sfxI = sfx2DList[i].GetComponent<AudioSource>();
                Debug.Log(sfxI);
                if (sfxI.clip != null)Debug.Log(sfxI.clip.name);
                Debug.Log(sfxI.isPlaying);
                Debug.Log(_sfxName);
                if (sfxI.clip != null && sfxI.clip.name == _sfxName && sfxI.isPlaying)
                {
                    sfxI.Stop();
                    Debug.Log("STOP");
                    return;
                }
            }
        }
    }

    public void Stop3DSFX(string _sfxName)
    {
        if (sfx3DList.Count == 0) return;
        for (int i = 0; i < sfx3DList.Count; ++i)
        {
            AudioSource sfxI = sfx3DList[i].GetComponent<AudioSource>();
            if (sfxI.clip != null && sfxI.clip.name == _sfxName && sfxI.isPlaying)
            {
                sfxI.Stop();
                return;
            }
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

    public void PlayExplosionSound()
    {
        StartCoroutine(LowCoroutine());
    }

    private IEnumerator LowCoroutine()
    {
        bool isLowPassActive = true;
        float lowPassTime = 2f;
        float lowPassReadyTime = 0f;
        float lowPassCurrentTime = 0f;
        while (isLowPassActive)
        {
            if (lowPassReadyTime < 1f)
            {
                mixer.SetFloat("Low", 500f);
                lowPassReadyTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                lowPassCurrentTime += Time.deltaTime;
                float t = lowPassCurrentTime / lowPassTime;
                if (t >= 1f)
                {
                    mixer.SetFloat("Low", 22000f);
                    isLowPassActive = false;
                }
                else
                {
                    float frequency = Mathf.Lerp(500f, 22000f, t);
                    mixer.SetFloat("Low", frequency);
                }
                yield return null;
            }
        }
    }
}