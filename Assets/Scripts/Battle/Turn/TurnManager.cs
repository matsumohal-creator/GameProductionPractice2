using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    //シングルトン
    public static TurnManager Instance;

    //プレイヤーとエネミーのリストを保持
    private List<PlayerBase> players;
    private List<EnemyBase> enemies;

    private Queue<TurnUnit> turnQueue;

    private TurnUnit currentUnit;

    private void Awake()
    {
        // シングルトンの初期化
        Instance = this;
    }

    private void Start()
    {
        // Debug.Log(BattleManager.Instance);
        // BattleManagerからプレイヤーとエネミーのリストを取得
        players = BattleManager.Instance.Players;
        enemies = BattleManager.Instance.Enemies;

        // 最初のラウンド開始
        StartRound();
    }

    // ターンの開始
    public void StartRound()
    {
        List<TurnUnit> units = new List<TurnUnit>();

        Debug.Log("players = " + players.Count);
        Debug.Log("enemies = " + enemies.Count);

        foreach (PlayerBase player in players)
        {
            // Debug.Log(player);
            if (player == null)
            {
                Debug.LogError("Playerがnullです");
                continue;
            }

            if (player.CurrentHp > 0)
            {
                units.Add(new TurnUnit()
                {
                    isPlayer = true,
                    player = player
                });
            }
        }

        foreach (EnemyBase enemy in enemies)
        {
            //enemyの有無を確認
            if (enemy == null)
            {
                Debug.LogError("Enemyがnullです");
                continue;
            }

            if (enemy.CurrentHp > 0)
            {
                units.Add(new TurnUnit()
                {
                    isPlayer = false,
                    enemy = enemy
                });
            }
        }

        var sortedUnits =
            units.OrderByDescending(x => x.Speed)
            .ToList();

        turnQueue = new Queue<TurnUnit>(sortedUnits);

        NextTurn();
    }

    // ターンの終了
    public void EndTurn()
    {
        if (currentUnit == null)
            return;

        if (currentUnit.isPlayer)
        {
            currentUnit.player.OnTurnEnd();
        }
        else
        {
            currentUnit.enemy.OnTurnEnd();
        }

        IsWaitingPlayerInput = false;
        NextTurn();
    }

    // 次のターンに進む
    public void NextTurn()
    {
        if (turnQueue.Count == 0)
        {
            Debug.Log("ラウンド終了");
            StartRound();
            return;
        }

        // 現在のターンユニットを取得
        currentUnit = turnQueue.Dequeue();

        // ターンの開始処理
        if (currentUnit.isPlayer)
        {
            StartPlayerTurn(currentUnit.player);
            Debug.Log(currentUnit.player.name + " のターン");
        }
        else
        {
            StartEnemyTurn(currentUnit.enemy);
            Debug.Log(currentUnit.enemy.name + " のターン");
        }
    }

    // プレイヤーのターン処理
    private void StartPlayerTurn(PlayerBase player)
    {
        Debug.Log(player.name + " のターン開始");

        // エナジー回復
        player.RefillEnergy();

        // 手札取得
        HandManager hand =
            player.GetComponent<HandManager>();

        if (hand != null)
        {
            hand.DrawToFive();
        }

        // ターン開始時のステータス効果処理
        BattleManager.Instance.ChangeState(
         BattleState.PlayerInput);

        //↓playerターン処理
        IsWaitingPlayerInput = true;
    }

    // エネミーのターン処理
    private void StartEnemyTurn(EnemyBase enemy)
    {
        IsWaitingPlayerInput = false;
        //Debug.Log(enemy.name + " のターン開始");

        //AI処理
        //Coroutine化
        StartCoroutine(EnemyTurnRoutine(enemy));

    }

    // 現在のターンのプレイヤーを取得するプロパティ
    public PlayerBase CurrentPlayer
    {
        get
        {
            if (currentUnit == null) return null;

            if (!currentUnit.isPlayer) return null;

            return currentUnit.player;
        }
    }

    // 現在のターンのエネミーを取得するプロパティ
    public EnemyBase CurrentEnemy
    {
        get
        {
            if (currentUnit == null) return null;
            if (currentUnit.isPlayer) return null;
            return currentUnit.enemy;
        }
    }

    //
    public TurnUnit CurrentUnit => currentUnit;

    // プレイヤーの入力待ち状態を管理するプロパティ
    public bool IsWaitingPlayerInput { get; private set; }

    //別スクリプトのUI用
    public bool IsPlayerTurn
    {
        //TurnManager.Instance.EndTurn(); で次のターンになる
        get
        {
            if (currentUnit == null) return false;
            return currentUnit.isPlayer;
        }
    }

    // エネミーのターン処理をコルーチンで実行する
    private IEnumerator EnemyTurnRoutine(EnemyBase enemy)
    {
        Debug.Log(enemy.name + " のターン開始");

        //0.5秒待機
        yield return new WaitForSeconds(0.5f);

        // エネミーのターン処理を実行
        enemy.ExecuteTurn();

        //1.0秒待機
        yield return new WaitForSeconds(1.0f);

        // ターン終了処理
        EndTurn();
    }
}