using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSetting : MonoBehaviour
{
    public MainScreen mainScreen;

    public GameObject SoundON;
    public GameObject SoundOff;

    public void OnTouchBack()
    {
        gameObject.SetActive(false);
        mainScreen.SetMainPopup(true);
    }

    public void OnTouchSound()
    {

    }

    public void OnTouchShare()
    {

    }

    public void OnTouchAds()
    {

    }

    public void OnTouchRestore()
    {

    }
}
