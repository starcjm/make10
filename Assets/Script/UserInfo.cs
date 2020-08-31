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
    //세이브 파일 이름
    private string LevelLabel = "LEVEL";
    private string HighScoreLabel = "HIGHSCORE";
    private string SoundLabel = "SOUND";
    private string CoinLabel = "COIN";

    //재시작일 경우 플래그
    public bool isRetry = false;

    //사운드 플래그
    private bool isSound = false;

    public bool IsSound()
    {
        return isSound;
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
            //SoundManager.Instance.PlayBGM(E_BGM.BGM_ONE);
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

    private int level = 0;
    public int Level
    {
        get { return level; }
        set
        {
            if (level < value)
            {
                level = value;
                PlayerPrefs.SetInt(LevelLabel, level);
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
        DontDestroyOnLoad(UserInfo.Instance);
    }

    public void LoadUserData()
    {
        coin = PlayerPrefs.GetInt(CoinLabel, 1000);
        HighScore = PlayerPrefs.GetInt(HighScoreLabel, 0);
        Level = PlayerPrefs.GetInt(LevelLabel, 1);
        Sound = PlayerPrefs.GetInt(SoundLabel, 0);
    }
}
