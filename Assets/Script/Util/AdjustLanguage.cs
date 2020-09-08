using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustLanguage : MonoBehaviour
{
    public Text uiText;
    public string korText;

    void Start()
    {
        if(PlayerPrefs.GetString("systemLanguage") == "Korean")
        {
            uiText.text = korText;
        }
    }
}
