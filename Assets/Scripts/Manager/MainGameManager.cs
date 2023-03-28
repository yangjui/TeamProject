using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainGameManager : MonoBehaviour
{
    public void ChangePlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
