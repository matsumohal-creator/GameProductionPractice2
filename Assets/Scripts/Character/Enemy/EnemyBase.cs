using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IStatusEffectTarget
{
    [Header("Base Status")]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int currentHp = 100;
    [SerializeField] private int shield = 0;
    [Header("Status Effect Master")]
    [SerializeField] private List<StatusEffectData> statusEffectMaster = new List<StatusEffectData>();

    [Header("Skills")]
    [SerializeField] private List<SkillData> skills = new List<SkillData>();

    private readonly Dictionary<StatusEffectType, StatusEffectData> statusEffectLookup = new Dictionary<StatusEffectType, StatusEffectData>();
    private readonly Dictionary<StatusEffectType, StatusEffectInstance> activeStatusEffects = new Dictionary<StatusEffectType, StatusEffectInstance>();

    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;
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

    // シールドを加算する
    public void GainShield(int amount)
    {
        shield += Mathf.Max(0, amount);
    }

    // ダメージを受ける
    public void TakeDamage(int amount)
    {
        amount = Mathf.Max(0, amount);

        // シールドで吸収
        if (shield > 0)
        {
            int blocked = Mathf.Min(shield, amount);

            shield -= blocked;
            amount -= blocked;
        }

        // 残ったダメージをHPへ
        currentHp = Mathf.Max(0, currentHp - amount);
    }

    // シールド無視ダメージ
    public void TakeDirectDamage(int amount)
    {
        currentHp = Mathf.Max(0, currentHp - Mathf.Max(0, amount));
    }

    // HPを回復する
    public void Heal(int amount)
    {
        currentHp = Mathf.Min(maxHp, currentHp + Mathf.Max(0, amount));
    }

    // ターン終了時処理
    public void OnTurnEnd()
    {
        TickStatusEffects();
        // ターン終了時にシールドをリセット
        shield = 0;
    }

    // ターン進行時の状態異常処理を行う
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
