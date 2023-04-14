using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image reStartImage = null;

    public void OnReStartImage()
    {
        reStartImage.gameObject.SetActive(true);
    }

    public void ChangePlayScene()
    {
        SoundManager.instance.StopBGM();
        LoadingSceneController.LoadScene("PlayScene");
    }

    public void ChangeTitleScene()
    {
        SoundManager.instance.StopBGM();
        LoadingSceneController.LoadScene("TitleScene");
    }
}
