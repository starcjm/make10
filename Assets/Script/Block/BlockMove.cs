using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

//선택 오브젝트 움직임 제어
public class BlockMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, 
                                        IPointerDownHandler, IPointerUpHandler
{
    //시작 좌표
    private Vector2 oriPos;

    private Vector2 touchOriPos;

    public E_BLOCK_SHAPE_TYPE shapeType = E_BLOCK_SHAPE_TYPE.ONE;
    
    private List<Transform> tempGrids = new List<Transform>();

    public void OnPointerDown(PointerEventData eventData)
    {
        if(shapeType != E_BLOCK_SHAPE_TYPE.ONE)
        {
            touchOriPos = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //타겟 블럭 터치 할떄 -90도씩 회전
        if (shapeType != E_BLOCK_SHAPE_TYPE.ONE)
        {
            if (touchOriPos == eventData.position)
            {
                Vector3 rot = new Vector3(0.0f, 0.0f, -90.0f);
                transform.Rotate(rot, Space.Self);
                touchOriPos = Vector3.zero;
                var blocks = transform.GetComponentsInChildren<Transform>();
                for(int i = 0; i < blocks.Length; ++i)
                {
                    if(blocks[i] != this.transform)
                    {
                        //하위 개체는 회전 반대값
                        Vector3 rRot = new Vector3(0.0f, 0.0f, 90.0f);
                        blocks[i].Rotate(rRot, Space.Self);
                    }
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        oriPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //타켓 블록과 그리드공간에 매칭 검사
        var blocks = transform.GetComponentsInChildren<Transform>();
        for (int i = 0; i < blocks.Length; ++i)
        {
            Transform block = blocks[i];
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
        if(tempGrids.Count == blocks.Length)
        {
            //todo 유효성 검사를 해야 할지도
            for(int i = 0; i < blocks.Length; ++i)
            {
                var blockData = blocks[i].GetComponent<BlockData>();
                if(blockData)
                {
                    GridData gridData = tempGrids[i].GetComponent<GridData>();
                    if (gridData)
                    {
                        gridData.blockType = blockData.type;
                        GameManager.Instance.CreateGridOverBlock(tempGrids[i].position, blockData.type);
                    }
                }
            }
            //매칭이 잘되엇다면 새 타겟 블록 만들어주고 자신 삭제
            GameManager.Instance.CreateTargetBlock();
            Destroy(this.gameObject);
        }
        else
        {
            //블록 중에 하나라도 안맞는다면 리셋
            DataReset();
        }
    }

    private void DataReset()
    {
        transform.position = oriPos;
        tempGrids.Clear();
    }
}
