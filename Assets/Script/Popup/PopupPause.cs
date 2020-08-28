using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PopupPause : MonoBehaviour
{
    public MainScreen mainScreen;

    public void OnTouchBack()
    {
        gameObject.SetActive(false);
        GameManager.Instance.SetGameState(E_GAME_STATE.GAME);
    }

    public void OnTouchHome()
    {
        SceneManager.LoadScene(1);
    }

    public void OnTouchSound()
    {

    }

    public void OnTouchRetry()
    {
        gameObject.SetActive(false);
        UserInfo.Instance.isRetry = true;
        SceneManager.LoadScene(1);
    }

    public void OnTouchContinue()
    {
        gameObject.SetActive(false);
        GameManager.Instance.SetGameState(E_GAME_STATE.GAME);
    }
}
