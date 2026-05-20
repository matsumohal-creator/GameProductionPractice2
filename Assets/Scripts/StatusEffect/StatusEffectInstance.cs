
//　ここはステータス効果のインスタンスを表すクラスです。
//　ステータス効果のデータと、残りのターン数、スタック数を保持します。
//  ステータス効果のデータは、StatusEffectDataクラスで定義されているものを参照します。

[System.Serializable]
public class StatusEffectInstance
{
    public StatusEffectData data;

    public int remainingTurns;

    public int stack;
}
