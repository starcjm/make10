using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인게임 화면 스크린
/// </summary>
public class MainScreen : MonoBehaviour, IAndroidBackButton
{
    //현재 점수 
    public Text currentScore;
    //최대 점수
    public Text highScore;

    public Text Level;
    public Text NextLevel;
    public Text Coin;

    public Text HammerCoin;
    public Text NextBlockCoin;

    public Image FiledExp;

    //인게임 유아이
    public GameObject inGameUI;

    //팝업들
    public GameObject Main;
    public GameObject LevelUp;
    public GameObject GameOver;
    public GameObject Continue;
    public GameObject Setting;
    public GameObject Pause;
    public GameObject Shop;
    public GameObject MessageBox;

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

    private void Update()
    {
        OnTouchAndroidBackButton();
    }

    public void OnTouchAndroidBackButton()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (GameManager.Instance.GetState() == E_GAME_STATE.GAME)
                {
                    ShowPausePopup();
                }
                else if (GameManager.Instance.GetState() == E_GAME_STATE.ITEM)
                {
                    OnTouchHammer();
                }
            }
        }
    }

    public void GameStart()
    {
        SetHighScore();
        SetInGameUI(true);
        InitPriceData();
        GameManager.Instance.GameStart();
    }

    private void InitPriceData()
    {
        NextBlockCoin.text = Const.BLOCK_NEXT_PRICE.ToString();
        HammerCoin.text = Const.HAMMER_PRICE.ToString();
        SetCoin(UserInfo.Instance.Coin);
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
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        Main.SetActive(false);
        Setting.SetActive(true);
    }

    public void ShowPausePopup()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        if (GameManager.Instance.GetState() == E_GAME_STATE.GAME)
        {
            GameManager.Instance.SetGameState(E_GAME_STATE.PAUSE);
            Pause.SetActive(true);
        }
    }

    public void ShowShopPopup()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        if (GameManager.Instance.GetState() == E_GAME_STATE.GAME)
        {
            GameManager.Instance.SetGameState(E_GAME_STATE.PAUSE);
            Shop.SetActive(true);
        }
    }

    public void ShowGameOver()
    {
        GameManager.Instance.SetGameState(E_GAME_STATE.PAUSE);
        GameOver.SetActive(true);
    }

    public void ShowGameEnd()
    {
        if(!MessageBox.activeSelf)
        {
            MessageBox.SetActive(true);
        }
    }

    public void SetLevel(int level)
    {
        UserInfo.Instance.Level = level;
        if(currentLevel != level)
        {
            GameManager.Instance.SetGameState(E_GAME_STATE.PAUSE);
            LevelUp.SetActive(true);
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
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        if (UserInfo.Instance.Coin >= Const.BLOCK_NEXT_PRICE)
        {
            GameManager.Instance.AddCoin(-Const.BLOCK_NEXT_PRICE);
            SetCoin(UserInfo.Instance.Coin);
            GameManager.Instance.ChangeShapeBlock();
        }
        else
        {
            ShowShopPopup();
        }
    }

    public void GameClose()
    {
        GameManager.Instance.GameClose();
    }

    public void ShowContinuePopup(int score)
    {
        var popUp = Continue.GetComponent<PopupContinue>();
        if(popUp)
        {
            popUp.SetScore(score);
        }
        Continue.SetActive(true);
    }

    public void OnTouchHammer()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        if (UserInfo.Instance.Coin >= Const.HAMMER_PRICE)
        {
            GameManager.Instance.ShowBlockX();
            GameManager.Instance.SetGameItemState();
        }
        else
        {
            ShowShopPopup();
        }
    }
}
