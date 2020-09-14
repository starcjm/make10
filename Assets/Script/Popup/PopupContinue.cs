using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 컨티뉴 팝업
/// </summary>
public class PopupContinue : PopupBase
{
    public Text score;
    public Text highScore;

    public Text timeCount;
    public Image timeGage;

    private float time = 0.0f;
    private readonly float continueTime = 5.0f;

    public override void OnTouchAndroidBackButton()
    {
        OnTouchHome();
    }

    public void Init()
    {
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
        timeGage.fillAmount = 1 - (time / continueTime);
        timeCount.text = ((int)(continueTime - time)).ToString();
        if (time >= continueTime)
        {
            OnTouchHome();
        }
    }

    public void SetScore(int Score)
    {
        score.text = Score.ToString();
        highScore.text = UserInfo.Instance.HighScore.ToString();
    }

    public void OnTouchContinue()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        AdsManager.Instance.SetRewardType(E_REWARD_TYPE.COTINUE);
        AdsManager.Instance.RewardAdShow();
    }

    public void OnTouchHome()
    {
        if(UserInfo.Instance.IsHighScore)
        {
            UserInfo.Instance.IsHighScore = false;
            GameManager.Instance.GetMainScreen().ShowBestScorePopup();
            gameObject.SetActive(false);
        }
        else
        {
            AdsManager.Instance.InterstitialAdShow();
            SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
            SceneManager.LoadScene((int)E_SCENE.GAME);
        }
    }
}
