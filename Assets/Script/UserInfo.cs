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

    public enum E_REVIEW
    {
        NO = 0,
        YES,
    }

    public enum E_TEN_BLOCK
    {
        NO = 0,
        YES,
    }

    //세이브 파일 이름
    private string HighScoreLabel = "HIGHSCORE";
    private string SoundLabel = "SOUND";
    private string CoinLabel = "COIN";
    private string TutorialLabel = "TUTORIAL";
    private string ReviewOkLabel = "REVIEWOK";
    private string ReviewNoFirstLabel = "REVIEWNOFIRST";
    private string ReviewCountLabel = "REVIEWCOUNT";
    private string TenBlockLabel = "TENBLOCK";

    private int reviewCount = 0;
    public int ReviewCount 
    {
        get { return reviewCount; }
        set 
        { 
            reviewCount = value;
            PlayerPrefs.SetInt(ReviewCountLabel, reviewCount);
        }
    }

    //리뷰보기 노 버튼 처음이면 3게임 아니면 10게임 하고 리뷰창 출력
    private bool reviewNoFirst = false;

    public int ReviewNoFirst
    {
        get { return PlayerPrefs.GetInt(ReviewNoFirstLabel, 0); }
        set
        {
            if (value == (int)E_REVIEW.NO)
            {
                reviewNoFirst = false;
            }
            else
            {
                reviewNoFirst = true;
            }
            PlayerPrefs.SetInt(ReviewNoFirstLabel, value);
        }
    }
    public bool IsReviewNoFirst()
    {
        return reviewNoFirst;
    }

    private bool reviewOk = false;
    //0이면 안쓴거 1이면 리뷰쓰러간거
    public int ReviewOk
    {
        get { return PlayerPrefs.GetInt(ReviewOkLabel, 0); }
        set
        {
            if (value == (int)E_REVIEW.NO)
            {
                reviewOk = false;
            }
            else
            {
                reviewOk = true;
            }
            PlayerPrefs.SetInt(ReviewOkLabel, value);
        }
    }

    public bool IsReviewOk()
    {
        return reviewOk;
    }

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

    public bool IsHighScore { get; set; } = false;

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

    private bool tenBlock = false;

    public int TenBlock
    {
        get { return PlayerPrefs.GetInt(TenBlockLabel, 0); }
        set
        {
            if (value == (int)E_TEN_BLOCK.NO)
            {
                tenBlock = false;
            }
            else
            {
                tenBlock = true;
            }
            PlayerPrefs.SetInt(TenBlockLabel, value);
        }
    }
    public bool IsTenBlockPopup()
    {
        return tenBlock;
    }

    private void Start()
    {
        DontDestroyOnLoad(Instance);
    }

    public void InitUserData()
    {
        Coin = 0;
        highScore = 0;
        PlayerPrefs.SetInt(HighScoreLabel, highScore);
        Sound = (int)E_SOUND_SET.ON;
        Tutorial = (int)E_TUTORIAL_SET.NO;
        ReviewOk = (int)E_REVIEW.NO;
        ReviewNoFirst = (int)E_REVIEW.NO;
        ReviewCount = 0;
    }

    public void LoadUserData()
    {
        coin = PlayerPrefs.GetInt(CoinLabel, 0);
        HighScore = PlayerPrefs.GetInt(HighScoreLabel, 0);
        Sound = PlayerPrefs.GetInt(SoundLabel, 0);
        Tutorial = PlayerPrefs.GetInt(TutorialLabel, 0);
        ReviewOk = PlayerPrefs.GetInt(ReviewOkLabel, 0);
        ReviewNoFirst = PlayerPrefs.GetInt(ReviewNoFirstLabel, 0);
        ReviewCount = PlayerPrefs.GetInt(ReviewCountLabel, 0);
        TenBlock = PlayerPrefs.GetInt(TenBlockLabel, 0);
    }
}
