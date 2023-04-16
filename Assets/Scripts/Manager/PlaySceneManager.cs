using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    [SerializeField] private NavAgentManager navAgentManager;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private Image option;
    [SerializeField] private OptionSetting optionSetting;
    [SerializeField] private UIManager uiManager;
    [System.NonSerialized] public Transform playerPosition;
    [SerializeField] private WaveTrigger waveTrigger;
    [SerializeField] private OffScreenIndicator offScreenIndicator;

    private bool isStop = false;

    private void Awake()
    {
        SoundManager.instance.Init();
        playerManager.OnChangeAimModeDelegate(ChangeAimMode);
        playerManager.OnPlayerIsDeadDelegate(OnReStart);
        playerManager.Init();
        offScreenIndicator.Init();
        optionSetting.Init();
        navAgentManager.Init(PlayerPosition());
        waveTrigger.WaveChangeDelegate(ChangeWave);
    }

    private void Start()
    {
        
        SoundManager.instance.PlayBGM((int)SoundManager.Stage3_BGM.main);
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

    private void ChangeWave()
    {
        navAgentManager.ChangeWave();
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
        SoundManager.instance.StopBGM();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
        LoadingSceneController.LoadScene("TitleScene");
    }
}
