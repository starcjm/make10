using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupBestScore : PopupBase
{
    public GameObject imgCrown;
    public GameObject textBest;

    public Text textScore;

    public override void OnTouchAndroidBackButton()
    {
        OnTouchClose();
    }

    private void Start()
    {
        SetScore();
        SoundManager.Instance.PlaySFX(E_SFX.BEST_SCORE);
        ActiveCrown();
        Invoke("ActiveScore", 1.0f);
        Invoke("ActiveBestText", 2.0f);
        Invoke("OnTouchClose", 6.5f);
    }

    public void SetScore()
    {
        if(textScore)
        {
            textScore.text = UserInfo.Instance.HighScore.ToString();
        }
    }

    public void ActiveCrown()
    {
        imgCrown.SetActive(true);
        imgCrown.transform.DOScale(1.0f, 1.0f);
    }

    public void ActiveBestText()
    {
        textBest.SetActive(true);
        textBest.transform.DOScale(1.0f, 1.0f);
    }

    public void ActiveScore()
    {
        textScore.gameObject.SetActive(true);
        textScore.transform.DOScale(1.0f, 1.0f);
    }

    public void OnTouchClose()
    {
        AdsManager.Instance.InterstitialAdShow();
        SceneManager.LoadScene((int)E_SCENE.GAME);
    }
}
