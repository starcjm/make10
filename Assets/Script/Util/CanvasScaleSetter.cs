/***************************************************************************************
 * HANDLES THE CANVAS SIZE BASED ON DEVICE RESOLUTION.
 ***************************************************************************************/

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasScaleSetter : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance.screenAspect > 1.86F)
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }
    }
}
