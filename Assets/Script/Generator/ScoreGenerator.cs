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

    public GameObject TenBlockRange;
    public GameObject TenBlockPopup;

    public GameObject CreateAddScore(Transform parent, Vector3 pos, int addScore)
    {
        GameObject cloneScore = Instantiate(prefabScore);
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
            cloneCombo = Instantiate(prefabCombo1);
        }
        else if(combo == 3)
        {
            cloneCombo = Instantiate(prefabCombo2);
        }
        else if(combo > 0)
        {
            cloneCombo = Instantiate(prefabCombo3);
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

    public void CreateTenBlockRange(Transform parent, Vector3 pos)
    {
        GameObject cloneTenBlockRange = Instantiate(TenBlockRange);
        cloneTenBlockRange.name = string.Format("TenBlockRange");
        cloneTenBlockRange.transform.SetParent(parent.transform);
        cloneTenBlockRange.transform.localScale = Vector3.one;
        cloneTenBlockRange.transform.position = pos;
        Destroy(cloneTenBlockRange, 0.51f);
    }

    public void CreateTenBlockPopup(Transform parent)
    {
        GameObject cloneTenBlockPopup = Instantiate(TenBlockPopup);
        cloneTenBlockPopup.name = string.Format("TebBlockPopup");
        cloneTenBlockPopup.transform.SetParent(parent.transform);
        cloneTenBlockPopup.transform.localScale = Vector3.one;
        cloneTenBlockPopup.transform.position = Vector3.zero;
    }
}
