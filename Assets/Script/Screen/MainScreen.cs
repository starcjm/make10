using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    public void ResetBlock()
    {
        GameManager.Instance.ResetScene();
    }

    public void ChangeShapeBlock()
    {
        GameManager.Instance.ChangeShapeBlock();
    }

    public void GameClose()
    {
        GameManager.Instance.GameClose();
    }
}
