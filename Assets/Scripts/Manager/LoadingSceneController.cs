using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    
    private static string nextScene;

    public static void LoadScene(string _sceneName)
    {
        nextScene = _sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        // LoadScene은 동기방식으로 씬을 불러오게되서 씬을 다 불러오기 전까지 아무 행동을 할 수 없다.
        // 하지만 LoadSceneAsync는 비동기방식으로 씬을 불러오기에 씬을 불러오면서 다른 행동을 할 수 있다.
        // AsyncOperation 방식으로 씬이 얼마나 불러와졌는지를 반환받아서 알 수 있다.

        op.allowSceneActivation = false;
        // 씬을 비동기로 불러들일 때 씬의 로딩이 끝나면 자동으로 다음 씬을 불러올지를 설정하는 기능이다.
        // false로 만드는 이유는 로딩이 너무 빨랐을 때 화면이 확 넘어가는것을 방지하기 위함이다.
        // false 로 설정하면 로딩을 90%까지 한 후 추가 명령이 있을 때까지 대기한다.
        float timer = 0f;
        while (!op.isDone) // 씬 로딩이 끝나지 않았다면 계속 반복한다.
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime * 0.5f; // 로딩이 90%까지 찼다면 타이머 변수에 unscaledDeltaTime을 더해주고
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // 90%의 로딩을 1초동안 10% 더 채우도록 만들어준다.
                if (progressBar.fillAmount >= 1f) // 1초가 지났다면
                {
                    op.allowSceneActivation = true; // true로 만들고 다음씬으로 넘어간다.
                    yield break;
                }
            }
        }
    }
}
