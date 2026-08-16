using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Event Data")]
public class EventData : ScriptableObject
{
    [Header("イベントID")]
    public int eventId;

    [Header("イベントタイトル")]
    public string eventTitle;

    [Header("イベント本文")]
    [TextArea(3, 10)]
    public string eventText;

    [Header("選択肢")]
    public List<EventChoiceData> choices = new();

    [Header("イベント効果")]
    public List<EventEffectData> effects = new();
}