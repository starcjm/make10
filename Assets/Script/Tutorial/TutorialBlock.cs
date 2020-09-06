using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
                                        IPointerDownHandler, IPointerUpHandler
{
    //모양 블럭이 드래그 할떄 손에 가려져서 살짝 위로
    private float dragYDelta = 1.0f;

    private Vector3 pos;

    public void OnPointerDown(PointerEventData eventData)
    {
        pos = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (TutorialManager.Instance.tutorialIndex == TutorialManager.E_TUTORIAL.INDEX_3)
        {
            if(Vector3.Distance(pos, eventData.position) < 30.0f)
            {
                TutorialManager.Instance.Page2Tap();
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (TutorialManager.Instance.tutorialIndex == TutorialManager.E_TUTORIAL.INDEX_1
         || TutorialManager.Instance.tutorialIndex == TutorialManager.E_TUTORIAL.INDEX_4)
        {
            SoundManager.Instance.PlaySFX(E_SFX.SHAPE_BLOCK_UP);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(TutorialManager.Instance.tutorialIndex == TutorialManager.E_TUTORIAL.INDEX_1 
         ||TutorialManager.Instance.tutorialIndex == TutorialManager.E_TUTORIAL.INDEX_4)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
            pos.z = 0;
            pos.y += dragYDelta;
            transform.position = pos;
            transform.localScale = Vector3.one;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (TutorialManager.Instance.tutorialIndex == TutorialManager.E_TUTORIAL.INDEX_1
         || TutorialManager.Instance.tutorialIndex == TutorialManager.E_TUTORIAL.INDEX_4)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, transform.forward,
            float.NaN, LayerMask.GetMask("Grid"));
            if (rayHit)
            {
                if (TutorialManager.Instance.tutorialIndex == TutorialManager.E_TUTORIAL.INDEX_1)
                {
                    transform.position = rayHit.transform.position;
                    TutorialManager.Instance.Page1Complete(this.transform);
                }
                else if(TutorialManager.Instance.tutorialIndex == TutorialManager.E_TUTORIAL.INDEX_4)
                {
                    TutorialManager.Instance.page2Complete(this.transform);
                }
            }
            else
            {
                if (TutorialManager.Instance.tutorialIndex == TutorialManager.E_TUTORIAL.INDEX_1)
                {
                    transform.position = TutorialManager.Instance.page1StartPos.position;
                }
                else if (TutorialManager.Instance.tutorialIndex == TutorialManager.E_TUTORIAL.INDEX_4)
                {
                    transform.position = TutorialManager.Instance.page2StartPos.position;
                }
            }
        }
    }

    
}
