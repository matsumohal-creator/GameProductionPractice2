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


   

    [Header("Debug Player")]
    [SerializeField]
    private PlayerBase playerPrefab;

    [Header("Debug Enemy")]
    [SerializeField]
    private EnemyBase enemyPrefab;


    //シングルトンの初期化
    private void Awake()
    {
        Instance = this;

        SpawnPlayers();
        SpawnEnemies();

    }

    // 初期化
    private void Start()
    {

        // partyMembers = GameManager.Instance.partyMembers;

        //
        //デッキの初期化
        InitializePlayerDecks();

    }

    public List<PlayerBase> Players => players;
    public List<EnemyBase> Enemies => enemies;

    // プレイヤーを生成
    private void SpawnPlayers()
    {

        Debug.Log("SpawnPlayers開始");

        GameObject obj = Instantiate(
       playerPrefab.gameObject,
       playerSpawnPoints[0],   // ← 1Pを親にする
       false                   // ワールド座標を維持しない
         );

        // 生成したGameObjectからPlayerBaseコンポーネントを取得
        PlayerBase player = obj.GetComponent<PlayerBase>();

        Debug.Log("生成したGameObject = " + obj);

        if (obj == null)
        {
            Debug.LogError("InstantiateしたPlayerがnull");
        }

        Debug.Log(obj);
        Debug.Log(player);

        // 生成したプレイヤーをリストに追加
        players.Add(player);
    }

    // エネミーを生成
    private void SpawnEnemies()
    {
        enemies.Clear();
       // Debug.Log("SpawnEnemies開始");

        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyPrefabが設定されていません");
            return;
        }

        GameObject obj = Instantiate(
      enemyPrefab.gameObject,
      enemySpawnPoints[0],   // ← Enemy側の1P(敵スロット)を親にする
      false                  // ローカル座標を使用
      );

        // Debug.Log("Enemy生成完了: " + obj.name);
        enemies.Add(obj.GetComponent<EnemyBase>());
        //Debug.Log("enemies数 = " + enemies.Count);
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


            //確認用のログ
            Debug.Log(player);
            Debug.Log(player.DefaultDeck);
            Debug.Log(deck);
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