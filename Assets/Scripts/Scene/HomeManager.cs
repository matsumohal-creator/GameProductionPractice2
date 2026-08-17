using UnityEngine;
using UnityEngine.SceneManagement;

// ホームシーンの管理クラス
// ホームシーンは、ゲームのメインメニューやスタート画面などを担当するシーン
public class HomeManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private SlidePanel partyPanel;
    // バトルシーンに遷移するメソッド(どこかのシーンに派生させときはこんな感じでできます)
    // このメソッドは仮置きですので実際には仕様しないかもです。
    public void GoToBattle()
    {
        SceneLoader.NextSceneName = "BattleScene";
        SceneManager.LoadScene("LoadingScene");
    }

    public void OnClickParty()
    {
        if (partyPanel != null)
        {
            partyPanel.Open();
            return;
        }

        SceneLoader.NextSceneName = "EditScene";
        SceneManager.LoadScene("LoadingScene");
    }

    public void OnClickStage()
    {
        if (partyPanel != null)
        {
            partyPanel.Open();
            return;
        }

        SceneLoader.NextSceneName = "StageScene";
        SceneManager.LoadScene("LoadingScene");
    }   
}