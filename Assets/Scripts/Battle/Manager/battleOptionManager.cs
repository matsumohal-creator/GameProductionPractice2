using UnityEngine;

public class BattleOptionManager : MonoBehaviour
{
    // =========================
    // 各設定ウィンドウ
    // =========================

    [Header("Battle Windows")]

    // ゲーム設定
    [SerializeField] private GameObject gameSettingWindow;

    // バトル設定
    [SerializeField] private GameObject battleSettingWindow;

    // パーティ情報
    [SerializeField] private GameObject statusWindow;

    // 戦闘ログ
    [SerializeField] private GameObject battleLogWindow;

    // 戦闘離脱確認
    [SerializeField] private GameObject exitWindow;

    // =========================
    // オプション全体
    // =========================

    [Header("Main Option Panel")]

    [SerializeField] private GameObject optionPanel;

    // =========================
    // 背景暗転
    // =========================

    [Header("Background")]

    [SerializeField] private GameObject darkBack;

    // =========================
    // 初期化
    // =========================

    private void Start()
    {
        CloseAll();

        optionPanel.SetActive(false);

        if (darkBack != null)
        {
            darkBack.SetActive(false);
        }
    }

    // =========================
    // 全ウィンドウを閉じる
    // =========================

    private void CloseAll()
    {
        gameSettingWindow.SetActive(false);
        battleSettingWindow.SetActive(false);
        statusWindow.SetActive(false);
        battleLogWindow.SetActive(false);
        exitWindow.SetActive(false);
    }

    // =========================
    // ゲーム設定
    // =========================

    public void OpenGameSetting()
    {
        CloseAll();

        gameSettingWindow.SetActive(true);
    }

    // =========================
    // バトル設定
    // =========================

    public void OpenBattleSetting()
    {
        CloseAll();

        battleSettingWindow.SetActive(true);
    }

    // =========================
    // パーティ情報
    // =========================

    public void OpenStatus()
    {
        CloseAll();

        statusWindow.SetActive(true);
    }

    // =========================
    // 戦闘ログ
    // =========================

    public void OpenBattleLog()
    {
        CloseAll();

        battleLogWindow.SetActive(true);
    }

    // =========================
    // 戦闘離脱
    // =========================

    public void OpenExitWindow()
    {
        CloseAll();

        exitWindow.SetActive(true);
    }

    // =========================
    // オプションを開く
    // =========================

    public void OpenOption()
    {
        optionPanel.SetActive(true);

        if (darkBack != null)
        {
            darkBack.SetActive(true);
        }

        // バトル停止
        Time.timeScale = 0f;

        // 最初に開く画面
        OpenBattleSetting();
    }

    // =========================
    // オプションを閉じる
    // =========================

    public void CloseOption()
    {
        optionPanel.SetActive(false);

        if (darkBack != null)
        {
            darkBack.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    // =========================
    // 戦闘離脱確認
    // =========================

    public void ConfirmExitBattle()
    {
        Debug.Log("戦闘離脱");

        Time.timeScale = 1f;

        // TODO
        // SceneManager.LoadScene("HomeScene");
    }

    public void CancelExitBattle()
    {
        OpenBattleSetting();
    }
}