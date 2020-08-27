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

    public Image FiledExp;

    //인게임 유아이들
    public GameObject levelUp;
    public GameObject GameOver;
    public GameObject Continue;

    private int currentLevel = 1;


    public void ShowGameOver()
    {
        GameOver.SetActive(true);
    }

    public void SetLevel(int level)
    {
        if(currentLevel != level)
        {
            levelUp.SetActive(true);
        }
        currentLevel = level;
        if (Level)
        {
            Level.text =level.ToString();
        }
        if(NextLevel)
        {
            NextLevel.text = (level + 1).ToString();
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
    }

    public void SetHighScore(int score)
    {
        if(highScore)
        {
            highScore.text = score.ToString();
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
