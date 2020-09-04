using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스코어 회득 텍스트 생성
/// </summary>
public class ScoreGenerator : Singleton<ScoreGenerator>
{
    private readonly float aniTime = 1.0f;
    public GameObject prefabScore;

    public GameObject prefabCombo1;
    public GameObject prefabCombo2;
    public GameObject prefabCombo3;

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
        cloneScore.transform.position = pos;
        cloneScore.transform.DOMoveY(pos.y + 0.3f, aniTime).OnComplete(() =>
        {
            Destroy(cloneScore);
        });
        return cloneScore;
    }

    //콤보 이팩트( 화면 중앙에 출력)
    public GameObject CreateComboEffect(Transform parent, Vector3 pos, int combo)
    {
        GameObject cloneCombo = null;

        if(combo == 2)
        {
            cloneCombo = (GameObject)Instantiate(prefabCombo1);
        }
        else if(combo == 3)
        {
            cloneCombo = (GameObject)Instantiate(prefabCombo2);
        }
        else if(combo > 0)
        {
            cloneCombo = (GameObject)Instantiate(prefabCombo3);
        }

        cloneCombo.name = string.Format("COMBO");
        cloneCombo.transform.SetParent(parent.transform);
        cloneCombo.transform.localScale = Vector3.one;
        cloneCombo.transform.position = pos;
        cloneCombo.transform.DOMoveY(pos.y + 0.3f, aniTime).OnComplete(() =>
        {
            Destroy(cloneCombo);
        });
        return cloneCombo;
    }
}
