using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IStatusEffectTarget
{
    [Header("Base Status")]
    [SerializeField] private string characterName;
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int currentHp = 100;
    [SerializeField] private int maxEnergy = 3;
    [SerializeField] private int currentEnergy = 3;
    [SerializeField] private int shield = 0;
    [SerializeField] private int speed = 0; //追加したよ



    [Header("Status Effect Master")]
    [SerializeField] private List<StatusEffectData> statusEffectMaster = new List<StatusEffectData>();

    [Header("Skills")]
    [SerializeField] private List<SkillData> skills = new List<SkillData>();

    private readonly Dictionary<StatusEffectType, StatusEffectData> statusEffectLookup = new Dictionary<StatusEffectType, StatusEffectData>();
    private readonly Dictionary<StatusEffectType, StatusEffectInstance> activeStatusEffects = new Dictionary<StatusEffectType, StatusEffectInstance>();

    // 以下を追加しました
    [Header("Character Class")]
    [SerializeField]
    private CharacterClass characterClass;// 各プレイヤーのクラスを指定するフィールド
    public CharacterClass CharacterClass => characterClass;// キャラクタークラスは、プレイヤーの役割やスキルセットを定義するためのものです。

    [Header("Default Deck")]
    [SerializeField]
    private CharacterDeckData defaultDeck;// 各プレイヤーのデフォルトデッキを指定するフィールド
    public CharacterDeckData DefaultDeck => defaultDeck;// デフォルトデッキは、プレイヤーのクラスに応じた初期カードセットを提供するためのものです。

    [Header("Icon")]
    [SerializeField]
    private Sprite icon;

    public Sprite Icon => icon;

    [Header("UI")]
    [SerializeField]
    private Vector3 uiOffset = new Vector3(0, 2.0f, 0);

    public Vector3 UIOffset => uiOffset;


    public string CharacterName => characterName;
    public int Speed => speed; // 追加したよ
    public int CurrentHp => currentHp; 
    public int MaxHp => maxHp;
    public int CurrentEnergy => currentEnergy;
    public int MaxEnergy => maxEnergy;
    public int Shield => shield;
    public IReadOnlyCollection<StatusEffectInstance> ActiveStatusEffects => activeStatusEffects.Values;
    public IReadOnlyList<SkillData> Skills => skills;

    public event Action<StatusEffectInstance> StatusEffectApplied;
    public event Action<StatusEffectInstance> StatusEffectRemoved;

    // 初期値を補正し、状態異常マスターを参照テーブル化する
    protected virtual void Awake()
    {
        maxHp = Mathf.Max(1, maxHp);
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        maxEnergy = Mathf.Max(0, maxEnergy);
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        BuildStatusEffectLookup();
    }

    // Enumと状態異常データの対応表を作成する
    public void BuildStatusEffectLookup()
    {
        statusEffectLookup.Clear();

        foreach (StatusEffectData data in statusEffectMaster)
        {
            if (data == null)
            {
                continue;
            }

            statusEffectLookup[data.effectType] = data;
        }
    }

    // Enumから状態異常データを取得する
    public bool TryGetStatusEffectData(StatusEffectType type, out StatusEffectData data)
    {
        return statusEffectLookup.TryGetValue(type, out data);
    }

    // Enum指定で状態異常を付与する
    public void ApplyStatusEffect(StatusEffectType type, int duration, int stack = 1)
    {
        if (!TryGetStatusEffectData(type, out StatusEffectData data))
        {
            return;
        }

        ApplyStatusEffect(data, duration, stack);
    }

    // 状態異常データを直接指定して付与する
    public void ApplyStatusEffect(StatusEffectData statusData, int duration, int stack = 1)
    {
        if (statusData == null)
        {
            return;
        }

        int clampedStack = Mathf.Max(1, stack);
        int maxStackValue = Mathf.Max(1, statusData.maxStack);
        int clampedDuration = Mathf.Max(1, duration);

        if (activeStatusEffects.TryGetValue(statusData.effectType, out StatusEffectInstance instance))
        {
            instance.stack = Mathf.Clamp(instance.stack + clampedStack, 1, maxStackValue);
            instance.remainingTurns = Mathf.Max(instance.remainingTurns, clampedDuration);
            OnStatusEffectApplied(instance);
            return;
        }

        var newInstance = new StatusEffectInstance
        {
            data = statusData,
            remainingTurns = clampedDuration,
            stack = Mathf.Clamp(clampedStack, 1, maxStackValue)
        };

        activeStatusEffects[statusData.effectType] = newInstance;
        OnStatusEffectApplied(newInstance);
    }

    // 指定状態異常を保持しているか確認する
    public bool HasStatusEffect(StatusEffectType type)
    {
        return activeStatusEffects.ContainsKey(type);
    }

    // 指定状態異常のスタック数を返す
    public int GetStatusStack(StatusEffectType type)
    {
        if (!activeStatusEffects.TryGetValue(type, out StatusEffectInstance instance))
        {
            return 0;
        }

        return instance.stack;
    }

    // 指定状態異常を取得する。存在しない場合はfalseを返す。
    public bool TryGetStatusEffect(
    StatusEffectType type,
    out StatusEffectInstance instance)
    {
        return activeStatusEffects.TryGetValue(type, out instance);
    }

    // 指定状態異常を解除する
    public void RemoveStatusEffect(StatusEffectType type)
    {
        if (!activeStatusEffects.TryGetValue(type, out StatusEffectInstance instance))
        {
            return;
        }

        activeStatusEffects.Remove(type);
        OnStatusEffectRemoved(instance);
    }

    // 全状態異常を解除する
    public void ClearStatusEffects()
    {
        List<StatusEffectType> keys = new List<StatusEffectType>(activeStatusEffects.Keys);
        foreach (StatusEffectType key in keys)
        {
            RemoveStatusEffect(key);
        }
    }

    // HPを直接設定する
    public void SetHp(int value)
    {
        currentHp = Mathf.Clamp(value, 0, maxHp);
    }

    // エナジーを直接設定する
    public void SetEnergy(int value)
    {
        currentEnergy = Mathf.Clamp(value, 0, maxEnergy);
    }

    // エナジーを最大まで回復する
    public void RefillEnergy()
    {
        currentEnergy = maxEnergy;
    }

    // エナジー消費を試み、成功可否を返す
    public bool TryUseEnergy(int cost)
    {
        int clampedCost = Mathf.Max(0, cost);
        if (currentEnergy < clampedCost)
        {
            return false;
        }

        currentEnergy -= clampedCost;
        return true;
    }

    // エナジーを加算する
    public void GainEnergy(int amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + Mathf.Max(0, amount), 0, maxEnergy);
    }

    // シールドを加算する
    public void GainShield(int amount)
    {
        shield += Mathf.Max(0, amount);
    }

    // ダメージを受ける
    public void TakeDamage(int amount, IStatusEffectTarget attacker = null)
    {
        amount = Mathf.Max(0, amount);

        if (shield > 0)
        {
            int blocked = Mathf.Min(shield, amount);

            shield -= blocked;
            amount -= blocked;
        }

        int beforeHp = currentHp;
        currentHp = Mathf.Max(0, currentHp - amount);

        if (currentHp < beforeHp)
        {
            GameManager.Sound?.PlayDamageSE();
        }

        // 被弾時の状態異常処理
        OnDamaged(attacker);
    }

    // 状態異常によるダメージのそらしを考慮してダメージを受ける
    public void ReceiveDamage(int amount, IStatusEffectTarget attacker = null)
    {
        PlayerBase redirect =
            StatusEffectManager.RedirectDamageTarget(this);

        if (redirect != this)
        {
            redirect.TakeDamage(amount, attacker);
            return;
        }

        TakeDamage(amount, attacker);
    }

    public void OnDamaged(IStatusEffectTarget attacker)
    {
        StatusEffectManager.OnDamaged(this, attacker);
    }

    // シールド無視ダメージ
    public void TakeDirectDamage(int amount)
    {
        currentHp = Mathf.Max(0, currentHp - Mathf.Max(0, amount));
    }

    // 自身のHPを消費する（最低1HPは残す）
    public int ConsumeHpPercent(int percent)
    {
        percent = Mathf.Clamp(percent, 0, 100);

        // 消費量
        int consume = maxHp * percent / 100;

        // 現在HP-1までしか減らせない
        consume = Mathf.Min(consume, currentHp - 1);

        // HP1なら消費できない
        if (consume <= 0)
        {
            return 0;
        }

        currentHp -= consume;

        // 実際に減った値を返す
        return consume;
    }

    // HPを回復する
    public void Heal(int amount)
    {
        // 回復量は0以上に補正
        amount = Mathf.Max(0, amount);
        // HealBoost状態異常がある場合、回復量を1.5倍にする
        int healBoost = GetStatusStack(StatusEffectType.HealBoost);

        if (healBoost > 0)
        {
            amount = Mathf.RoundToInt(amount * 1.5f);
        }
        // 回復量を最大HPを超えないように補正
        currentHp = Mathf.Min(maxHp, currentHp + amount);
    }

    // ターン終了時処理
    public void OnTurnEnd()
    {
        TickStatusEffects();

        // ターン終了時にシールドをリセット
        shield = 0;
    }

    // 状態異常のターン経過処理を行う
    public void TickStatusEffects()
    {
        List<StatusEffectType> expired = new List<StatusEffectType>();

        foreach (KeyValuePair<StatusEffectType, StatusEffectInstance> pair in activeStatusEffects)
        {
            StatusEffectManager.ApplyTurnTick(this, pair.Value);
            pair.Value.remainingTurns--;

            if (pair.Value.remainingTurns <= 0 || pair.Value.stack <= 0)
            {
                expired.Add(pair.Key);
            }
        }

        foreach (StatusEffectType type in expired)
        {
            RemoveStatusEffect(type);
        }
    }

    // 状態異常付与時の通知フック
    protected virtual void OnStatusEffectApplied(StatusEffectInstance instance)
    {
        StatusEffectApplied?.Invoke(instance);
    }

    // 状態異常解除時の通知フック
    protected virtual void OnStatusEffectRemoved(StatusEffectInstance instance)
    {
        StatusEffectRemoved?.Invoke(instance);
    }
}