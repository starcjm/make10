using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    public enum E_TUTORIAL
    {
        INDEX_1 = 0,
        INDEX_2,
        INDEX_3,
        INDEX_4,
        INDEX_5,
    }

    [Header("튜토리얼 1페이지 데이터")]
    //page1 데이터
    public Transform page1MoveEndBlock;
    public Transform page1MoveBlock1;
    public Transform page1MoveBlock2;
    public Transform page1Finger1;

    public Transform page1StartPos;

    public Transform page1CreatBlock;

    [Header("튜토리얼 2페이지 데이터")]
    //page2 데이터
    public Transform page2TargetBlock1;
    public Transform page2TargetBlock2;
    public Transform page2MoveBlock1;
    public Transform page2MoveBlock2;
    public Transform page2GridTarget;
    public Transform page2BlockTarget;
    public Transform page2MoveEndBlock;
    public Transform page2CreatBlock1;
    public Transform page2CreatBlock2;

    public Transform page2StartPos;

    public GameObject page2Text1;
    public GameObject page2Text2;
    public GameObject page2Finger1;
    public GameObject page2Finger2;
    public GameObject page2Tap;

    public GameObject TutorialPage1;
    public GameObject TutorialPage2;
    public GameObject completePopup;

    public E_TUTORIAL tutorialIndex = E_TUTORIAL.INDEX_1;

    private void Start()
    {
        //임시 스플래쉬 씬부터 하면 필요없음
        UserInfo.Instance.LoadUserData();
        SoundManager.Instance.Init();
        SoundManager.Instance.StopBGM();
        MoveFinger1();
    }

    private void MoveFinger1()
    {
        Vector3 endPos = page1MoveEndBlock.position;
        page1Finger1.DOMoveY(endPos.y - 0.6f, 2.0f).SetLoops(-1, LoopType.Restart);
    }

    public void Page1Complete(Transform block)
    {
        tutorialIndex = E_TUTORIAL.INDEX_2;
        SoundManager.Instance.PlaySFX(E_SFX.BLOCK_DROP);
        page1MoveBlock1.DOMoveX(page1MoveEndBlock.position.x, 0.3f).OnComplete(() =>
        {
            SoundManager.Instance.PlaySFX(E_SFX.BLOCK_MERGE);
            block.gameObject.SetActive(false);
            page1MoveEndBlock.gameObject.SetActive(false);
            page1CreatBlock.gameObject.SetActive(true);
            page1MoveBlock1.gameObject.SetActive(false);
            
        });
        page1MoveBlock2.DOMoveX(page1MoveEndBlock.position.x, 0.3f).OnComplete(() =>
        {
            page1MoveBlock2.gameObject.SetActive(false);
        });
        StartCoroutine(pageChange());
    }

    IEnumerator pageChange()
    {
        yield return new WaitForSeconds(1.0f);
        TutorialPage1.SetActive(false);
        TutorialPage2.SetActive(true);
        page1Finger1.transform.DOKill();
        tutorialIndex = E_TUTORIAL.INDEX_3;
    }

    public void Page2Tap()
    {
        SoundManager.Instance.PlaySFX(E_SFX.SHAPE_BLOCK_ROT);
        page2TargetBlock1.gameObject.SetActive(false);
        page2TargetBlock2.gameObject.SetActive(true);

        page2Text1.SetActive(false);
        page2Text2.SetActive(true);

        page2Finger1.SetActive(false);
        page2Finger2.SetActive(true);

        page2Tap.SetActive(false);

        tutorialIndex = E_TUTORIAL.INDEX_4;
        MoveFinger2();
    }

    private void MoveFinger2()
    {
        Vector3 endPos = page2GridTarget.position;
        page2Finger2.transform.DOMoveY(endPos.y - 0.6f, 2.0f).SetLoops(-1, LoopType.Restart);
    }

    public void page2Complete(Transform block)
    {
        tutorialIndex = E_TUTORIAL.INDEX_5;
        SoundManager.Instance.PlaySFX(E_SFX.BLOCK_DROP);
        block.transform.position = page2BlockTarget.transform.position;
        page2MoveBlock1.DOMoveX(page2MoveEndBlock.position.x, 0.3f).OnComplete(() =>
        {
            block.gameObject.SetActive(false);
            SoundManager.Instance.PlaySFX(E_SFX.BLOCK_MERGE);
            page2MoveEndBlock.gameObject.SetActive(false);
            page2CreatBlock1.gameObject.SetActive(true);
            page2CreatBlock2.gameObject.SetActive(true);
            page1MoveBlock1.gameObject.SetActive(false);
            page2Finger2.transform.DOKill();
            Invoke("ShowCompletePopup", 0.5f);
        });
        page2MoveBlock2.DOMoveX(page2MoveEndBlock.position.x, 0.3f).OnComplete(() =>
        {
            page2MoveBlock2.gameObject.SetActive(false);
        });
    }
    
    public void ShowCompletePopup()
    {
        TutorialPage2.SetActive(false);
        completePopup.SetActive(true);
    }

    public void TutorialComplete()
    {
        UserInfo.Instance.Tutorial = (int)UserInfo.E_TUTORIAL_SET.YES;
    }
}

