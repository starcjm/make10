using System.Collections;
using System.Collections.Generic;
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

    public void ResetBlock()
    {
        GameManager.Instance.ResetScene();
    }

    public void ChangeShapeBlock()
    {
        GameManager.Instance.ChangeShapeBlock();
    }

    public void GameClose()
    {
        GameManager.Instance.GameClose();
    }
}
