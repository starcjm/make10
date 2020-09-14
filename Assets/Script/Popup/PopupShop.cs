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

    public Text PriceNoAds;

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
        AdsCoin.text = "25";

        Coin200.text = Const.COIN_200.ToString();
        Price200.text = string.Format("${0}", Const.PRICE_200);

        Coin500.text = Const.COIN_500.ToString();
        Price500.text = string.Format("${0}", Const.PRICE_500);

        Coin1250.text = Const.COIN_1250.ToString();
        Price1250.text = string.Format("${0}", Const.PRICE_1250);

        Coin3500.text = Const.COIN_3500.ToString();
        Price3500.text = string.Format("${0}", Const.PRICE_3500);

        PriceNoAds.text = string.Format("${0}", Const.NO_ADS);

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
        gameObject.SetActive(false);
    }

    public void SetCoin()
    {
        if(CurrentCoin)
        {
            CurrentCoin.text = UserInfo.Instance.Coin.ToString();
        }
    }

    public void OnTouchAds()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        AdsManager.Instance.SetRewardType(E_REWARD_TYPE.SHOP_ADS_COIN);
        AdsManager.Instance.RewardAdShow();
    }

    public void OnTouch200()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.AddCoin(Const.COIN_200);
        GameManager.Instance.GetMainScreen().SetCoin(UserInfo.Instance.Coin);
        CurrentCoin.text = UserInfo.Instance.Coin.ToString();
        popupMain.SetCoin();
    }

    public void OnTouch500()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.AddCoin(Const.COIN_500);
        GameManager.Instance.GetMainScreen().SetCoin(UserInfo.Instance.Coin);
        CurrentCoin.text = UserInfo.Instance.Coin.ToString();
        popupMain.SetCoin();
    }

    public void OnTouch1250()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.AddCoin(Const.COIN_1250);
        GameManager.Instance.GetMainScreen().SetCoin(UserInfo.Instance.Coin);
        CurrentCoin.text = UserInfo.Instance.Coin.ToString();
        popupMain.SetCoin();
    }

    public void OnTouch3500()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.AddCoin(Const.COIN_3500);
        GameManager.Instance.GetMainScreen().SetCoin(UserInfo.Instance.Coin);
        CurrentCoin.text = UserInfo.Instance.Coin.ToString();
        popupMain.SetCoin();
    }

    public void OnTouchNoADS()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
    }
}
