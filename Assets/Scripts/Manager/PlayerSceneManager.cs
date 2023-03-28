using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSceneManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerHUD playerHUD;
    [SerializeField] private Image setting;

    private bool isStop = false;

    private void Start()
    {
        playerManager.OnChangeAimModeDelegate(ChangeAimMode);
        playerManager.Init();
    }
    private void Update()
    {
        Setting();
    }

    private void ChangeAimMode(bool _ainMode)
    {
        playerHUD.ChangeAimMode(_ainMode);
    }

    private void Setting()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isStop)
        {
            isStop = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            setting.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && isStop)
        {
            isStop = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            setting.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
