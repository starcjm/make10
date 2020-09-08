using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTenBlockDsc : MonoBehaviour
{
    public void OnTouchClose()
    {
        gameObject.SetActive(false);
    }
}
