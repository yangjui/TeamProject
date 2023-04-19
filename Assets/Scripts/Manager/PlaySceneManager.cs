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
    [SerializeField] private QuestManager questManager;
    [SerializeField] private OffScreenIndicator offScreenIndicator;
    
    private CameraController cameraController;

    private bool isStop = false;

    private void Awake()
    {
        SoundManager.instance.Init();
        playerManager.OnChangeAimModeDelegate(ChangeAimMode);
        playerManager.OnPlayerIsDeadDelegate(OnReStart);
        playerManager.Init();
        offScreenIndicator.Init();
        optionSetting.Init();
        navAgentManager.SetCountDelegate(SetGroupA, SetGroupB, SetGroupC, SetMaxKillCount, SetCurrentMaxKillCount);
        navAgentManager.Init(PlayerPosition());
        questManager.WaveChangeDelegate(ChangeWave);
        navAgentManager.SetQuestDelegate(StartQuest3);
        navAgentManager.SetClearDelegate(OnClear);
    }

    private void Start()
    {
        SoundManager.instance.PlayBGM((int)SoundManager.Stage3_BGM.main);
        cameraController = FindObjectOfType<PlayerController>().transform.GetComponentInChildren<CameraController>();
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

    private void SetGroupA(int _count)
    {
        uiManager.GroupACount(_count);
    }

    private void SetGroupB(int _count)
    {
        uiManager.GroupBCount(_count);
    }

    private void SetGroupC(int _count)
    {
        uiManager.GroupCCount(_count);
    }

    private void SetMaxKillCount(int _count)
    {
        uiManager.MaxKillCount(_count);
    }

    private void SetCurrentMaxKillCount(int _count)
    {
        uiManager.MaxKill(_count);
    }

    private void ChangeWave(string _name)
    {
        navAgentManager.ChangeWave(_name);
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
    }

    private void StartQuest3()
    {
        questManager.StartQuest3();
    }

    private void OnReStart()
    {
        cameraController.DeadCameraMove();
        uiManager.OnReStartImage();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnClear()
    {
        uiManager.OnReStartImage();
        playerManager.OnClear();
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
