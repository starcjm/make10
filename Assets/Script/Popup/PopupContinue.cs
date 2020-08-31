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

    public override void OnTouchAndroidBackButton()
    {
        OnTouchHome();
    }

    public void SetScore(int Score)
    {
        score.text = Score.ToString();
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
