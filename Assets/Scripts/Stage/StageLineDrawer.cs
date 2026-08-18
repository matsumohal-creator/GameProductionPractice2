using UnityEngine;
using UnityEngine.UI;

// ステージとステージを結ぶ線を描画するクラス

public class StageLineDrawer : MonoBehaviour
{
    [Header("線の親")]
    [SerializeField] private RectTransform lineRoot;

    [Header("線プレハブ(Image)")]
    [SerializeField] private Image linePrefab;

    [Header("ステージボタン一覧")]
    [SerializeField] private StageButton[] stageButtons;

    [Header("マップデータ")]
    [SerializeField] private StageMapData stageMap;

    [Header("線の太さ")]
    [SerializeField] private float lineWidth = 6f;

    [Header("線の色")]
    [SerializeField] private Color clearedColor = new Color(1f, 0.75f, 0.2f);
    [SerializeField] private Color availableColor = Color.white;
    [SerializeField] private Color lockedColor = new Color(0.45f, 0.45f, 0.45f);

    private SaveData saveData;

    private void Start()
    {
        // CurrentSave を参照して同じステージ情報を使います。
        saveData = SaveManager.CurrentSave;

        DrawLines();
    }

    public void DrawLines()
    {
        // 既存ライン削除
        for (int i = lineRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(lineRoot.GetChild(i).gameObject);
        }

        foreach (StageNodeData fromStage in stageMap.allStages)
        {
            StageButton fromButton = FindButton(fromStage);
            if (fromButton == null) continue;

            foreach (StageNodeData toStage in fromStage.nextStages)
            {
                StageButton toButton = FindButton(toStage);
                if (toButton == null) continue;

                Color lineColor = GetLineColor(fromStage, toStage);

                CreateLine(
                    fromButton.transform.position,
                    toButton.transform.position,
                    lineColor);
            }
        }
    }

    // ステージ間の線の色を決定するメソッド
    private Color GetLineColor(StageNodeData fromStage, StageNodeData toStage)
    {
        bool fromCleared = saveData.clearedStageIds.Contains(fromStage.stageId);
        bool toCleared = saveData.clearedStageIds.Contains(toStage.stageId);

        // 通過済みルート
        if (fromCleared && toCleared)
        {
            return clearedColor;
        }

        // 現在地から進めるルート
        if (fromStage.stageId == saveData.currentStageId)
        {
            return availableColor;
        }

        // 未開放
        return lockedColor;
    }

    private StageButton FindButton(StageNodeData stage)
    {
        foreach (StageButton button in stageButtons)
        {
            if (button.StageData == stage)
            {
                return button;
            }
        }

        return null;
    }

    private void CreateLine(Vector3 worldStart, Vector3 worldEnd, Color lineColor)
    {
        RectTransform rootRect = lineRoot;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootRect,
            worldStart,
            null,
            out Vector2 start);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rootRect,
            worldEnd,
            null,
            out Vector2 end);

        Vector2 direction = end - start;
        float length = direction.magnitude;

        Image line = Instantiate(linePrefab, lineRoot);
        line.color = lineColor;

        RectTransform rect = line.rectTransform;
        rect.sizeDelta = new Vector2(length, lineWidth);
        rect.anchoredPosition = start + direction * 0.5f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rect.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}