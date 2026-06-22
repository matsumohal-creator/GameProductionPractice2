using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditScene : MonoBehaviour
{
    // Inspectorで設定するキャラクター画像一覧（Character0?Character5を想定）
    [SerializeField]
    private List<Image> characterImages = new();

    // 編成済みキャラをどの程度暗く表示するか（1に近いほど明るい）
    [SerializeField, Range(0.1f, 1f)]
    private float selectedBrightness = 0.7f;

    // シーンをまたいでも保持する編成状態（0:未編成 / 1:編成済み）
    private static readonly List<int> selectedState = new();

    // 現在シーンで使う編成状態（表示反映用）
    private List<int> selectedFlg = new();

    // 編成可能な最大人数
    [SerializeField]
    private int maxSelectedCount = 4;

    private void Awake()
    {
        // 画像数に合わせて編成状態リストのサイズを調整
        EnsureStateSize(characterImages.Count);

        // static状態をローカル状態へコピー
        selectedFlg = new List<int>(selectedState);

        // 起動時に全キャラクターの見た目を状態に合わせて更新
        for (int i = 0; i < characterImages.Count; i++)
        {
            ApplyCharacterVisual(i);
        }
    }

    // 編成状態リストの要素数をキャラ数に合わせる
    private static void EnsureStateSize(int size)
    {
        while (selectedState.Count < size)
        {
            selectedState.Add(0);
        }

        while (selectedState.Count > size)
        {
            selectedState.RemoveAt(selectedState.Count - 1);
        }
    }

    // キャラ画像クリック時の共通処理
    private void OnCharacterClick(int index)
    {
        // 範囲外は無視
        if (index < 0 || index >= selectedFlg.Count)
        {
            return;
        }

        // すでに編成済みなら再処理しない
        if (selectedFlg[index] == 1)
        {
            return;
        }

        // 編成上限に達している場合は新規編成しない
        int currentSelectedCount = 0;
        for (int i = 0; i < selectedFlg.Count; i++)
        {
            if (selectedFlg[i] == 1)
            {
                currentSelectedCount++;
            }
        }

        if (currentSelectedCount >= maxSelectedCount)
        {
            return;
        }

        // 編成済みに更新（ローカル + static）
        selectedFlg[index] = 1;
        selectedState[index] = 1;

        // 見た目を暗くして編成済みを表現
        ApplyCharacterVisual(index);
    }

    // 指定キャラの見た目を編成状態に応じて更新
    private void ApplyCharacterVisual(int index)
    {
        if (index < 0 || index >= characterImages.Count)
        {
            return;
        }

        Image image = characterImages[index];

        if (image == null)
        {
            return;
        }

        // 編成済みなら少し暗く、未編成なら通常色
        if (selectedFlg[index] == 1)
        {
            image.color = new Color(selectedBrightness, selectedBrightness, selectedBrightness, 1f);
            return;
        }

        image.color = Color.white;
    }

    // 各ボタンから呼ばれるクリックイベント
    public void Character0Click()
    {
        OnCharacterClick(0);
    }
    public void Character1Click()
    {
        OnCharacterClick(1);
    }
    public void Character2Click()
    {
        OnCharacterClick(2);
    }
    public void Character3Click()
    {
        OnCharacterClick(3);
    }
    public void Character4Click()
    {
        OnCharacterClick(4);
    }
    public void Character5Click()
    {
        OnCharacterClick(5);
    }

    // 外部クラスから編成済み状態を参照するためのヘルパー
    public static bool IsCharacterSelected(int index)
    {
        if (index < 0 || index >= selectedState.Count)
        {
            return false;
        }

        return selectedState[index] == 1;
    }
}
