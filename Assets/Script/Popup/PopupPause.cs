using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PopupPause : PopupBase
{
    public MainScreen mainScreen;

    public GameObject SoundON;
    public GameObject SoundOff;

    private void Start()
    {
        bool isSound = UserInfo.Instance.IsSound();
        SoundON.SetActive(isSound);
        SoundOff.SetActive(!isSound);
    }

    public override void OnTouchAndroidBackButton()
    {
        OnTouchBack();
    }

    public void OnTouchBack()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        gameObject.SetActive(false);
        GameManager.Instance.SetGameState(E_GAME_STATE.GAME);
    }

    public void OnTouchHome()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        SceneManager.LoadScene((int)E_SCENE.GAME);
    }

    public void OnTouchSound()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        UserInfo.Instance.SetSound();
        bool isSound = UserInfo.Instance.IsSound();
        SoundON.SetActive(isSound);
        SoundOff.SetActive(!isSound);
    }

    public void OnTouchRetry()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.Retry();
        gameObject.SetActive(false);
    }

    public void OnTouchContinue()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        gameObject.SetActive(false);
        GameManager.Instance.SetGameState(E_GAME_STATE.GAME);
    }
}
