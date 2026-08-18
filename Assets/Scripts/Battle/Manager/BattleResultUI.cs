using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleResultUI : MonoBehaviour
{

    public static BattleResultUI Instance;

    //リザルトウィンドウ
    [Header("Result Window")]
    [SerializeField]
    private GameObject resultWindow;

    //リザルトテキスト
    [Header("Result Text")]
    [SerializeField]
    private TMPro.TMP_Text resultText;

    //報酬テキスト
    [Header("Reward Text")]
    [SerializeField]
    private TMPro.TMP_Text rewardText;

    private void Awake()
    {
        Instance = this;

        // 最初は非表示
        if (resultWindow != null)
        {
            resultWindow.SetActive(false);
        }
    }

    /// <summary>
    /// 勝利画面を表示
    /// </summary>
    public void ShowVictory()
    {
        if (resultWindow == null)
        {
            Debug.LogError("ResultPanelが設定されていません");
            return;
        }

        resultWindow.SetActive(true);

        //勝利表示
        if (resultText != null)
        {
            resultText.text = "VICTORY";
        }

        // 報酬表示
        if (rewardText != null)
        {
            rewardText.text = "カード × 1\nコイン × 100";
        }
    }

    /// <summary>
    /// 敗北画面を表示
    /// </summary>
    public void ShowDefeat()
    {
        if (resultWindow == null)
        {
            Debug.LogError("ResultPanelが設定されていません");
            return;
        }

        resultWindow.SetActive(true);

        if (resultText != null)
        {
            resultText.text = "DEFEAT";
        }
    }

    /// <summary>
    /// マップへ戻る
    /// </summary>
    public void ReturnToMap()
    {
        Time.timeScale = 1f;

        //β版
        //今後（マスター版）はオーバーレイ表示に変更
      SceneManager.LoadScene("StageScene");
    }
}