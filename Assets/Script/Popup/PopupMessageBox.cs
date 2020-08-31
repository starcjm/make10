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
        Application.Quit();
    }

    public void OnTouchNo()
    {
        gameObject.SetActive(false);
    }
}
