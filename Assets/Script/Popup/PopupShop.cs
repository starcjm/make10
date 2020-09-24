using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : PopupBase
{
    public enum E_SHOP_TYPE
    {
        LOBBY,
        IN_GAME,
    }

    public PopupMain popupMain;

    public GameObject getCoinAdsOn;
    public GameObject getCoinAdsOff;
    public GameObject noAdsOn;
    public GameObject noAdsOff;

    public Text CurrentCoin;

    public Text AdsCoin;
    public Text Coin200;
    public Text Price200;
    public Text Coin500;
    public Text Price500;
    public Text Coin1250;
    public Text Price1250;
    public Text Coin3500;
    public Text Price3500;

    public Text PriceNoAdsOn;
    public Text PriceNoAdsOff;

    private E_SHOP_TYPE shopType;

    public override void OnTouchAndroidBackButton()
    {
        OnTouchClose();
    }

    public void SetShopType(E_SHOP_TYPE type)
    {
        shopType = type;
    }

    private void Start()
    {
        InitData();
    }

    private void InitData()
    {
        AdsCoin.text = Const.ADS_COIN.ToString();

        Coin200.text = Const.COIN_200.ToString();
        Price200.text = string.Format("{0}", IAPManager.Instance.GetPrice(Const.PRODUCT_COIN_200));

        Coin500.text = Const.COIN_500.ToString();
        Price500.text = string.Format("{0}", IAPManager.Instance.GetPrice(Const.PRODUCT_COIN_500));

        Coin1250.text = Const.COIN_1250.ToString();
        Price1250.text = string.Format("{0}", IAPManager.Instance.GetPrice(Const.PRODUCT_COIN_1250));

        Coin3500.text = Const.COIN_3500.ToString();
        Price3500.text = string.Format("{0}", IAPManager.Instance.GetPrice(Const.PRODUCT_COIN_3500));

        SetNoAds();
        PriceNoAdsOn.text = string.Format("{0}", IAPManager.Instance.GetPrice(Const.PRODUCT_NO_ADS));
        PriceNoAdsOff.text = string.Format("{0}", IAPManager.Instance.GetPrice(Const.PRODUCT_NO_ADS));

        CurrentCoin.text = UserInfo.Instance.Coin.ToString();
    }

    public void OnTouchClose()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        if(shopType == E_SHOP_TYPE.LOBBY)
        {
            GameManager.Instance.SetGameState(E_GAME_STATE.PAUSE);
        }
        else if(shopType == E_SHOP_TYPE.IN_GAME)
        {
            GameManager.Instance.SetGameState(E_GAME_STATE.GAME);
        }
        getCoinAdsOn.SetActive(true);
        getCoinAdsOff.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetCoin()
    {
        if(CurrentCoin)
        {
            CurrentCoin.text = UserInfo.Instance.Coin.ToString();
        }
    }

    public void SetNoAds()
    {
        noAdsOn.SetActive(!AdsManager.Instance.IsNoAdsBuy());
        noAdsOff.SetActive(AdsManager.Instance.IsNoAdsBuy());
    }

    public void OnTouchAds()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        AdsManager.Instance.SetRewardType(E_REWARD_TYPE.SHOP_ADS_COIN);
        AdsManager.Instance.RewardAdShow();

        getCoinAdsOn.SetActive(false);
        getCoinAdsOff.SetActive(true);
    }

    public void OnTouch200()
    {
        GameManager.Instance.BuyShopItem(Const.PRODUCT_COIN_200);
    }

    public void OnTouch500()
    {
        GameManager.Instance.BuyShopItem(Const.PRODUCT_COIN_500);
    }

    public void OnTouch1250()
    {
        GameManager.Instance.BuyShopItem(Const.PRODUCT_COIN_1250);
    }

    public void OnTouch3500()
    {
        GameManager.Instance.BuyShopItem(Const.PRODUCT_COIN_3500);
    }

    public void OnTouchNoADS()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.BuyShopItem(Const.PRODUCT_NO_ADS);
    }
}
