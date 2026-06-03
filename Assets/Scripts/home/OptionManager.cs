using UnityEngine;

public class OptionManager : MonoBehaviour
{
    // =========================
    // 各設定ウィンドウ
    // =========================

    [Header("Setting Windows")]

    // BGM設定ウィンドウ
    [SerializeField] GameObject bgmWindow;

    // グラフィック設定ウィンドウ
    [SerializeField] GameObject graphicWindow;

    // 操作設定ウィンドウ
    [SerializeField] GameObject controlWindow;

    // =========================
    // OptionPanel全体
    // =========================

    [Header("Main Option Panel")]

    // オプション全体のパネル
    // 後で閉じる処理に使える
    [SerializeField] GameObject optionPanel;

    // =========================
    // 背景暗転用
    // =========================

    [Header("Background")]

    // 背景を暗くするオブジェクト
    // オプション表示時だけONにする
    [SerializeField] GameObject darkBack;



    // =========================
    // 初期設定
    // =========================

    private void Start()
    {
        // 最初は全部閉じる
        CloseAll();

       
    }

    // =========================
    // 全ウィンドウを閉じる
    // =========================

    void CloseAll()
    {
        bgmWindow.SetActive(false);
        graphicWindow.SetActive(false);
        controlWindow.SetActive(false);
    }

    //


    // =========================
    // BGM設定を開く
    // =========================

    public void OpenBGM()
    {
        Debug.Log("OpenBGM");

        CloseAll();

        bgmWindow.SetActive(true);

        Debug.Log(bgmWindow.name);
    }

    // =========================
    // グラフィック設定を開く
    // =========================

    public void OpenGraphic()
    {
        CloseAll();

        graphicWindow.SetActive(true);
    }

    // =========================
    // 操作設定を開く
    // =========================

    public void OpenControl()
    {
        CloseAll();

        controlWindow.SetActive(true);
    }

    // =========================
    // オプションを閉じる
    // =========================

    public void CloseOption()
    {
        // オプションパネルを閉じる
        optionPanel.SetActive(false);

        // 背景暗転も消す
        darkBack.SetActive(false);
    }

    // =========================
    // オプションを開く
    // =========================

    public void OpenOption()
    {
        // オプション表示
        optionPanel.SetActive(true);

        // 背景暗転表示
        darkBack.SetActive(true);

        // 最初はBGM画面
        OpenBGM();
    }
}