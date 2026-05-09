using UnityEngine;

// スキルデータベースを管理するクラス
// スキルデータベースは、ゲーム内で使用されるすべてのスキルを管理するためのクラスです。
// スキルデータベースは、ScriptableObjectとして実装されており、Unityのエディタ上で簡単にスキルデータを追加・編集することができます。

[CreateAssetMenu(menuName = "Game/SkillDatabase")] 
public class SkillDatabase : ScriptableObject
{
    public SkillData[] skills; 
}
