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
        LoadingSceneController.LoadScene("PlayScene");
    }

    public void ChangeTitleScene()
    {
        LoadingSceneController.LoadScene("TitleScene");
    }
}
