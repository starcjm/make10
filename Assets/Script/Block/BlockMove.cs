using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

//선택 오브젝트 움직임 제어
public class BlockMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, 
                                        IPointerDownHandler, IPointerUpHandler
{  
    //드래그 블럭 터치 시작 좌표
    private Vector2 touchOriPos;

    //현재 드래그하고 있는 블록 모양 타입
    public E_BLOCK_SHAPE_TYPE shapeType = E_BLOCK_SHAPE_TYPE.ONE;

    //모양 블록 회전용 터치에 대한 up down 거리값
    private float touchRecognitionDis = 0.15f;

    //모양 블록 터치 초기화 시간
    private float touchInitTime = 0.15f;

    //누르고 있을때 모양 블록 터치 초기화값
    private bool isTouch = false;
    
    //현재 드래그 하고 있는 블록들 검사용 공간
    private List<Transform> tempGrids = new List<Transform>();


    private void TouchInit()
    {
        isTouch = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(shapeType != E_BLOCK_SHAPE_TYPE.ONE)
        {
            isTouch = true;
            if (isTouch)
            {
                Invoke("TouchInit", touchInitTime);
            }
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
            pos.z = 0;
            touchOriPos = pos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {   
        //타겟 블럭 터치 할떄 -90도씩 회전
        if (isTouch && shapeType != E_BLOCK_SHAPE_TYPE.ONE)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
            pos.z = 0;
            //float tempDis = 0.0f;
            float tempDis = math.distance(touchOriPos, (Vector2)pos);
            if (tempDis < touchRecognitionDis)
            {
                Vector3 rot = new Vector3(0.0f, 0.0f, -90.0f);
                transform.Rotate(rot, Space.Self);
                touchOriPos = Vector3.zero;

                for (int i = 0; i < transform.childCount; ++i)
                {
                    var blocks = transform.GetChild(i);

                    //하위 개체는 회전 반대값 그래야 숫자가 정상적으로 보임
                    Vector3 rRot = new Vector3(0.0f, 0.0f, 90.0f);
                    blocks.Rotate(rRot, Space.Self);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isTouch)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
            pos.z = 0;
            transform.position = pos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //타켓 블록과 그리드공간에 매칭 검사
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform block = transform.GetChild(i);
            RaycastHit2D rayHit = Physics2D.Raycast(block.position, block.forward,
            float.NaN, LayerMask.GetMask("Grid"));
            if (rayHit)
            {
                GridData gridData = rayHit.transform.GetComponent<GridData>();
                //빈공간이라면 그리드 데이터 수집
                if (gridData.blockType == E_BLOCK_TYPE.NONE)
                {
                    tempGrids.Add(rayHit.transform);
                }
            }
        }

        //타겟 블록 갯수와 그리드 빈공간의 갯수가 일치 하다면 그리드에 블록 배치
        if(tempGrids.Count == transform.childCount)
        {
            //todo 유효성 검사를 해야 할지도
            for(int i = 0; i < transform.childCount; ++i)
            {
                var blockData = transform.GetChild(i).GetComponent<BlockData>();
                if(blockData)
                {
                    GridData gridData = tempGrids[i].GetComponent<GridData>();
                    if (gridData)
                    {
                        gridData.blockType = blockData.blockType;
                        GameManager.Instance.CreateGridOverBlock(gridData, tempGrids[i].position);
                    }
                }
            }

            //숫자 매칭 검사
            for(int i = 0; i < tempGrids.Count; ++i)
            {
                GridData gridData = tempGrids[i].GetComponent<GridData>();
                GameManager.Instance.MergeCheck(gridData.column, gridData.row);
            }

            //매칭이 잘되엇다면 새 모양 블록 만들어주고 자신 삭제
            GameManager.Instance.CreateTargetBlock();
            Destroy(this.gameObject);
        }
        else
        {
            //블록 중에 하나라도 안맞는다면 리셋
            DragDataReset();
        }
    }

    //드래그 후에 데이터 초기화
    private void DragDataReset()
    {
        transform.position = GameManager.Instance.targetBlockPos.transform.position;
        touchOriPos = Vector3.zero;
        tempGrids.Clear();
    }
}
