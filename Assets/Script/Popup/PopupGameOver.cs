using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGameOver : PopupBase
{
    public override void OnTouchAndroidBackButton()
    {
        OnTouchGameOver();
    }

    public void OnTouchGameOver()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ShowContinuePopup();
    }
}
