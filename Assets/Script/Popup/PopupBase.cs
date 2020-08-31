using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupBase : MonoBehaviour
{
    virtual public void OnTouchAndroidBackButton()
    {
    }

    void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                OnTouchAndroidBackButton();
            }
        }
    }
}
