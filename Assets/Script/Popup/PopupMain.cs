using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMain : PopupBase
{
    public GameObject textGroup;
    public GameObject adsCoinButton;
    public GameObject noAds;

    public Text AdsCoin;
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
        if(AdsCoin)
        {
            AdsCoin.text = Const.ADS_COIN.ToString();
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
        adsCoinButton.SetActive(true);
        GameManager.Instance.GetMainScreen().GameStart();
    }

    public void OnTouchRank()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
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
        adsCoinButton.SetActive(false);
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
        adsCoinButton.SetActive(true);
    }

    public void SetNoAds()
    {
        noAds.SetActive(!AdsManager.Instance.IsNoAdsBuy());
    }
}
