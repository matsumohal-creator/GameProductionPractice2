using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;

    //プレイヤーベース
    [Header("Players Base")]
    [SerializeField]
    private List<PlayerBase> players = new();

    //エネミーベース
    [Header("Enemies Base")]
    [SerializeField]
    private List<EnemyBase> enemies = new();


    //スポーン位置

    // プレイヤーのスポーン位置
    [Header("Player Spawn Points")]
    [SerializeField]
    private Transform[] playerSpawnPoints;

    // エネミーのスポーン位置
    [Header("Enemy Spawn Points")]
    [SerializeField]
    private Transform[] enemySpawnPoints;



    //プレハブ（今後消す）
    [Header("Test Player Prefabs")]
    [SerializeField]
    private List<PlayerBase> testPlayerPrefabs = new();

    [Header("Test Enemy Prefabs")]
    [SerializeField]
    private List<EnemyBase> testEnemyPrefabs = new();
    //消す

    //シングルトンの初期化
    private void Awake()
    {
        Instance = this;
    }

    // 初期化
    private void Start()
    {
        //
        SpawnPlayers();
        SpawnEnemies();

        //デッキの初期化
        InitializePlayerDecks();
    }

    public List<PlayerBase> Players => players;
    public List<EnemyBase> Enemies => enemies;

    // プレイヤーを生成
    private void SpawnPlayers()
    {
        players.Clear();

        for (int i = 0; i < testPlayerPrefabs.Count; i++)
        {
            if (i >= playerSpawnPoints.Length)
                break;

            PlayerBase player =
                Instantiate(
                    testPlayerPrefabs[i],
                    playerSpawnPoints[i].position,
                    Quaternion.identity);

            players.Add(player);
        }
    }

    // エネミーを生成
    private void SpawnEnemies()
    {
        enemies.Clear();
        for (int i = 0; i < testEnemyPrefabs.Count; i++)
        {
            if (i >= enemySpawnPoints.Length)
                break;
            EnemyBase enemy =
                Instantiate(
                    testEnemyPrefabs[i],
                    enemySpawnPoints[i].position,
                    Quaternion.identity);
            enemies.Add(enemy);
        }
    }

    // プレイヤーのデッキを初期化
    private void InitializePlayerDecks()
    {
        foreach (PlayerBase player in players)
        {
            DeckManager deck =
                player.GetComponent<DeckManager>();

            if (deck == null)
                continue;

            deck.SetDeck(player.DefaultDeck.startDeck);
        }
    }

    //スキルの使用
    public void UseSkill(
    PlayerBase user,
    SkillData skill,
    IStatusEffectTarget singleTarget)
    {
        // ターゲットを指定
        List<IStatusEffectTarget> enemyTargets =
            new List<IStatusEffectTarget>();

        // ターゲットのベクトル
        List<IStatusEffectTarget> allyTargets =
            new List<IStatusEffectTarget>();

        // 敵と味方のターゲットを追加
        foreach (EnemyBase enemy in enemies)
        {
            enemyTargets.Add(enemy);
        }

        foreach (PlayerBase player in players)
        {
            allyTargets.Add(player);
        }

        // ターゲットの解決
        List<IStatusEffectTarget> targets =
            TargetResolver.Resolve(
                skill.targetType,
                user,
                singleTarget,
                enemyTargets,
                allyTargets);

        // スキルの実行
        SkillExecution.ExecuteSkill(
            user,
            targets,
            skill);

        // 戦闘結果の確認
        CheckBattleResult();
    }

    // 戦闘結果の確認
    private void CheckBattleResult()
    {
        // 全ての敵が倒されているか確認
        bool allEnemiesDead = true;

        // 敵のHPを確認
        foreach (EnemyBase enemy in enemies)
        {
            // 敵のHPが0より大きい場合、全ての敵が倒されていないと判断
            if (enemy.CurrentHp > 0)
            {
                allEnemiesDead = false;
                break;
            }
        }

        // 全ての敵が倒されている場合、勝利と判断
        if (allEnemiesDead)
        {
            Debug.Log("Victory");
            return;
        }

        // 全てのプレイヤーが倒されているか確認
        bool allPlayersDead = true;

        // プレイヤーのHPを確認
        foreach (PlayerBase player in players)
        {
            // プレイヤーのHPが0より大きい場合、全てのプレイヤーが倒されていないと判断
            if (player.CurrentHp > 0)
            {
                allPlayersDead = false;
                break;
            }
        }

        // 全てのプレイヤーが倒されている場合、敗北と判断
        if (allPlayersDead)
        {
            Debug.Log("Defeat");
        }
    }
}