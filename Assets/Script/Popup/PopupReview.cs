using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupReview : PopupBase
{
    public override void OnTouchAndroidBackButton()
    {
        OnTouchNotNow();
    }

    public void OnTouchNotNow()
    {
        if (!UserInfo.Instance.IsReviewNoFirst())
        {
            UserInfo.Instance.ReviewNoFirst = (int)UserInfo.E_REVIEW.YES;
        }
        UserInfo.Instance.ReviewCount = 0;
        gameObject.SetActive(false);
    }

    public void OnTouchLetsGo()
    {
        UserInfo.Instance.ReviewOk = (int)UserInfo.E_REVIEW.YES;
        //todo 각 스토어 연결 해야댐
        gameObject.SetActive(false);
    }
}
