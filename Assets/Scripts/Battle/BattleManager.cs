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

    //
    [Header("Battle Result")]
    [SerializeField]
    private BattleResultUI battleResultUI;

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

    [Header("Debug")]
    [SerializeField]
    private bool debugMode = false;

    [Header("Player Prefabs")]
    [SerializeField]
    private List<PlayerBase> debugPlayerPrefabs = new();

    [Header("Debug Enemy")]
    [SerializeField]
    private EnemyBase debugEnemyPrefab;


    //データ関連

    //ステージデータの参照
    private StageNodeData currentBattleStage;

    //ステージ検索
    [Header("Stage Data")]
    [SerializeField]
    private StageMapData stageMap;

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

        // デバッグモード
        if (debugMode)
        {
            SpawnDebugPlayers();
            return;
        }

        // 通常プレイ
        if (GameManager.IsBattleSetup)
        {
            SpawnSelectedPlayers();
            return;
        }

        Debug.LogError(
            "Battle準備がされていません");
    }

    //通常プレイ用
    private void SpawnSelectedPlayers()
    {
        // セーブデータ確認
        if (SaveManager.CurrentSave == null)
        {
            Debug.LogError(
                "SaveManager.CurrentSave が存在しません");
            return;
        }

        // パーティ編成確認
        if (SaveManager.CurrentSave.partyMembers == null ||
            SaveManager.CurrentSave.partyMembers.Count == 0)
        {
            Debug.LogError(
                "現在のパーティメンバーが存在しません");
            return;
        }

        // パーティメンバーを順番に生成
        for (int i = 0;
             i < SaveManager.CurrentSave.partyMembers.Count;
             i++)
        {
            // スポーンポイント確認
            if (i >= playerSpawnPoints.Length)
            {
                Debug.LogError(
                    "プレイヤーのスポーンポイントが足りません");
                break;
            }

            // パーティデータ取得
            PartyMemberData member =
                SaveManager.CurrentSave.partyMembers[i];

            if (member == null)
            {
                Debug.LogWarning(
                    $"PartyMember[{i}] がnullです");
                continue;
            }

            // キャラクター番号
            int index = member.characterIndex;

            // インデックス確認
            if (index < 0 || index >= playerPrefabs.Count)
            {
                Debug.LogError(
                    $"Player index が不正です: {index}");
                continue;
            }

            // プレハブ取得
            PlayerBase prefab =
                playerPrefabs[index];

            if (prefab == null)
            {
                Debug.LogError(
                    $"PlayerPrefab[{index}] がnullです");
                continue;
            }

            // プレイヤー生成
            GameObject obj = Instantiate(
                prefab.gameObject,
                playerSpawnPoints[i],
                false);

            // 位置をリセット
            obj.transform.localPosition = Vector3.zero;

            // PlayerBase取得
            PlayerBase player =
                obj.GetComponent<PlayerBase>();

            if (player == null)
            {
                Debug.LogError(
                    "生成したオブジェクトにPlayerBaseがありません");

                Destroy(obj);
                continue;
            }

            // BattleManagerのプレイヤーリストへ追加
            players.Add(player);

            Debug.Log(
                $"[Battle] Player生成: " +
                $"Index={index}, " +
                $"Name={player.CharacterName}");
        }
    }

    // デバッグ用
    private void SpawnDebugPlayers()
    {
        Debug.Log("===== DEBUG PLAYER SPAWN =====");

        for (int i = 0; i < debugPlayerPrefabs.Count; i++)
        {
            if (i >= playerSpawnPoints.Length)
            {
                Debug.LogError(
                    "デバッグ用プレイヤーのスポーンポイントが足りません");
                break;
            }

            PlayerBase prefab = debugPlayerPrefabs[i];

            if (prefab == null)
            {
                Debug.LogWarning(
                    $"DebugPlayerPrefabs[{i}] が設定されていません");
                continue;
            }

            GameObject obj = Instantiate(
                prefab.gameObject,
                playerSpawnPoints[i],
                false);

            obj.transform.localPosition = Vector3.zero;

            PlayerBase player =
                obj.GetComponent<PlayerBase>();

            if (player == null)
            {
                Debug.LogError(
                    "Debug PlayerにPlayerBaseがありません");
                continue;
            }

            players.Add(player);

            Debug.Log(
                $"DEBUG Player生成: {player.CharacterName}");
        }
    }

    // エネミーを生成
    private void SpawnEnemies()
    {
        enemies.Clear();

        // デバッグ
        if (debugMode)
        {
            SpawnDebugEnemies();
            return;
        }

        // 通常プレイ
        if (GameManager.IsBattleSetup)
        {
            SpawnSelectedEnemies();
            return;
        }

        Debug.LogError(
            "Battle準備がされていません");
    }

    private void SpawnSelectedEnemies()
    {
        if (SaveManager.CurrentSave == null)
        {
            Debug.LogError("SaveManager.CurrentSave がありません");
            return;
        }

        int stageId =
            SaveManager.CurrentSave.currentBattleStageId;

        currentBattleStage = GetStageById(stageId);

        if (currentBattleStage == null)
        {
            Debug.LogError(
                $"戦闘ステージが取得できません。ID={stageId}");
            return;
        }

        Debug.Log(
            $"===== Battle Stage =====\n" +
            $"ID : {currentBattleStage.stageId}\n" +
            $"Name : {currentBattleStage.stageName}");

        if (currentBattleStage.enemyPrefabs == null ||
            currentBattleStage.enemyPrefabs.Count == 0)
        {
            Debug.LogWarning(
                $"ステージ {currentBattleStage.stageName} に敵が設定されていません");

            return;
        }

        for (int i = 0;
             i < currentBattleStage.enemyPrefabs.Count;
             i++)
        {
            if (i >= enemySpawnPoints.Length)
            {
                Debug.LogError(
                    "敵のスポーンポイントが足りません");
                break;
            }

            GameObject prefab =
                currentBattleStage.enemyPrefabs[i];

            if (prefab == null)
            {
                Debug.LogWarning(
                    $"EnemyPrefab[{i}] がnullです");
                continue;
            }

            GameObject obj = Instantiate(
                prefab,
                enemySpawnPoints[i],
                false);

            obj.transform.localPosition = Vector3.zero;

            EnemyBase enemy =
                obj.GetComponent<EnemyBase>();

            if (enemy == null)
            {
                Debug.LogError(
                    $"生成した敵 {obj.name} にEnemyBaseがありません");

                Destroy(obj);
                continue;
            }

            enemies.Add(enemy);

            Debug.Log(
                $"敵生成: {enemy.CharacterName}");
        }
    }

    private void SpawnDebugEnemies()
    {
        Debug.Log("===== DEBUG ENEMY SPAWN =====");

        if (debugEnemyPrefab == null)
        {
            Debug.LogError(
                "Debug Enemy Prefabが設定されていません");
            return;
        }

        if (enemySpawnPoints.Length == 0)
        {
            Debug.LogError(
                "Enemy Spawn Pointがありません");
            return;
        }

        GameObject obj = Instantiate(
            debugEnemyPrefab.gameObject,
            enemySpawnPoints[0],
            false);

        obj.transform.localPosition = Vector3.zero;

        EnemyBase enemy =
            obj.GetComponent<EnemyBase>();

        if (enemy == null)
        {
            Debug.LogError(
                "Debug EnemyにEnemyBaseがありません");
            return;
        }

        enemies.Add(enemy);

        Debug.Log(
            $"DEBUG Enemy生成: {enemy.CharacterName}");
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
                Debug.LogError("{player.CharacterName} にDeckManagerがありません");
                continue;
            }

            //プレイヤーのデッキ
            Debug.Log("DefaultDeck = " + player.DefaultDeck);

            if (player.DefaultDeck == null)
            {
                Debug.LogError(
                    $"{player.CharacterName} のDefaultDeckが設定されていません"
                );

                continue;
            }

            deck.SetDeck(player.DefaultDeck.startDeck);

            Debug.Log(
             $"{player.CharacterName} : " +
             $"DrawPile枚数 = {deck.DrawPile.Count}"
         );


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

            // ターン進行を停止
            if (TurnManager.Instance != null)
            {
                TurnManager.Instance.StopBattle();
            }

            // リザルト表示
            if (battleResultUI != null)
            {
                battleResultUI.ShowVictory();
            }

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

            // ターン進行を停止
            if (TurnManager.Instance != null)
            {
                TurnManager.Instance.StopBattle();
            }

            // リザルト表示
            if (battleResultUI != null)
            {
                battleResultUI.ShowDefeat();
            }
        }
    }

    private void StopBattle()
    {
        Debug.Log("=== Battle Stop ===");

        // ターン進行を停止
        if (turnManager != null)
        {
            turnManager.StopBattle();
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
        {
            Debug.LogError("EnemyAttack:enemy か　target が null");
            return;
        }

        Debug.Log(
       $"[EnemyAttack開始] " +
       $"攻撃者={enemy.CharacterName} / " +
       $"対象={target.CharacterName} / " +
       $"攻撃力={attackPower} / " +
       $"対象HP={target.CurrentHp}");


        // 攻撃力を使ってダメージ計算
        enemy.PlayAttackAnimation();

        int damage =
            DamageCalculator.CalculateDamage(
                attackPower,
                enemy,
                target);

        // プレイヤーがダメージを受ける
        target.TakeDamage(damage, enemy);

        enemy.PlayIdleAnimation();

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

    ///セーブデータ関連

    // ステージIDからStageNodeDataを取得する
    private StageNodeData GetStageById(int stageId)
    {
        if (stageMap == null)
        {
            Debug.LogError("BattleManagerにStageMapDataが設定されていません");
            return null;
        }

        foreach (StageNodeData stage in stageMap.allStages)
        {
            if (stage == null)
            {
                continue;
            }

            if (stage.stageId == stageId)
            {
                return stage;
            }
        }

        Debug.LogError(
            $"stageId={stageId} のStageNodeDataが見つかりません");

        return null;
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