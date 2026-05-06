using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

// ローディングシーンの管理クラス
// ローディングシーンは、次のシーンを非同期で読み込むためのシーン

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar;
    public TMP_Text progressText;

    void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneLoader.NextSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        float minTime = 2.0f; // 最低表示時間
        while (!op.isDone)
        {
            // タイマーを更新
            timer += Time.deltaTime;
            // ローディングの進捗を0～1の範囲で取得
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            // プログレスバーとテキストを更新
            progressBar.value = progress;
            if (progressText != null)
                progressText.text = (progress * 100f).ToString("F0") + "%";

            // シーンの読み込みが完了し、最低表示時間が経過したらシーンを切り替える
            if (progress >= 1.0f && timer >= minTime)
            {
                op.allowSceneActivation = true;
            }
            // 次のフレームまで待機
            yield return null;
        }
    }
}