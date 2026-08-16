using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventChoiceData
{
    [Header("‘I‘ðŽˆ")]
    public string choiceText;

    [Header("Œ‹‰Ê")]
    [TextArea(2, 5)]
    public string resultText;

    [Header("‘I‘ðŽž‚ÌŒø‰Ê")]
    public List<EventEffectData> effects = new();
}