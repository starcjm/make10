using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMain : MonoBehaviour
{
    public MainScreen mainScreen;

    public GameObject textGroup;

    public Text Level;
    public Text HighScore;

    private void SetText()
    {
        if(UserInfo.Instance.HighScore > 0)
        {
            textGroup.SetActive(true);
            if(Level)
            {
                Level.text = string.Format("Lv {0}", UserInfo.Instance.Level);
            }
            if(HighScore)
            {
                HighScore.text = UserInfo.Instance.HighScore.ToString();
            }
        }
        else
        {
            textGroup.SetActive(false);
        }
    }

    private void Start()
    {
        textGroup.SetActive(false);
        SetText();
    }

    public void OnTouchAds()
    {

    }

    public void OnTouchPlay()
    {
        gameObject.SetActive(false);
        mainScreen.GameStart();
    }

    public void OnTouchRank()
    {

    }

    public void OnTouchSetting()
    {
        mainScreen.ShowSettingPopup();
    }
}
