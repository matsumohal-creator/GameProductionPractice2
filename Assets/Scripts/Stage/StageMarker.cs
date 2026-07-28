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

    // 指定されたステージインデックスに基づいてマーカーを移動させるメソッド
    public void MoveToStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stageButtons.Length)
            return;

        marker.position = stageButtons[stageIndex].transform.position;
    }
}