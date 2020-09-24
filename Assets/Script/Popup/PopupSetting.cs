using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSetting : PopupBase
{
    public GameObject SoundON;
    public GameObject SoundOff;

    public GameObject MusicON;
    public GameObject MusicOff;

    public GameObject noAdsOn;
    public GameObject noAdsOff;

    public override void OnTouchAndroidBackButton()
    {
        OnTouchBack();
    }

    private void Start()
    {
        bool isSound = UserInfo.Instance.IsSound();
        SoundON.SetActive(isSound);
        SoundOff.SetActive(!isSound);

        bool isMusic = UserInfo.Instance.IsMusic();
        MusicON.SetActive(isMusic);
        MusicOff.SetActive(!isMusic);
        SetNoAds();
    }

    public void SetNoAds()
    {
        noAdsOn.SetActive(!AdsManager.Instance.IsNoAdsBuy());
        noAdsOff.SetActive(AdsManager.Instance.IsNoAdsBuy());
    }

    public void OnTouchBack()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        gameObject.SetActive(false);
        GameManager.Instance.GetMainScreen().SetMainPopup(true);
    }

    public void OnTouchSound()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        UserInfo.Instance.SetSound();
        bool isSound = UserInfo.Instance.IsSound();
        SoundON.SetActive(isSound);
        SoundOff.SetActive(!isSound);
    }

    public void OnTouchMusic()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        UserInfo.Instance.SetMusic();
        bool isMusic = UserInfo.Instance.IsMusic();
        MusicON.SetActive(isMusic);
        MusicOff.SetActive(!isMusic);
    }

    public void OnTouchShare()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
    }

    public void OnTouchNoAds()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.BuyShopItem(Const.PRODUCT_NO_ADS);
    }

    public void OnTouchRestore()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        IAPManager.Instance.RestorePurchase();
    }
}
