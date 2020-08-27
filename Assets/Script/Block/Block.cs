using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//블럭 랜더 클래스
public class Block : MonoBehaviour
{
    public readonly float hammerTime = 0.4f;

    public GameObject imgX;
    public GameObject hammer;
    public BlockData data = new BlockData();

    //망치 아이템 쓸대 보여줄 x이미지
    public void ShowImgX(bool on)
    {
        imgX.SetActive(on);
    }

    public void OnTouchX()
    {
        GameManager.Instance.ShowBlockX();
        CreateHammer();
        StartCoroutine(HammerTouchDelay());
    }

    public void CreateHammer()
    {
        GameObject cloneGrid = (GameObject)Instantiate(hammer);
        cloneGrid.name = string.Format("Hammer");
        cloneGrid.transform.SetParent(GameManager.Instance.blockLayer.transform);
        cloneGrid.transform.localScale = Vector3.one;
        cloneGrid.transform.position = gameObject.transform.position;
        GameManager.Instance.SetGameState(E_GAME_STATE.GAME);
        DestroyHammer(cloneGrid);
    }

    private void DestroyHammer(GameObject cloneHammer)
    {
        Destroy(cloneHammer, 0.3f);
    }

    IEnumerator HammerTouchDelay()
    {
        yield return new WaitForSeconds(hammerTime);
        int key = BlockDefine.GetGridKey(data.column, data.row);
        GameManager.Instance.RemoveBlockData(key);
        
    }
}
