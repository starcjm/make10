using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMain : PopupBase
{
    public GameObject textGroup;

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

    private void Start()
    {
        textGroup.SetActive(false);
        SetText();
        SetCoin();
    }

    public void OnTouchAds()
    {
    }

    public void OnTouchPlay()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        gameObject.SetActive(false);
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

    public void OnTouchAdsCoin()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        AdsManager.Instance.SetRewardType(E_REWARD_TYPE.MAIN_COIN_ADD);
        AdsManager.Instance.RewardAdShow();
    }

    public void OnTouchShop()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        GameManager.Instance.GetMainScreen().OpenShopPopup(PopupShop.E_SHOP_TYPE.LOBBY);
    }
}
