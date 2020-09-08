using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : Singleton<CoinGenerator>
{
    public GameObject effectLayer;
    public GameObject prefabCoin;

    private readonly float aniTime = 0.5f;

    public GameObject CreateCoinEffect(Vector3 startPos, Vector3 targetPos)
    {
        GameObject cloneCoin = (GameObject)Instantiate(prefabCoin);

        cloneCoin.name = string.Format("COIN");
        cloneCoin.transform.SetParent(effectLayer.transform);
        cloneCoin.transform.localScale = Vector3.one;
        cloneCoin.transform.position = startPos;
        cloneCoin.transform.DOMove(targetPos, aniTime).OnComplete(() =>
        {
            SoundManager.Instance.PlaySFX(E_SFX.ADD_COIN);
            Destroy(cloneCoin);
        });
        return cloneCoin;
    }
}
