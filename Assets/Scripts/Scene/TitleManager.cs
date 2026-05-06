using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneLoader.NextSceneName = "HomeScene";
        SceneManager.LoadScene("LoadingScene");
    }
}