using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 레벨업 팝업
/// </summary>
public class PopupLevelUp : MonoBehaviour
{
    public void OnTouchLevelUp()
    {
        gameObject.SetActive(false);
    }
    
}
