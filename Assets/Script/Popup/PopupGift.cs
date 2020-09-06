using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupGift : PopupBase
{
    public GameObject openGift;
    public GameObject closeGift;

    public GameObject coinIcon;
    public Text coin;

    private bool isTouchButton = false;

    private float effectTime = 1.5f;

    public override void OnTouchAndroidBackButton()
    {
        OnTouchClaim();
    }

    public void DataInit()
    {
        isTouchButton = false;
        coinIcon.SetActive(false);
        openGift.SetActive(false);
        closeGift.SetActive(true);
    }

    public void SetCoin(int addCoin)
    {
        if(coin)
        {
            coin.gameObject.SetActive(true);
            coin.text = addCoin.ToString();
        }
    }

    private void GetGift()
    {
        openGift.SetActive(true);
        closeGift.SetActive(false);
    }

    public void OnTouchAdsClaim()
    {
        if(!isTouchButton)
        {
            isTouchButton = true;
            SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
            GetGift();
            Invoke("CoinIconActive", 0.7f);
            SetCoin(Const.GIFT_COIN * 2);
            GameManager.Instance.AddCoin(Const.GIFT_COIN * 2);
            Invoke("StartCoinEffect", effectTime);
            
        }
    }

    public void OnTouchClaim()
    {
        if(!isTouchButton)
        {
            isTouchButton = true;
            SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
            GetGift();
            Invoke("CoinIconActive", 0.7f);
            SetCoin(Const.GIFT_COIN);
            GameManager.Instance.AddCoin(Const.GIFT_COIN);
            Invoke("StartCoinEffect", effectTime);
        }
    }

    public void CoinIconActive()
    {
        coinIcon.SetActive(true);
    }

    public void StartCoinEffect()
    {
        GameManager.Instance.GetMainScreen().CreateCoinEffect(coinIcon.transform.position);
        gameObject.SetActive(false);
    }
}
