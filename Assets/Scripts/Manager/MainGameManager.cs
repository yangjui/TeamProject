using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] private TitleSceneManager titleSceneManager;

    public void ChangeScene()
    {
        titleSceneManager.ChangePlayScene();
    }
}
