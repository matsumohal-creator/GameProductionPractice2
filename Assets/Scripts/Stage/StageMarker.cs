using UnityEngine;

public class StageMarker : MonoBehaviour
{
    [SerializeField]
    private RectTransform marker;

    [SerializeField]
    private StageButton[] stageButtons;

    public void MoveToStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stageButtons.Length)
            return;

        marker.position = stageButtons[stageIndex].transform.position;
    }
}