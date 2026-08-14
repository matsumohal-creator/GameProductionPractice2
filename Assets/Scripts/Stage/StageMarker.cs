using System.Collections;
using UnityEngine;

// ここはステージマーカーを管理するクラスです。
// ステージマーカーは、現在のステージを示すUI要素であり、ステージボタンの位置に移動します。
// StageMarkerクラスは、ステージボタンの配列とマーカーのRectTransformを保持し、
// 指定されたステージインデックスに基づいてマーカーを移動させます。

public class StageMarker : MonoBehaviour
{
    [SerializeField] private RectTransform marker;
    [SerializeField] private StageButton[] stageButtons;
    [SerializeField] private float moveDuration = 0.4f;

    private Coroutine moveCoroutine;

    // ロード時は瞬間移動
    public void SetPositionImmediate(int stageId)
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

    // 選択時はスーッと移動
    public void MoveToStage(int stageId)
    {
        foreach (StageButton button in stageButtons)
        {
            if (button.StageData != null &&
                button.StageData.stageId == stageId)
            {
                if (moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }

                moveCoroutine =
                    StartCoroutine(MoveRoutine(button.transform.position));

                return;
            }
        }

        Debug.LogWarning($"StageMarker: stageId {stageId} が見つかりません");
    }

    private IEnumerator MoveRoutine(Vector3 targetPos)
    {
        Vector3 startPos = marker.position;
        float time = 0f;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / moveDuration);

            // 少し滑らかに
            t = Mathf.SmoothStep(0f, 1f, t);

            marker.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        marker.position = targetPos;
    }
}