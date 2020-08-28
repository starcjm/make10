using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// 선택한 모양 블럭 움직임 클래스
/// </summary>
public class BlockMove : MonoBehaviour, IDragHandler, IEndDragHandler, 
                                        IPointerDownHandler, IPointerUpHandler
{
    //알파 블럭 저장 데이터
    private class AlphaBlock
    {
        public int key = 0;
        public E_BLOCK_TYPE blockType = E_BLOCK_TYPE.NONE;
        public GameObject block = null;
    }
    //현재 드래그하고 있는 블록 모양 타입
    public E_BLOCK_SHAPE_TYPE shapeType = E_BLOCK_SHAPE_TYPE.ONE;

    //드래그 블럭 터치 시작 좌표
    private Vector2 touchOriPos;
    //모양 블록 회전용 터치에 대한 up down 거리값
    private float touchRecognitionDis = 0.05f;
    //모양 블록 터치 초기화 시간
    private float touchInitTime = 0.15f;
    //누르고 있을때 모양 블록 터치 초기화값
    private bool isTouch = false;
    //모양 블럭이 드래그 할떄 손에 가려져서 살짝 위로
    private float dragYDelta = 1.0f;
    //선행 블록일때는 터지 안되야댐
    private bool isTouchFlag = false;
    //회전중 다시 회전 금지
    private bool isRot = false;
    //모양 블럭 회전 시간
    private float rotTime = 0.2f;

    //알파적용된 모양블럭 
    private Dictionary<int, AlphaBlock> alphaShapeBlock = new Dictionary<int, AlphaBlock>();

    public void SetTouchFlag(bool flag)
    {
        isTouchFlag = flag;
    }

    private void TouchInit()
    {
        isTouch = false;
    }

    private bool CheckGameState()
    {
        if(GameManager.Instance.GetState() == E_GAME_STATE.GAME)
        {
            return true;
        }
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TouchStart(eventData.position);
    }

    public void TouchStart(Vector3 touchPos)
    {
        if(!CheckGameState())
        {
            return;
        }
        if (isTouchFlag)
        {
            if (shapeType != E_BLOCK_SHAPE_TYPE.ONE)
            {
                isTouch = true;
                if (isTouch)
                {
                    Invoke("TouchInit", touchInitTime);
                }
                Vector3 pos = Camera.main.ScreenToWorldPoint(touchPos);
                pos.z = 0;
                touchOriPos = pos;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CheckGameState())
        {
            return;
        }
        BlockRotate(eventData.position);
    }

    private void BlockRotate(Vector3 touchPos)
    {
        if (!CheckGameState())
        {
            return;
        }
        if (isTouchFlag)
        {
            //타겟 블럭 터치 할떄 -90도씩 회전
            if (!isRot && isTouch && shapeType != E_BLOCK_SHAPE_TYPE.ONE)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(touchPos);
                pos.z = 0;
                float tempDis = math.distance(touchOriPos, (Vector2)pos);
                if (tempDis < touchRecognitionDis)
                {
                    //터치 했을때 모양 블록 회전
                    isRot = true;
                    var tween = TweenRot(transform, rotTime, new Vector3(0.0f, 0.0f, -90.0f));
                    tween.OnUpdate(() =>
                    {
                        for (int i = 0; i < transform.childCount; ++i)
                        {
                            var block = transform.GetChild(i);
                            //하위 오브젝트의 방향은 위
                            block.rotation = quaternion.Euler(0.0f, 0.0f, 0.0f);
                        }
                    });
                    tween.OnComplete(() =>
                    {
                        isRot = false;
                    });
                }
            }
        }
    }

    private DG.Tweening.Core.TweenerCore<Quaternion, Vector3, DG.Tweening.Plugins.Options.QuaternionOptions> TweenRot(Transform transform, float time, Vector3 rotation)
    {
        return TweenRot(transform, time, rotation, RotateMode.Fast);
    }

    private DG.Tweening.Core.TweenerCore<Quaternion, Vector3, DG.Tweening.Plugins.Options.QuaternionOptions> TweenRot(Transform transform, float time, Vector3 rotation, RotateMode mode)
    {
        Vector3 rot = rotation;
        rot += transform.eulerAngles;
        return transform.DORotate(rot, time, mode);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CheckGameState())
        {
            return;
        }
        DragMove(eventData.position);
    }

    private void DragMove(Vector3 touchPos)
    {
        if (!CheckGameState())
        {
            return;
        }
        if (isTouchFlag)
        {
            if (!isTouch)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(touchPos);
                pos.z = 0;
                pos.y += dragYDelta;
                transform.position = pos;
                CheckAlphaBlock();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CheckGameState())
        {
            return;
        }
        CheckDropShapeObject();
        DestroyAlphaShapeBlock();
    }

    private void CheckAlphaBlock()
    {
        List<GameObject> tempGrids = new List<GameObject>();
        //타켓 블록과 그리드공간에 매칭 검사
        GridCheck(ref tempGrids);
        //알파 블록이 이미 배치 되있다면 패스
        if(AlphaBlockCheck(tempGrids))
        {
            return;
        }
        else
        {
            DestroyAlphaShapeBlock();
        }

        //모양 블록 갯수와 그리드 빈공간의 갯수가 일치 하다면 그리드에 알파 블록
        if (tempGrids.Count == transform.childCount)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                var block = transform.GetChild(i).GetComponent<Block>();
                if (block)
                {
                    Grid grid = tempGrids[i].GetComponent<Grid>();
                    if (grid)
                    {
                        //외부에서 그리드에 올릴 블럭 타입 설정 하고 생성
                        var key = BlockDefine.GetGridKey(grid.data.column, grid.data.row);
                        var alphaBlock = GameManager.Instance.CreateAlphaShapeBlock(block.gameObject,
                                                                                    tempGrids[i].transform.position);
                        AlphaBlock alphaBlockData = new AlphaBlock
                        {
                            key = key,
                            blockType = block.data.blockType,
                            block = alphaBlock
                        };
                        alphaShapeBlock.Add(key, alphaBlockData);
                    }
                }
            }
        }
    }

    //현재 그리드에 알파 블록이 현재 모양 블록하고 같은지 체크
    private bool AlphaBlockCheck(List<GameObject> tempGrids)
    {
        int equalCount = 0;
        if (tempGrids.Count > 0)
        {
            if (alphaShapeBlock.Count <= 0)
            {
                return false;
            }
            for (int i = 0; i < transform.childCount; ++i)
            {
                var block = transform.GetChild(i).GetComponent<Block>();
                if (block)
                {
                    if(i < tempGrids.Count)
                    {
                        Grid grid = tempGrids[i].GetComponent<Grid>();
                        if (grid)
                        {
                            int key = BlockDefine.GetGridKey(grid.data.column, grid.data.row);
                            if (alphaShapeBlock.ContainsKey(key))
                            {
                                if (alphaShapeBlock[key].blockType == block.data.blockType)
                                {
                                    equalCount++;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        if(equalCount == alphaShapeBlock.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //그리드 빈공간 체크
    private void GridCheck(ref List<GameObject> tempGrids)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform block = transform.GetChild(i);
            RaycastHit2D rayHit = Physics2D.Raycast(block.position, block.forward,
            float.NaN, LayerMask.GetMask("Grid"));
            if (rayHit)
            {
                Grid grid = rayHit.transform.GetComponent<Grid>();
                //빈공간이라면 그리드 데이터 수집
                if (grid.data.blockType == E_BLOCK_TYPE.NONE)
                {
                    tempGrids.Add(rayHit.transform.gameObject);
                }
            }
        }
    }

    //빈 그리드에 모양 블럭 배치
    private void DropObject(List<GameObject> tempGrids)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var block = transform.GetChild(i).GetComponent<Block>();
            if (block)
            {
                Grid grid = tempGrids[i].GetComponent<Grid>();
                if (grid)
                {
                    //그리드에 올릴 블럭 타입 설정 하고 생성
                    grid.data.blockType = block.data.blockType;
                    GameManager.Instance.CreateGridOverBlock(grid.data, tempGrids[i].transform.position);
                }
            }
        }
    }

    //블록 매칭
    private void BlockMatching(List<GameObject> tempGrids)
    {
        tempGrids.Sort(delegate (GameObject A, GameObject B)
        {
            GridData gridA = A.GetComponent<Grid>().data;
            GridData gridB = B.GetComponent<Grid>().data;
            if(gridA.blockType > gridB.blockType)
            {
                return 1;
            }
            else if(gridA.blockType > gridB.blockType)
            {
                return -1;
            }
            return 0;
        });
        GameManager.Instance.MergeDelayCheck(tempGrids);
    }

    private void CheckDropShapeObject()
    {
        if (isTouchFlag)
        {
            //현재 드래그 하고 있는 블록들 검사용 공간
            List<GameObject> tempGrids = new List<GameObject>();
            //모양 블록과 그리드공간에 매칭 검사
            GridCheck(ref tempGrids);
            //모양 블록 갯수와 그리드 빈공간의 갯수가 일치 하다면 그리드에 블록 배치
            if (tempGrids.Count == transform.childCount)
            {
                //모양 블록 배치
                DropObject(tempGrids);
                //모양 블록 매칭
                BlockMatching(tempGrids);

                //드랍이 되면 새 모양 블록 만들어주고 자신 삭제
                CreateNextShapeBlock();
                Destroy(this.gameObject);
            }
            else
            {
                //블록 중에 하나라도 안맞는다면 리셋
                DragDataReset();
            }
        }
    }
    
    //현재 모양 블록 위치가 바뀌면 알파 블록삭제
    private void DestroyAlphaShapeBlock()
    {
        foreach(var alphaBlock in alphaShapeBlock.Values)
        {
            if(alphaBlock.block)
            {
                Destroy(alphaBlock.block);
            }
        }
        alphaShapeBlock.Clear();
    }

    public void CreateNextShapeBlock()
    {
        GameManager.Instance.NextShapeBlock(); 
    }

    //드래그 후에 데이터 초기화
    private void DragDataReset()
    {
        transform.position = GameManager.Instance.shapeBlockPos.transform.position;
        touchOriPos = Vector3.zero;
    }
}
