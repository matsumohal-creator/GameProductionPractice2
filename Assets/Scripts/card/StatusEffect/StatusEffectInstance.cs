
//　ここはステータス効果のインスタンスを表すクラスです。
//　ステータス効果のデータと、残りのターン数、スタック数を保持します。
//  ステータス効果のデータは、StatusEffectDataクラスで定義されているものを参照します。

[System.Serializable]
public class StatusEffectInstance
{
    public StatusEffectData data;
    // 現在あと何ターン残っているか
    public int remainingTurns;
    // 効果量
    public int stack;
}
