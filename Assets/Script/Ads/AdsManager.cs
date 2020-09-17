using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public enum E_REWARD_TYPE
{
    NONE,
    MAIN_COIN_ADD,
    SHOP_ADS_COIN,
    COTINUE,
}

public enum E_NO_ADS_BUY
{
    NO = 0,
    YES = 1,
}
public class AdsManager : Singleton<AdsManager>
{
    //배너 광고
    private BannerView banner;
    //전면 광고
    private InterstitialAd screenAd;
    //동영상 광고
    private RewardedAd rewardAd;

    private E_REWARD_TYPE rewardAdType = E_REWARD_TYPE.NONE;

    private bool noAdsBuy = false;

    public void SetRewardType(E_REWARD_TYPE type)
    {
        rewardAdType = type;
    }

    private void Start()
    {
        DontDestroyOnLoad(Instance);
        
    }

    private void SetNoAds()
    {
        E_NO_ADS_BUY noAds = (E_NO_ADS_BUY)PlayerPrefs.GetInt("NOADSBUY", (int)E_NO_ADS_BUY.NO);
        if (noAds == E_NO_ADS_BUY.NO)
        {
            noAdsBuy = false;
        }
        else if(noAds == E_NO_ADS_BUY.YES)
        {
            noAdsBuy = true;
        }
    }

    public bool IsNoAdsBuy()
    {
        return noAdsBuy;
    }

    public void BuyNoAds()
    {
        PlayerPrefs.SetInt("NOADSBUY", (int)E_NO_ADS_BUY.YES);
        SetNoAds();
        DestroyAd();
    }

    public void Init()
    {
        SetNoAds();
        MobileAds.Initialize(
            (initStatus) =>
            {
                InitBannerAd();
                InitInterstitialAd();
                initRewardAd();
            });
    }

    //배너광고
    private void InitBannerAd()
    {
        if(!IsNoAdsBuy())
        {
            AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
            banner = new BannerView(Const.ADS_ID_BANNER, adaptiveSize, AdPosition.Bottom);
            AdRequest request = new AdRequest.Builder().Build();
            banner.LoadAd(request);
        }
    }

    public void SetBannerAd(bool on)
    {
        if (on)
        {
            banner.Show();
        }
        else
        {
            banner.Hide();
        }
    }

    private void DestroyAd()
    {
        if(banner != null)
        {
            banner.Destroy();
        }
    }

    //전면 광고
    private void InitInterstitialAd()   
    {
        if (!IsNoAdsBuy())
        {
            if (screenAd != null)
            {
                screenAd.Destroy();
            }
            screenAd = new InterstitialAd(Const.ADS_ID_FRONT);
            screenAd.OnAdLoaded += HandleOnAdLoaded;
            screenAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            screenAd.OnAdOpening += HandleOnAdOpened;
            screenAd.OnAdClosed += HandleOnAdClosed;
            screenAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;
            RequestInterstitial();
        }
    }

    private void RequestInterstitial()
    {
        AdRequest request = new AdRequest.Builder().Build();
        screenAd.LoadAd(request);
    }


    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        RequestInterstitial();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLeavingApplication event received");
    }
    

    public void InterstitialAdShow()
    {
#if !UNITY_EDITOR
        if (!IsNoAdsBuy())
        {
            StartCoroutine("ShowScreenAd");
        }
#endif
    }

    private IEnumerator ShowScreenAd()
    {
        while(!screenAd.IsLoaded())
        {
            yield return null;
        }
        screenAd.Show();
    }

    //보상형 동영상 광고
    private void initRewardAd()
    {
        rewardAd = new RewardedAd(Const.ADS_ID_REWARD);
        rewardAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardAd.OnAdOpening += HandleRewardedAdOpening;
        rewardAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        rewardAd.OnAdClosed += HandleRewardedAdClosed;
        rewardAd.OnUserEarnedReward += HandleUserEarnedReward;
        AdRequest request = new AdRequest.Builder().Build();
        rewardAd.LoadAd(request);
    }


    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        initRewardAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        switch(rewardAdType)
        {
            case E_REWARD_TYPE.MAIN_COIN_ADD:
                {
                    GameManager.Instance.AddCoin(Const.ADS_COIN);
                    GameManager.Instance.GetMainScreen().SetCoin(UserInfo.Instance.Coin);
                    GameManager.Instance.GetMainScreen().MainPopupUIRefresh();
                    GameManager.Instance.GetMainScreen().ShowAdsCoinPopup(Const.ADS_COIN);
                }
                break;
            case E_REWARD_TYPE.SHOP_ADS_COIN:
                {
                    GameManager.Instance.AddCoin(Const.ADS_COIN);
                    GameManager.Instance.GetMainScreen().SetCoin(UserInfo.Instance.Coin);
                    GameManager.Instance.GetMainScreen().MainPopupUIRefresh();
                    GameManager.Instance.ShopUiRefesh();
                    GameManager.Instance.GetMainScreen().ShowAdsCoinPopup(Const.ADS_COIN);
                }
                break;
            case E_REWARD_TYPE.COTINUE:
                {
                    GameManager.Instance.TouchContinue();
                }
                break;
            default:
                break;
        }
    }

    public void RewardAdShow()
    {
#if !UNITY_EDITOR
        StartCoroutine("ShowReawardAd");
#endif
    }

    private IEnumerator ShowReawardAd()
    {
        while (!rewardAd.IsLoaded())
        {
            yield return null;
        }
        rewardAd.Show();
    }
}
