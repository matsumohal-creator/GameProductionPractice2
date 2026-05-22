using UnityEngine;
using System.Collections;

public class SlidePanel : MonoBehaviour
{
    private RectTransform rect;

    [SerializeField] private Vector2 hiddenPos;
    [SerializeField] private Vector2 showPos;

    [SerializeField] private float speed = 10f;

    private bool isOpen = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        rect.anchoredPosition = hiddenPos;
    }

    public void Toggle()
    {
        StopAllCoroutines();

        if (isOpen)
        {
            StartCoroutine(MovePanel(hiddenPos));
        }
        else
        {
            StartCoroutine(MovePanel(showPos));
        }

        isOpen = !isOpen;
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