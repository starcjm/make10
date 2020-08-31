using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : PopupBase
{
    public MainScreen mainScreen;

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

    public Text NoAds;
    public Text PriceNoAds;

    public override void OnTouchAndroidBackButton()
    {
        OnTouchClose();
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

        NoAds.text = "No ADS";
        PriceNoAds.text = string.Format("${0}", Const.NO_ADS);

        CurrentCoin.text = UserInfo.Instance.Coin.ToString();
    }

    public void OnTouchClose()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.SetGameState(E_GAME_STATE.GAME);
        gameObject.SetActive(false);
    }

    public void OnTouchAds()
    {
        GameManager.Instance.AddCoin(Const.ADS_COIN);
        mainScreen.SetCoin(UserInfo.Instance.Coin);
        CurrentCoin.text = UserInfo.Instance.Coin.ToString();
    }

    public void OnTouch200()
    {
        GameManager.Instance.AddCoin(Const.COIN_200);
        mainScreen.SetCoin(UserInfo.Instance.Coin);
        CurrentCoin.text = UserInfo.Instance.Coin.ToString();
    }

    public void OnTouch500()
    {
        GameManager.Instance.AddCoin(Const.COIN_500);
        mainScreen.SetCoin(UserInfo.Instance.Coin);
        CurrentCoin.text = UserInfo.Instance.Coin.ToString();
    }

    public void OnTouch1250()
    {
        GameManager.Instance.AddCoin(Const.COIN_1250);
        mainScreen.SetCoin(UserInfo.Instance.Coin);
        CurrentCoin.text = UserInfo.Instance.Coin.ToString();
    }

    public void OnTouch3500()
    {
        GameManager.Instance.AddCoin(Const.COIN_3500);
        mainScreen.SetCoin(UserInfo.Instance.Coin);
        CurrentCoin.text = UserInfo.Instance.Coin.ToString();
    }

    public void OnTouchNoADS()
    {
    }
}
