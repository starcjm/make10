using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRot : MonoBehaviour
{
    public float rotateSpeed = 0.1f;

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, -rotateSpeed));
    }
}
