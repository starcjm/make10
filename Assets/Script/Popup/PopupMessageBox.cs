using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMessageBox : PopupBase
{
    public override void OnTouchAndroidBackButton()
    {
        OnTouchNo();
    }

    public void OnTouchYes()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        Application.Quit();
    }

    public void OnTouchNo()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        gameObject.SetActive(false);
    }
}
