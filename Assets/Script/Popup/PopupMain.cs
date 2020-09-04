using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMain : PopupBase
{
    public MainScreen mainScreen;

    public GameObject textGroup;

    public Text HighScore;
    public Text Coin;

    public override void OnTouchAndroidBackButton()
    {
        if(GameManager.Instance.GetState() != E_GAME_STATE.SHOP)
        { 
            mainScreen.ShowGameEnd();
        }
    }

    public void SetCoin()
    {
        if(Coin)
        {
            Coin.text = UserInfo.Instance.Coin.ToString();
        }
    }

    private void SetText()
    {
        if(UserInfo.Instance.HighScore > 0)
        {
            textGroup.SetActive(true);
            if(HighScore)
            {
                HighScore.text = UserInfo.Instance.HighScore.ToString();
            }
        }
        else
        {
            textGroup.SetActive(false);
        }
    }

    private void Start()
    {
        textGroup.SetActive(false);
        SetText();
        SetCoin();
    }

    public void OnTouchAds()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
    }

    public void OnTouchPlay()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        gameObject.SetActive(false);
        mainScreen.GameStart();
    }

    public void OnTouchRank()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
    }

    public void OnTouchSetting()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        mainScreen.ShowSettingPopup();
    }

    public void OnTouchAdsCoin()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
    }

    public void OnTouchShop()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        mainScreen.OpenShopPopup(PopupShop.E_SHOP_TYPE.LOBBY);
    }
}
