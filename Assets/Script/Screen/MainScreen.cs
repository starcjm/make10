using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 화면 스크린
/// </summary>
public class MainScreen : MonoBehaviour
{
    //현재 점수 
    public Text currentScore;
    //최대 점수
    public Text highScore;

    public Text Level;
    public Text NextLevel;

    public Text Coin;

    public Image FiledExp;

    //인게임 유아이
    public GameObject inGameUI;

    //팝업들
    public GameObject Main;
    public GameObject levelUp;
    public GameObject GameOver;
    public GameObject Continue;
    public GameObject Setting;
    public GameObject pause;

    private int currentLevel = 1;

    private void Start()
    {
        if(UserInfo.Instance.isRetry)
        {
            UserInfo.Instance.isRetry = false;
            GameStart();
            SetMainPopup(false);
        }
    }
    public void GameStart()
    {
        SetHighScore();
        SetInGameUI(true);
        GameManager.Instance.GameStart();
    }

    public void SetInGameUI(bool on)
    {
        inGameUI.SetActive(on);
    }

    public void SetMainPopup(bool on)
    {
        Main.SetActive(on);
    }

    public void ShowSettingPopup()
    {
        Main.SetActive(false);
        Setting.SetActive(true);
    }

    public void ShowPausePopup()
    {
        if(GameManager.Instance.GetState()== E_GAME_STATE.GAME)
        {
            GameManager.Instance.SetGameState(E_GAME_STATE.PAUSE);
            pause.SetActive(true);
        }
    }

    public void ShowGameOver()
    {
        GameManager.Instance.SetGameState(E_GAME_STATE.PAUSE);
        GameOver.SetActive(true);
    }

    public void SetLevel(int level)
    {
        UserInfo.Instance.Level = level;
        if(currentLevel != level)
        {
            levelUp.SetActive(true);
        }
        currentLevel = level;
        if (Level)
        {
            Level.text = level.ToString();
        }
        if(NextLevel)
        {
            NextLevel.text = (level + 1).ToString();
        }
    }

    public void SetCoin(int coin)
    {
        if(Coin)
        {
            Coin.text = coin.ToString();
        }
    }

    public void SetExp(int score)
    {
        float fillAmount = (float)score / Const.MAX_EXP;
        int quotient = (int)fillAmount + 1;
        SetLevel(quotient);
        int remainder = score % Const.MAX_EXP;
        FiledExp.fillAmount = (float)remainder / Const.MAX_EXP;
    }

    public void SetScore(int score)
    {
        if(currentScore)
        {
            currentScore.text = score.ToString();
        }
        SetHighScore();
    }

    public void SetHighScore()
    {
        if(highScore)
        {
            highScore.text = UserInfo.Instance.HighScore.ToString();
        }
    }

    public void ChangeShapeBlock()
    {
        GameManager.Instance.ChangeShapeBlock();
    }

    public void GameClose()
    {
        GameManager.Instance.GameClose();
    }

    public void ShowContinuePopup()
    {
        Continue.SetActive(true);
    }

    public void OnTouchHammer()
    {
        GameManager.Instance.ShowBlockX();
        GameManager.Instance.SetGameItemState();
    }
}
