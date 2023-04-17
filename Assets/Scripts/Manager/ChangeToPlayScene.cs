using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeToPlayScene : MonoBehaviour
{
    [SerializeField] private Image fadePanel;

    private Color c;
    private bool change = false;

    private void Awake()
    {
        c = fadePanel.color;
        c = Color.black;
        c.a = 0f;
        fadePanel.color = c;
    }

    public void ChangeScene()
    {
        if (change) return;
        StartCoroutine(FadeInOutStart());
    }

    private IEnumerator FadeInOutStart()
    {
        change = true;
        SoundManager.instance.StopBGM();
        for (float f = 0f; f < 1; f += 0.02f)
        {
            c.a = f;
            fadePanel.color = c;
            yield return new WaitForSeconds(0.01f);
        }
        change = false;
        LoadingSceneController.LoadScene("Map_v2");
    }
}
