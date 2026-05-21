using UnityEngine;
using System.Collections;

public class SlidePanel : MonoBehaviour
{
    RectTransform rect;

    [SerializeField] Vector2 hiddenPos;
    [SerializeField] Vector2 showPos;

    [SerializeField] float speed = 10f;

    bool isOpen = false;

    Coroutine moveCoroutine;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.anchoredPosition = hiddenPos;
    }

    public void Toggle()
    {
        if (isOpen)
        {
            Move(hiddenPos);
        }
        else
        {
            Move(showPos);
        }

        isOpen = !isOpen;
    }

    void Move(Vector2 target)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(Slide(target));
    }

    IEnumerator Slide(Vector2 target)
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