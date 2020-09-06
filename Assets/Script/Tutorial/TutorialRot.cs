using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 튜토리얼 이미지 회전 클래스
/// </summary>
public class TutorialRot : MonoBehaviour
{
    public float rotateSpeed = 0.1f;

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, -rotateSpeed));
    }
}
