using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSceneManager : MonoBehaviour
{
    [SerializeField] private NavAgentManager navAgentManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private Image option;
    [SerializeField] private OptionSetting optionSetting;
    [SerializeField] private UIManager uiManager;
    [System.NonSerialized] public Transform playerPosition;

    private bool isStop = false;

    private void Start()
    {
        playerManager.OnChangeAimModeDelegate(ChangeAimMode);
        playerManager.OnPlayerIsDeadDelegate(OnReStart);
        playerManager.Init();
        optionSetting.Init();
        navAgentManager.Init(PlayerPosition());
    }

    private void Update()
    {
        Option();
    }

    public Transform PlayerPosition()
    {
        playerPosition = playerManager.PlayerPosition();

        return playerPosition;
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

    private void OnReStart()
    {
        uiManager.OnReStartImage();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
