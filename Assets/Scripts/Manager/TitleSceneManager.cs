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
    private void Awake()
    {
        c = fadePanel.color;
        c = Color.black;
        c.a = 0f;
        fadePanel.color = c;
    }

    public void ChangePlayScene()
    {
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
        //UnityEditor.EditorApplication.isPlaying = false; // 나가기 누르면 유니티 상에서도 재생이 멈춤. 대신 이거있으면 빌드 오류남
        Application.Quit();
    }
}
