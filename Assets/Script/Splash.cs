using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 스플래시 씬
/// </summary>

public enum E_SCENE
{
    SPLASH = 0,
    TUTORIAL,
    GAME,
}

public class Splash : MonoBehaviour
{
    public float time = 2.0f;
    private void Start()
    {
        AdsManager.Instance.Init();
        LoadLanguage();
        UserInfo.Instance.LoadUserData();
        Invoke("MainScene", time);
    }

    private void LoadLanguage()
    {
        string sysLanguage = Application.systemLanguage.ToString();
        if(sysLanguage == "Korean")
        {
            PlayerPrefs.SetString("systemLanguage", "Korean");
        }
        else
        {
            PlayerPrefs.SetString("systemLanguage", "English");
        }
    }

    private void MainScene()
    {
        if(UserInfo.Instance.IsTuroial())
        {
            //튜토리얼 한거
            SceneManager.LoadScene((int)E_SCENE.GAME);
        }
        else 
        {
            //튜토리얼 안한거
            SceneManager.LoadScene((int)E_SCENE.TUTORIAL);
        }
    }
}
