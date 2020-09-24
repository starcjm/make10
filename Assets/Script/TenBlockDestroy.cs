using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenBlockDestroy : MonoBehaviour
{
    public GameObject titleBG;
    public GameObject text;

    public void Start()
    {
        StartAni();
    }

    public void StartAni()
    {
        titleBG.transform.DOScale(Vector3.one, 0.1f).OnComplete(()=>
        {
            text.SetActive(true);
            Destroy(gameObject, 1.2f);
        });
    }
}
