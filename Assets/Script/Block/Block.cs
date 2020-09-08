using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 블럭 클래스
/// </summary>
public class Block : MonoBehaviour
{
    public readonly float hammerTime = 0.4f;

    public Image mainImg;
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
        GameManager.Instance.AddCoin(-Const.HAMMER_PRICE);
        GameManager.Instance.GetMainScreen().SetCoin(UserInfo.Instance.Coin);
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
        SoundManager.Instance.PlaySFX(E_SFX.ITEM_HAMMER);
        DestroyHammer(cloneGrid);
    }

    private void DestroyHammer(GameObject cloneHammer)
    {
        Destroy(cloneHammer, 0.3f);
    }

    IEnumerator HammerTouchDelay()
    {
        yield return new WaitForSeconds(hammerTime);
        GameManager.Instance.RemoveBlockData(data.key);
    }

    public void ChangeImage(E_BLOCK_TYPE type)
    {
        mainImg.sprite = BlockGenerator.Instance.blockSprite[(int)type - 1];
    }
}
