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
        if (resultWindow = null)
        {
            Debug.LogError("ResultPanelが設定されていません");
            return;
        }

        resultWindow.SetActive(true);

        if (resultText != null)
        {
            resultText.text = "VICTORY";
        }
    }

    /// <summary>
    /// 敗北画面を表示
    /// </summary>
    public void ShowDefeat()
    {
        if (resultWindow = null)
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

        SceneManager.LoadScene("StageScene");
    }
}