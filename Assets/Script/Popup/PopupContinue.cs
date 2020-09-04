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
        //이어하기 했을때 가운데 3 * 3블럭 삭제
        GameManager.Instance.ContinueDestryBlock();
        gameObject.SetActive(false);
        GameManager.Instance.SetGameState(E_GAME_STATE.GAME);
    }

    public void OnTouchHome()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        SceneManager.LoadScene(1);
    }
}
