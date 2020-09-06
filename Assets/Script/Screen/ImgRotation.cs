using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImgRotation : MonoBehaviour
{
    public float rotateSpeed = 0.1f;
    void FixedUpdate()
    {
        if(GameManager.Instance.GetState() == E_GAME_STATE.GAME)
        {
            transform.Rotate(new Vector3(0.0f, 0.0f, -rotateSpeed));
        }
    }
}
