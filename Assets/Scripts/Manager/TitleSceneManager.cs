using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private Image fadePanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;

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
        if (!optionPanel.activeSelf) optionPanel.SetActive(true);
        else if (optionPanel.activeSelf) optionPanel.SetActive(false);
    }

    public IEnumerator FadeInOutStart()
    {
        for (float f = 0f; f < 1; f += 0.01f)
        {
            c.a = f;
            fadePanel.color = c;
            yield return new WaitForSeconds(0.01f);
        }
        LoadingSceneController.LoadScene("PlayScene");
    }
}
