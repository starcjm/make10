using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//선택 오브젝트 움직임 제어
public class BlockMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //시작 좌표
    private Vector3 oriPos;

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
        var blockObject = transform.GetComponentsInChildren<GameObject>();
        for(int i = 0; i < blockObject.Length; ++i)
        {

        }

        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, transform.forward,
        float.NaN, LayerMask.GetMask("Grid"));

        if (rayHit)
        {
            GridData gridData = rayHit.transform.GetComponent<GridData>();
            //빈공간이라면 그곳에 새로운 블럭 오브젝트 생성하고 자신 파괴
            if(gridData.blockType == E_BLOCK_TYPE.NONE)
            {
                //현재 타겟블록 shape 검사해서 동시에 여러개 검사
                var blockDatas = transform.GetComponentsInChildren<BlockData>();
                for(int i = 0; i < blockDatas.Length; ++i)
                {
                    var blockData = blockDatas[i];
                    GameManager.Instance.CreateGridOverBlock(rayHit.transform.position, blockData.type);
                    gridData.blockType = blockData.type;
                }
                //그리드에 드랍이 된다면 새로운 오브젝트 생성 테스트
                
                GameManager.Instance.CreateTargetBlock();
                Destroy(this.gameObject);
            }
            else
            {
                PosReset();
            }
        }
        else
        {
            //그리드 외에 공간이거나 그리드안에 이미 오브젝트가 잇을경우
            PosReset();
        }
    }

    private void PosReset()
    {
        transform.position = oriPos;
    }
}
