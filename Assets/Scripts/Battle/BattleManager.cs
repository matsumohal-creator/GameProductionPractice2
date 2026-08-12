using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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

    //scriptの取得

    //ターンマネージャー
    [Header("TurnManager")]
    [SerializeField]
    private TurnManager turnManager;


    //プレイヤーUIマネージャー
    [SerializeField]
    private PlayerUIManager playerUIManager;


    // EnemyUI
    [SerializeField]
    private EnemyUIManager enemyUIManager;

    //スポーン位置

    // プレイヤーのスポーン位置
    [Header("Player Spawn Points")]
    [SerializeField]
    private Transform[] playerSpawnPoints;

    // エネミーのスポーン位置
    [Header("Enemy Spawn Points")]
    [SerializeField]
    private Transform[] enemySpawnPoints;

    // プレイヤーのプレハブ
    [Header("Player Prefabs")]
    [SerializeField]
    private List<PlayerBase> playerPrefabs = new();

    //デバック用・後で消す


    [Header("Debug Enemy")]
    [SerializeField]
    private EnemyBase enemyPrefab;




    //シングルトンの初期化
    private void Awake()
    {
        Instance = this;

    }

    // 初期化
    private void Start()
    {
        SpawnPlayers();
        SpawnEnemies();

        // partyMembers = GameManager.Instance.partyMembers;

        //
        //デッキの初期化
        InitializePlayerDecks();

        //UIManagerの初期化
        playerUIManager.CreateUI(players);
        enemyUIManager.CreateUI(enemies);

        //BattleStateをBattleStartに移行
        ChangeState(BattleState.BattleStart);

        //ターンの開始
        turnManager.StartRound();

       

    }

    //Battleの司令塔
    // バトルの状態を管理するプロパティ
    public BattleState CurrentState { get; private set; }

    public void ChangeState(BattleState newState)
    {
        CurrentState = newState;

        if (BattleUIManager.Instance != null)
        {
            BattleUIManager.Instance.RefreshUI(newState);
        }

       // Debug.Log("現在の状態：" + newState);
    }

    // プレイヤーとエネミーのリストを取得するプロパティ
    public List<PlayerBase> Players => players;
    public List<EnemyBase> Enemies => enemies;

    // プレイヤーを生成
    private void SpawnPlayers()
    {
        players.Clear();
        // Debug.Log("SpawnPlayers開始");

        // デバッグ用
        if (GameManager.selectedFlgs.Count == 0)
        {
            GameManager.selectedFlgs.Add(0);
            GameManager.selectedFlgs.Add(1);
            GameManager.selectedFlgs.Add(2);
            GameManager.selectedFlgs.Add(3);

        }


        for (int i = 0; i < GameManager.selectedFlgs.Count; i++)
        {
            //
            int index = GameManager.selectedFlgs[i];
            if (index < 0 || index >= playerPrefabs.Count)
            {
                continue;
            }

            //
            PlayerBase prefab = playerPrefabs[index];

            //プレイヤーの生成
            GameObject obj = Instantiate(
           prefab.gameObject,
           playerSpawnPoints[i],
           false);
            if (i >= playerSpawnPoints.Length)
            {
                Debug.LogError("スポーンポイントが足りません");
                break;
            }

            // 生成したプレイヤーの位置リセット
            obj.transform.localPosition = Vector3.zero;
           

            //playerベースの取得
            PlayerBase player = obj.GetComponent<PlayerBase>();
            if (obj == null)
            {
                Debug.LogError("InstantiateしたPlayerがnull");
            }

            // 生成したプレイヤーをリストに追加
            players.Add(player);
        }

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
            //デッキマネージャーのデッキ
            DeckManager deck =
                player.GetComponent<DeckManager>();

            //デッキの有無の確認
            if (deck == null)
            {
                Debug.LogError("DeckManagerがありません");
                continue;
            }

            //プレイヤーのデッキ
            Debug.Log("DefaultDeck = " + player.DefaultDeck);

            if (player.DefaultDeck != null)
            {
                Debug.Log("デッキ枚数 = " + player.DefaultDeck.startDeck.Count);
            }

            deck.SetDeck(player.DefaultDeck.startDeck);

            Debug.Log("DrawPile枚数 = " + deck.DrawPile.Count);


        }
    }



    //スキルの使用
    public void UseSkill(
    PlayerBase user,
    SkillData skill,
    IStatusEffectTarget singleTarget)
    {
        //
       Debug.Log(
            $"[CardDebug] UseSkill開始 " +
            $"User={user.CharacterName}, " +
            $"Skill={skill.skillName}, " +
            $"Target={singleTarget}"
        );

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

        //
            Debug.Log(
        $"[CardDebug] ターゲット解決完了: {targets.Count}体");

            foreach (IStatusEffectTarget target in targets)
            {
                Debug.Log(
                    $"[CardDebug] Target = {target}"
                );
            }

        // スキルの実行
        SkillExecution.ExecuteSkill(
            user,
            targets,
            skill);

        //カードの実行
        Debug.Log(
    　　　$"[CardDebug] SkillExecution.ExecuteSkill 実行");
        // UI更新
        playerUIManager.RefreshAll();
        enemyUIManager.RefreshAll();


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
            ChangeState(BattleState.Victory);
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
            ChangeState(BattleState.Defeat);
            Debug.Log("Defeat");
        }
    }

    //敵の攻撃
    // 敵の攻撃
    public void EnemyAttack(
        EnemyBase enemy,
        PlayerBase target,
        int attackPower)
    {
        if (enemy == null || target == null)
            return;

        // 攻撃力を使ってダメージ計算
        int damage =
            DamageCalculator.CalculateDamage(
                attackPower,
                enemy,
                target);

        // プレイヤーがダメージを受ける
        target.TakeDamage(damage, enemy);

        // UI更新
        playerUIManager.RefreshAll();
        enemyUIManager.RefreshAll();

        // 戦闘結果確認
        CheckBattleResult();

        //敵の行動のログ確認
        Debug.Log(
            $"{enemy.CharacterName} が {target.CharacterName} に {damage} ダメージ");
    }


    public PlayerBase GetRandomLivingPlayer()
    {
        List<PlayerBase> candidates = new();

        foreach (PlayerBase player in players)
        {
            if (player.CurrentHp > 0)
            {
                candidates.Add(player);
            }
        }

        if (candidates.Count == 0)
            return null;

        return candidates[
            Random.Range(0, candidates.Count)];
    }

    // 指定したプレイヤー以外の生存している味方をランダムで返す
    // 今は味方にダメージをそらすときに使用する
    public PlayerBase GetRandomLivingPlayerExcept(PlayerBase target)
    {
        List<PlayerBase> candidates = new();

        foreach (PlayerBase player in players)
        {
            // 自分自身は除外
            if (player == target)
            {
                continue;
            }

            // 戦闘不能は除外
            if (player.CurrentHp <= 0)
            {
                continue;
            }

            candidates.Add(player);
        }

        // 候補がいなければnull
        if (candidates.Count == 0)
        {
            return null;
        }

        int index = Random.Range(0, candidates.Count);

        return candidates[index];
    }
}