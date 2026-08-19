using UnityEngine;

public class PartyHpUI : MonoBehaviour
{
    [Header("キャラクターHP表示")]
    [SerializeField]
    private PartyHpSlotUI[] hpSlots;

    [Header("キャラクターアイコン")]
    [SerializeField]
    private Sprite[] characterIcons;

    private void Start()
    {
        Refresh();
    }

    // HP表示を更新
    public void Refresh()
    {
        SaveData saveData = SaveManager.CurrentSave;

        if (saveData == null)
        {
            Debug.LogWarning("SaveDataが存在しません");
            return;
        }

        if (saveData.partyMembers == null)
        {
            Debug.LogWarning("パーティメンバーが存在しません");
            return;
        }

        for (int i = 0; i < hpSlots.Length; i++)
        {
            // パーティ人数よりスロットが多い場合
            if (i >= saveData.partyMembers.Count)
            {
                hpSlots[i].gameObject.SetActive(false);
                continue;
            }

            PartyMemberData member = saveData.partyMembers[i];

            if (member == null)
            {
                hpSlots[i].gameObject.SetActive(false);
                continue;
            }

            hpSlots[i].gameObject.SetActive(true);

            Sprite icon = null;

            if (member.characterIndex >= 0 &&
                member.characterIndex < characterIcons.Length)
            {
                icon = characterIcons[member.characterIndex];
            }

            hpSlots[i].Setup(member, icon);
        }
    }
}