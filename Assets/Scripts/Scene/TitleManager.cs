using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void OnClickStart()
    {
        // 新しいゲームとしてセーブデータを初期化
        SaveManager.ResetSave();

        SceneLoader.NextSceneName = "HomeScene";
        SceneManager.LoadScene("LoadingScene");
    }
}