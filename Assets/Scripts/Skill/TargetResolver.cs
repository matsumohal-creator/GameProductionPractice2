using System.Collections.Generic;

// スキルのターゲットを解決するための静的クラス
// スキルのターゲットタイプに応じて、実際のターゲットをリストとして返す
// 例えば、EnemySingleなら単一のターゲットをリストにして返し、EnemyAllなら全ての敵をリストで返す
// AllySingleやAllyAllも同様に処理する
public static class TargetResolver
{
    public static List<IStatusEffectTarget> Resolve(
        SkillTargetType type,
        PlayerBase user,
        IStatusEffectTarget singleTarget,
        List<IStatusEffectTarget> allEnemies,
        List<IStatusEffectTarget> allAllies)
    {
        switch (type)
        {
            case SkillTargetType.EnemySingle:
            case SkillTargetType.AllySingle:
                return new List<IStatusEffectTarget> { singleTarget };

            case SkillTargetType.EnemyAll:
                return allEnemies;

            case SkillTargetType.AllyAll:
                return allAllies;

            case SkillTargetType.Self:
                return new List<IStatusEffectTarget> { user };

            default:
                return new List<IStatusEffectTarget>();
        }
    }
}