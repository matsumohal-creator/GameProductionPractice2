using UnityEngine;
using System.Collections;

public class SlidePanel : MonoBehaviour
{
    [SerializeField] RectTransform panel;

    [SerializeField] Vector2 hiddenPos;
    [SerializeField] Vector2 showPos;

    [SerializeField] float speed = 10f;

    bool isOpen = false;

    private void Start()
    {
        panel.anchoredPosition = hiddenPos;
    }

    public void TogglePanel()
    {
        StopAllCoroutines();

        if (isOpen)
        {
            StartCoroutine(Slide(hiddenPos));
        }
        else
        {
            StartCoroutine(Slide(showPos));
        }

        isOpen = !isOpen;
    }

    IEnumerator Slide(Vector2 target)
    {
        while (Vector2.Distance(panel.anchoredPosition, target) > 0.1f)
        {
            panel.anchoredPosition = Vector2.Lerp(
                panel.anchoredPosition,
                target,
                Time.deltaTime * speed
            );

            yield return null;
        }

        panel.anchoredPosition = target;
    }
}