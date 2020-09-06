using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupTutorialComplete : PopupBase
{
    public GameObject giftOn;
    public GameObject giftOff;

    public GameObject coin;

    private bool isComplete = false;

    public override void OnTouchAndroidBackButton()
    {
        OnTouchClaim();
    }

    private void GetGift()
    {
        giftOn.SetActive(true);
        giftOff.SetActive(false);
    }

    public void OnTouchClaim()
    {
        if(!isComplete)
        {
            SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
            isComplete = true;
            TutorialManager.Instance.TutorialComplete();
            UserInfo.Instance.Coin += Const.TUTORIAL_COIN;
            GetGift();
            Invoke("CoinIconActive", 0.7f);
            Invoke("TutorialComplete", 1.5f);
        }
    }

    public void CoinIconActive()
    {
        coin.SetActive(true);
    }

    public void TutorialComplete()
    {
        SceneManager.LoadScene((int)E_SCENE.GAME);
    }
}
