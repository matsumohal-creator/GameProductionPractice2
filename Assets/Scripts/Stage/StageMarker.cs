using UnityEngine;

// ここはステージマーカーを管理するクラスです。
// ステージマーカーは、現在のステージを示すUI要素であり、ステージボタンの位置に移動します。
// StageMarkerクラスは、ステージボタンの配列とマーカーのRectTransformを保持し、
// 指定されたステージインデックスに基づいてマーカーを移動させます。

public class StageMarker : MonoBehaviour
{
    [SerializeField]
    private RectTransform marker;

    [SerializeField]
    private StageButton[] stageButtons;

    public void MoveToStage(int stageId)
    {
        foreach (StageButton button in stageButtons)
        {
            if (button.StageData != null &&
                button.StageData.stageId == stageId)
            {
                marker.position = button.transform.position;
                return;
            }
        }

        Debug.LogWarning($"StageMarker: stageId {stageId} が見つかりません");
    }
}