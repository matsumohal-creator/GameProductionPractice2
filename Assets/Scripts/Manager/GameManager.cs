using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //シングルトンの初期化
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //パーティ編成
    [Header("Party")]
    public static List<int> selectedFlgs = new List<int>();

    //サウンドマネージャーの参照
    [Header("Sound")]
    [SerializeField]
    private SoundManager soundManager;

    public static SoundManager Sound => Instance?.soundManager;
}
