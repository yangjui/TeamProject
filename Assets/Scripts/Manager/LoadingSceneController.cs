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
        // LoadScene�� ���������� ���� �ҷ����ԵǼ� ���� �� �ҷ����� ������ �ƹ� �ൿ�� �� �� ����.
        // ������ LoadSceneAsync�� �񵿱������� ���� �ҷ����⿡ ���� �ҷ����鼭 �ٸ� �ൿ�� �� �� �ִ�.
        // AsyncOperation ������� ���� �󸶳� �ҷ����������� ��ȯ�޾Ƽ� �� �� �ִ�.

        op.allowSceneActivation = false;
        // ���� �񵿱�� �ҷ����� �� ���� �ε��� ������ �ڵ����� ���� ���� �ҷ������� �����ϴ� ����̴�.
        // false�� ����� ������ �ε��� �ʹ� ������ �� ȭ���� Ȯ �Ѿ�°��� �����ϱ� �����̴�.
        // false �� �����ϸ� �ε��� 90%���� �� �� �߰� ����� ���� ������ ����Ѵ�.
        float timer = 0f;
        while (!op.isDone) // �� �ε��� ������ �ʾҴٸ� ��� �ݺ��Ѵ�.
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime * 0.5f; // �ε��� 90%���� á�ٸ� Ÿ�̸� ������ unscaledDeltaTime�� �����ְ�
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // 90%�� �ε��� 1�ʵ��� 10% �� ä�쵵�� ������ش�.
                if (progressBar.fillAmount >= 1f) // 1�ʰ� �����ٸ�
                {
                    op.allowSceneActivation = true; // true�� ����� ���������� �Ѿ��.
                    yield break;
                }
            }
        }
    }
}
