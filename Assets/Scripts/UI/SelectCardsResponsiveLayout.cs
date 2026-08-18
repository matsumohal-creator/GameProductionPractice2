using UnityEngine;
using UnityEngine.UI;

// SelectCardsScene のUI崩れを防ぐためのレイアウト補助。
// 1) CanvasScalerを基準解像度で統一
// 2) カードエリア(ボード)を一定アスペクトで中央に収める
public class SelectCardsResponsiveLayout : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private CanvasScaler canvasScaler;

    [Header("基準解像度")]
    [SerializeField] private Vector2 referenceResolution = new(1920f, 1080f);

    [Range(0f, 1f)]
    [SerializeField] private float matchWidthOrHeight = 0.5f;

    [Header("カード配置ボード")]
    [SerializeField] private RectTransform boardRoot;

    [SerializeField] private bool keepBoardAspect = true;
    [SerializeField] private float boardAspect = 16f / 9f;

    private Vector2Int lastScreenSize;

    private void Awake()
    {
        ApplyLayout();
    }

    private void OnEnable()
    {
        ApplyLayout();
    }

    private void Update()
    {
        Vector2Int current = new(Screen.width, Screen.height);

        // 解像度変更時に再適用（GameView変更や端末回転対応）
        if (current != lastScreenSize)
        {
            ApplyLayout();
        }
    }

    private void ApplyLayout()
    {
        lastScreenSize = new Vector2Int(Screen.width, Screen.height);

        ApplyCanvasScaler();
        ApplyBoardAspect();
    }

    private void ApplyCanvasScaler()
    {
        if (canvasScaler == null)
        {
            canvasScaler = GetComponentInParent<CanvasScaler>();
        }

        if (canvasScaler == null)
        {
            return;
        }

        // 画面サイズが変わってもUI全体の見え方を安定させる
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = referenceResolution;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = matchWidthOrHeight;
    }

    private void ApplyBoardAspect()
    {
        if (!keepBoardAspect || boardRoot == null)
        {
            return;
        }

        RectTransform parent = boardRoot.parent as RectTransform;

        if (parent == null)
        {
            return;
        }

        // ボードを常に中央固定
        boardRoot.anchorMin = new Vector2(0.5f, 0.5f);
        boardRoot.anchorMax = new Vector2(0.5f, 0.5f);
        boardRoot.pivot = new Vector2(0.5f, 0.5f);
        boardRoot.anchoredPosition = Vector2.zero;

        // 親領域内でアスペクト維持して最大サイズにフィット
        float parentWidth = parent.rect.width;
        float parentHeight = parent.rect.height;

        if (parentWidth <= 0f || parentHeight <= 0f)
        {
            return;
        }

        float fittedWidth = parentWidth;
        float fittedHeight = fittedWidth / boardAspect;

        if (fittedHeight > parentHeight)
        {
            fittedHeight = parentHeight;
            fittedWidth = fittedHeight * boardAspect;
        }

        boardRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fittedWidth);
        boardRoot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fittedHeight);
    }
}
