// ゲーム全体で使うセーブデータを管理するクラスです。
// 各シーンから同じ SaveData を参照できるようにします。

public static class SaveManager
{
    // CurrentSave の実体を保持します。
    private static SaveData currentSave;

    public static SaveData CurrentSave
    {
        get
        {
            // 未作成ならここで生成し、null 参照を防ぎます。
            if (currentSave == null)
            {
                currentSave = new SaveData();
            }

            return currentSave;
        }
        set => currentSave = value;
    }

    public static void Initialize()
    {
        // ゲーム開始時に CurrentSave を生成しておきます。
        _ = CurrentSave;
    }
}
