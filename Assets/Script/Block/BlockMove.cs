using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//선택 오브젝트 움직임 제어
public class BlockMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //시작 좌표
    private Vector3 oriPos;

    private List<Transform> tempGrids = new List<Transform>();

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
            //블록 갯수 중에 하나라도 안맞는다면 리셋
            DataReset();
        }



        //RaycastHit2D rayHit = Physics2D.Raycast(transform.position, transform.forward,
        //float.NaN, LayerMask.GetMask("Grid"));

        //if (rayHit)
        //{
        //    GridData gridData = rayHit.transform.GetComponent<GridData>();
        //    //빈공간이라면 그곳에 새로운 블럭 오브젝트 생성하고 자신 파괴
        //    if(gridData.blockType == E_BLOCK_TYPE.NONE)
        //    {
        //        //현재 타겟블록 shape 검사해서 동시에 여러개 검사
        //        var blockDatas = transform.GetComponentsInChildren<BlockData>();
        //        for(int i = 0; i < blockDatas.Length; ++i)
        //        {
        //            var blockData = blockDatas[i];
        //            GameManager.Instance.CreateGridOverBlock(rayHit.transform.position, blockData.type);
        //            gridData.blockType = blockData.type;
        //        }
        //        //그리드에 드랍이 된다면 새로운 오브젝트 생성 테스트
                
        //        GameManager.Instance.CreateTargetBlock();
        //        Destroy(this.gameObject);
        //    }
        //    else
        //    {
        //        PosReset();
        //    }
        //}
        //else
        //{
        //    //그리드 외에 공간이거나 그리드안에 이미 오브젝트가 잇을경우
        //    PosReset();
        //}
    }

    private void DataReset()
    {
        transform.position = oriPos;
        tempGrids.Clear();
    }
}
