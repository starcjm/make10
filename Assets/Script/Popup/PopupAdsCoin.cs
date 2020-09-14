using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupAdsCoin : MonoBehaviour
{
    public Text coin;

    public void SetCoin(int addCoin)
    {
        SoundManager.Instance.PlaySFX(E_SFX.ADS_GET_COIN);
        if(coin)
        {
            coin.text = addCoin.ToString();
        }
    }

    public void OnTouchClose()
    {
        gameObject.SetActive(false);
    }
}
