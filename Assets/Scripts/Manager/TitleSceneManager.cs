using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private OptionSetting optionSetting;
    [SerializeField] private Image fadePanel;

    private Color c;
    private bool start = false;
    
    private void Awake()
    {
        c = fadePanel.color;
        c = Color.black;
        c.a = 0f;
        fadePanel.color = c;
        SoundManager.instance.Init();
    }

    private void Start()
    {
        SoundManager.instance.PlayBGM((int)SoundManager.Stage1_BGM.main);
    }

    public void ChangePlayScene()
    {
        if (start) return;
        StartCoroutine(FadeInOutStart());
    }

    public void OnOffOptionPanel()
    {
        if (!optionPanel.activeSelf)
        {
            optionPanel.SetActive(true);
            optionSetting.Init();
        }
        else if (optionPanel.activeSelf) optionPanel.SetActive(false);
    }

    public IEnumerator FadeInOutStart()
    {
        start = true;
        SoundManager.instance.Play2DSFX("GameStart");
        SoundManager.instance.SFX2DVolumeControl("GameStart", 1f);
        SoundManager.instance.StopBGM();
        for (float f = 0f; f < 1; f += 0.02f)
        {
            c.a = f;
            fadePanel.color = c;
            yield return new WaitForSeconds(0.01f);
        }
        LoadingSceneController.LoadScene("TrainingScene");
    }

    public void ExitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false; // ������ ������ ����Ƽ �󿡼��� ����� ����. ��� �̰������� ���� ������
        Application.Quit();
    }
}
