using UnityEngine;

public enum BattleState
{
    BattleStart,      //戦闘開始

    TurnStart,        //ターン開始

    Effect,            //演出処理中

    Action,            //行動処理中

    PlayerInput,      //プレイヤー入力待ち

    EnemyAction,      //敵行動中

    TurnEnd,          //ターン終了

    Victory,          //勝利

    Defeat            //敗北
}