using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유저 데이터
/// </summary>
public class UserInfo : Singleton<UserInfo>
{
    private enum E_SOUND_SET
    {
        ON = 0,
        OFF,
    }

    public enum E_TUTORIAL_SET
    {
        NO = 0,
        YES,
    }
    //세이브 파일 이름
    private string HighScoreLabel = "HIGHSCORE";
    private string SoundLabel = "SOUND";
    private string CoinLabel = "COIN";
    private string TutorialLabel = "TUTORIAL";

    //튜토리얼 플래그 (true 면 튜로리얼 완료)
    public bool isTutorial = false;
    public bool IsTuroial()
    {
        return isTutorial;
    }

    //재시작일 경우 플래그
    public bool isRetry = false;

    //사운드 플래그
    private bool isSound = false;

    public bool IsSound()
    {
        return isSound;
    }

    /// <summary>
    /// 0이면 튜토리얼 안한거 1이면 한거 
    /// </summary>
    public int Tutorial
    {
        get { return PlayerPrefs.GetInt(TutorialLabel, 0); }
        set
        {
            if (value == (int)E_TUTORIAL_SET.NO)
            {
                isTutorial = false;
            }
            else
            {
                isTutorial = true;
            }
            PlayerPrefs.SetInt(TutorialLabel, value);
        }
    }

    /// <summary>
    /// 0이면 사운드 온 1이면 오프
    /// </summary>
    public int Sound
    {
        get { return PlayerPrefs.GetInt(SoundLabel, 0); }
        set 
        { 
            if(value == (int)E_SOUND_SET.ON)
            {
                isSound = true;
            }
            else
            {
                isSound = false;
            }
            PlayerPrefs.SetInt(SoundLabel, value);
        }
    }

    public void SetSound()
    {
        if(Sound == (int)E_SOUND_SET.ON)
        {
            Sound = (int)E_SOUND_SET.OFF;
            SoundManager.Instance.StopBGM();
        } 
        else
        {
            Sound = (int)E_SOUND_SET.ON;
            SoundManager.Instance.PlayBGM(E_BGM.BGM_ONE);
        }
    }

    private int highScore = 0;
    public int HighScore
    {
        get { return highScore; }
        set 
        {
            if(highScore < value)
            {
                highScore = value;
                PlayerPrefs.SetInt(HighScoreLabel, highScore);
            }
        }
    }

    private int coin = 0;
    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            PlayerPrefs.SetInt(CoinLabel, coin);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(Instance);
    }

    public void InitUserData()
    {
        Coin = 1000;
        highScore = 0;
        PlayerPrefs.SetInt(HighScoreLabel, highScore);
        Sound = (int)E_SOUND_SET.ON;
        Tutorial = (int)E_TUTORIAL_SET.NO;
    }

    public void LoadUserData()
    {
        coin = PlayerPrefs.GetInt(CoinLabel, 1000);
        HighScore = PlayerPrefs.GetInt(HighScoreLabel, 0);
        Sound = PlayerPrefs.GetInt(SoundLabel, 0);
        Tutorial = PlayerPrefs.GetInt(TutorialLabel, 0);
    }
}
