using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGameOver : MonoBehaviour
{
    public void OnTouchGameOver()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ShowContinuePopup();
    }
}
