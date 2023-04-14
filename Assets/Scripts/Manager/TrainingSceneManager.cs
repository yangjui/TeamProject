using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrainingSceneManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private Image option;
    [SerializeField] private OptionSetting optionSetting;

    private bool isStop = false;

    private void Awake()
    {
        SoundManager.instance.Init();
    }

    private void Start()
    {
        playerManager.OnChangeAimModeDelegate(ChangeAimMode);
        playerManager.Init();
        optionSetting.Init();

        SoundManager.instance.PlayBGM((int)SoundManager.Stage2_BGM.main);

        //int random = Random.Range(0, 2);
        //switch (random)
        //{
        //    case 0:
        //        SoundManager.instance.PlayBGM("BGM_Idle1");
        //        break;
        //    case 1:
        //        SoundManager.instance.PlayBGM("BGM_Idle2");
        //        break;
        //}
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