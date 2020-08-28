using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 유저 데이터
/// </summary>
public class UserInfo : Singleton<UserInfo>
{
    //세이브 파일 이름
    private string LevelLabel = "LEVEL";
    private string HighScoreLabel = "HIGHSCORE";

    //재시작일 경우 플래그
    public bool isRetry = false;

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

    private void Start()
    {
        DontDestroyOnLoad(UserInfo.Instance);
    }

    public void LoadUserData()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreLabel, 0);
        Level = PlayerPrefs.GetInt(LevelLabel, 1);
    }
}
