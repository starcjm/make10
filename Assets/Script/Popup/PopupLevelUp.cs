using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 레벨업 팝업
/// </summary>
public class PopupLevelUp : PopupBase
{
    public MainScreen mainScreen;

    public override void OnTouchAndroidBackButton()
    {
        OnTouchClaim();
    }

    public void OnTouchAdsCoin()
    {
        gameObject.SetActive(false);
        GameManager.Instance.SetGameState(E_GAME_STATE.GAME);
        GameManager.Instance.AddCoin(Const.LEVEL_COIN * 2);
        mainScreen.SetCoin(UserInfo.Instance.Coin);
    }

    public void OnTouchClaim()
    {
        gameObject.SetActive(false);
        GameManager.Instance.SetGameState(E_GAME_STATE.GAME);
        GameManager.Instance.AddCoin(Const.LEVEL_COIN);
        mainScreen.SetCoin(UserInfo.Instance.Coin);
    }

}
