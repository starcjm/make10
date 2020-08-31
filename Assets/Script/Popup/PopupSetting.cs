using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSetting : PopupBase
{
    public MainScreen mainScreen;

    public GameObject SoundON;
    public GameObject SoundOff;

    public override void OnTouchAndroidBackButton()
    {
        OnTouchBack();
    }

    private void Start()
    {
        bool isSound = UserInfo.Instance.IsSound();
        SoundON.SetActive(isSound);
        SoundOff.SetActive(!isSound);
    }

    public void OnTouchBack()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        gameObject.SetActive(false);
        mainScreen.SetMainPopup(true);
    }

    public void OnTouchSound()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        UserInfo.Instance.SetSound();
        bool isSound = UserInfo.Instance.IsSound();
        SoundON.SetActive(isSound);
        SoundOff.SetActive(!isSound);
    }

    public void OnTouchShare()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
    }

    public void OnTouchAds()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
    }

    public void OnTouchRestore()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
    }
}
