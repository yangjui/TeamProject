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
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;

    private bool isStop = false;

    private void Start()
    {
        playerManager.OnChangeAimModeDelegate(ChangeAimMode);
        playerManager.Init();

        VolumeSetting();
    }

    private void VolumeSetting() // 볼륨설정값 저장
    {
        float val = SoundManager.instance.VolumeSetting();
        sfxSlider.value = Mathf.Pow(10, val / 20f);
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
        if (Input.GetKeyDown(KeyCode.Escape) && !isStop)
        {
            isStop = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            option.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }

        else if (Input.GetKeyDown(KeyCode.Escape) && isStop)
        {
            isStop = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            option.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
