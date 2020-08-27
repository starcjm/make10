using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScore : MonoBehaviour
{
    public Text score;

    public void SetAddScore(int addScore)
    {
        score.text = string.Format("+{0}", addScore);
    }
}
