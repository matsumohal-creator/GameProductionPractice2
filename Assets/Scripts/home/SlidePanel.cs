using UnityEngine;
using System.Collections;

public class SlidePanel : MonoBehaviour
{
    private RectTransform rect;
    [SerializeField] private OptionManager optionManager;
    [SerializeField] private Vector2 hiddenPos;
    [SerializeField] private Vector2 showPos;
    [SerializeField] private float speed = 10f;

    private bool isOpen = false;

    public bool IsOpen => isOpen;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.anchoredPosition = hiddenPos;
        isOpen = false;
    }

    public void Toggle()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        if (isOpen)
        {
            return;
        }

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        StopAllCoroutines();
        StartCoroutine(MovePanel(showPos));
        isOpen = true;
    }

    public void Close()
    {
        if (!isOpen)
        {
            return;
        }

        optionManager.CloseOption();

        StopAllCoroutines();
        StartCoroutine(MovePanel(hiddenPos));
        isOpen = false;
    }

    IEnumerator MovePanel(Vector2 target)
    {
        while (Vector2.Distance(rect.anchoredPosition, target) > 0.1f)
        {
            rect.anchoredPosition = Vector2.Lerp(
                rect.anchoredPosition,
                target,
                Time.deltaTime * speed
            );

            yield return null;
        }

        rect.anchoredPosition = target;
    }
}