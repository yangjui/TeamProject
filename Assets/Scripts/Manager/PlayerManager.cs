using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public delegate void ChangeAimModeDelegate(bool _aimMode);
    private ChangeAimModeDelegate changeAimModeCallback = null;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform startPos;
    [SerializeField] private PlayerHUD playerHUD;

    private GameObject go;

    public void Init()
    {
        go = Instantiate(playerPrefab, startPos.position, Quaternion.identity);
        go.GetComponent<WeaponSwitchingSystem>().playerHUD = playerHUD;
        go.GetComponent<WeaponSwitchingSystem>().Init();
        go.GetComponentInChildren<WeaponAssaultRifle>().OnChangeAimModeDelegate(OnChangeAimMode);
    }

    private void OnChangeAimMode(bool _aimMode)
    {
        changeAimModeCallback?.Invoke(_aimMode);
        go.GetComponent<PlayerRotate>().ChangeAimMode(_aimMode);
        go.GetComponent<PlayerController>().ChangeAimMode(_aimMode);
    }

    public void OnChangeAimModeDelegate(ChangeAimModeDelegate _changeAimModeCallback)
    {
        changeAimModeCallback = _changeAimModeCallback;
    }
}
