using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //シングルトンの初期化
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //パーティ編成
    [Header("Party")]
    public static List<int> selectedFlgs = new List<int>();

    //サウンドマネージャーの参照
    [Header("Sound")]
    [SerializeField]
    private SoundManager soundManager;

    public static SoundManager Sound => Instance?.soundManager;


    /// <summary>
    /// /////////
    /// </summary>
    //Battleのセットアップ


    // 今回の戦闘設定が存在するか
    public static bool IsBattleSetup { get; private set; }

    // 今回戦う敵のインデックス
    public static List<int> selectedEnemyFlags =
        new List<int>();

    // 戦闘を開始するための情報を設定
    public static void SetupBattle(
        List<int> playerFlags)
    {
        selectedFlgs.Clear();

        if (playerFlags != null)
        {
            selectedFlgs.AddRange(playerFlags);
        }

        IsBattleSetup = true;

        Debug.Log(
            $"[BattleSetup] Player={selectedFlgs.Count}");
    }
    // 戦闘設定を解除
    public static void ClearBattleSetup()
    {
        selectedFlgs.Clear();
        selectedEnemyFlags.Clear();

        IsBattleSetup = false;

        Debug.Log("BattleSetupをクリアしました");
    }

}

