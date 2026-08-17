// このクラスは、ゲームのセーブデータを管理するためのシングルトン的なクラスです。
// SaveDataクラスのインスタンスを保持し、ゲームの進行状況を保存および読み込むために使用されます。

public static class SaveManager
{
    public static SaveData CurrentSave;

    public static void Initialize()
    {
        if (CurrentSave != null)
        {
            return;
        }

        CurrentSave = new SaveData();

    }
}