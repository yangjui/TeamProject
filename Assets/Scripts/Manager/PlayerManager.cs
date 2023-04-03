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
    [System.NonSerialized] public Transform playerTransform;
    [System.NonSerialized] public GameObject go;

    public static PlayerManager instance;
    public void Init()
    {
        go = Instantiate(playerPrefab, startPos.position, Quaternion.identity);
        go.GetComponent<WeaponSwitchingSystem>().playerHUD = playerHUD;
        go.GetComponent<WeaponSwitchingSystem>().Init();
        go.GetComponentInChildren<WeaponAssaultRifle>().OnChangeAimModeDelegate(OnChangeAimMode);
        go.GetComponent<PlayerController>().SetUnderAttackDelegate(StartBloodScreenCoroutine);

        instance = this;

        playerTransform = go.transform;
    }

    public void Update()
    {
        Vector3 playerPosition = playerTransform.position;
    }

    private void OnChangeAimMode(bool _aimMode)
    {
        changeAimModeCallback?.Invoke(_aimMode);
        go.GetComponent<PlayerRotate>().ChangeAimMode(_aimMode);
        go.GetComponent<PlayerController>().ChangeAimMode(_aimMode);
    }

    private void StartBloodScreenCoroutine()
    {
        playerHUD.StartBloddScreenCoroutine();
    }

    public void OnChangeAimModeDelegate(ChangeAimModeDelegate _changeAimModeCallback)
    {
        changeAimModeCallback = _changeAimModeCallback;
    }
}
