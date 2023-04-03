using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSceneManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private Image option;
    [SerializeField] private OptionSetting optionSetting;

    private bool isStop = false;

    private void Start()
    {
        playerManager.OnChangeAimModeDelegate(ChangeAimMode);
        playerManager.Init();
        optionSetting.Init();
    }

    private void Update()
    {
        Option();
    }

    private void ChangeAimMode(bool _ainMode)
    {
        playerHUD.ChangeAimMode(_ainMode);
    }

    private void Option()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isStop)
            {
                isStop = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                option.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }

            else if (isStop)
            {
                isStop = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                option.gameObject.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        if(Input.GetKeyDown(KeyCode.KeypadEnter))
            LoadingSceneController.LoadScene("PlayScene");
    }

    public void OptionClose()
    {
        isStop = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        option.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnTitleScene()
    {
        isStop = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
        LoadingSceneController.LoadScene("TitleScene");
    }
}
