using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 컨티뉴 팝업
/// </summary>
public class PopupContinue : MonoBehaviour
{
    public void OnTouchContinue()
    {
        //이어하기 했을때 가운데 3 * 3블럭 삭제
        GameManager.Instance.ContinueDestryBlock();
        gameObject.SetActive(false);
        GameManager.Instance.SetGameState(E_GAME_STATE.GAME);
    }

    public void OnTouchReset()
    {
        GameManager.Instance.Retry();
        gameObject.SetActive(false);
    }
}
