using UnityEngine;
using UnityEngine.SceneManagement;

// ホームシーンの管理クラス
// ホームシーンは、ゲームのメインメニューやスタート画面などを担当するシーン

public class HomeManager : MonoBehaviour
{
    // バトルシーンに遷移するメソッド(どこかのシーンに派生させときはこんな感じでできます)
    // このメソッドは仮置きですので実際には仕様しないかもです。
    public void GoToBattle()
    {
        SceneLoader.NextSceneName = "BattleScene";
        SceneManager.LoadScene("LoadingScene");
    }
}