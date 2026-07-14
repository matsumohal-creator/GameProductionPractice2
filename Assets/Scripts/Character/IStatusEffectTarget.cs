public interface IStatusEffectTarget
{
    // 状態異常処理から呼ばれるダメージ受け取り口
    void TakeDamage(int amount);

    // 状態異常処理から呼ばれる回復受け取り口
    void Heal(int amount);

    // 最大HPを取得するプロパティ。状態異常処理から呼ばれる。
    int MaxHp { get; }

    // 状態異常処理から呼ばれる状態異常付与口
    void ApplyStatusEffect(
        StatusEffectData statusData,
        int duration,
        int stack);

    // 状態異常処理から呼ばれる状態異常解除口
    void RemoveStatusEffect(StatusEffectType type);

    // 状態異常処理から呼ばれる受け取り口
    int GetStatusStack(StatusEffectType type);

    // 状態異常処理から呼ばれる受け取り口。状態異常を取得し、存在しない場合はnullを返す。
    bool TryGetStatusEffect(
    StatusEffectType type,
    out StatusEffectInstance instance);

    // シールドを獲得
    void GainShield(int amount);

    // シールドを無視して直接HPへダメージ
    void TakeDirectDamage(int amount);
}
