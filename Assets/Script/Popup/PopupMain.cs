﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMain : PopupBase
{
    public GameObject textGroup;
    public GameObject adsCoinButtonOn;
    public GameObject adsCoinButtonOff;
    public GameObject noAdsOn;
    public GameObject noAdsOff;

    public Text AdsCoinOn;
    public Text AdsCoinOff;
    public Text HighScore;
    public Text Coin;

    public override void OnTouchAndroidBackButton()
    {
        if(GameManager.Instance.GetState() != E_GAME_STATE.SHOP)
        { 
            GameManager.Instance.GetMainScreen().ShowGameEnd();
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

    private void SetAdsCoin()
    {
        if(AdsCoinOn)
        {
            AdsCoinOn.text = Const.ADS_COIN.ToString();
        }
        if (AdsCoinOff)
        {
            AdsCoinOff.text = Const.ADS_COIN.ToString();
        }
    }

    private void Start()
    {
        textGroup.SetActive(false);
        SetText();
        SetCoin();
        SetAdsCoin();
        SetNoAds();
    }

    public void OnTouchPlay()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        gameObject.SetActive(false);
        adsCoinButtonOn.SetActive(true);
        adsCoinButtonOff.SetActive(false);
        GameManager.Instance.GetMainScreen().GameStart();
    }

    public void OnTouchRank()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.ShowRanking();
    }

    public void OnTouchSetting()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.GetMainScreen().ShowSettingPopup();
    }

    public void OnTouchNoAds()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.BuyShopItem(Const.PRODUCT_NO_ADS);
    }

    public void OnTouchAdsCoin()
    {
        adsCoinButtonOn.SetActive(false);
        adsCoinButtonOff.SetActive(true);
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        AdsManager.Instance.SetRewardType(E_REWARD_TYPE.MAIN_COIN_ADD);
        AdsManager.Instance.RewardAdShow();
    }

    public void OnTouchShop()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.GetMainScreen().OpenShopPopup(PopupShop.E_SHOP_TYPE.LOBBY);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        adsCoinButtonOn.SetActive(true);
        adsCoinButtonOff.SetActive(false);
    }

    public void SetNoAds()
    {
        noAdsOn.SetActive(!AdsManager.Instance.IsNoAdsBuy());
        noAdsOff.SetActive(AdsManager.Instance.IsNoAdsBuy());
    }
}
