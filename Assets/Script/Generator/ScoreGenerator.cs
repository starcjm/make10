using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스코어 회득 텍스트 생성
/// </summary>
public class ScoreGenerator : Singleton<ScoreGenerator>
{
    private readonly float aniTime = 0.4f;
    public GameObject prefabScore;

    public GameObject CreateAddScore(Transform parent, Vector3 pos, int addScore)
    {
        GameObject cloneScore = (GameObject)Instantiate(prefabScore);
        var scoreAdd = cloneScore.GetComponent<TextScore>();
        if(scoreAdd)
        {
            scoreAdd.SetAddScore(addScore);
        }
        cloneScore.name = string.Format("SCORE");
        cloneScore.transform.SetParent(parent.transform);
        cloneScore.transform.localScale = Vector3.one;
        cloneScore.transform.position = new Vector3(0.0f, pos.y);
        cloneScore.transform.DOMoveY(pos.y + 0.3f, aniTime).OnComplete(() =>
        {
            Destroy(cloneScore);
        });
        return cloneScore;
    }
}
