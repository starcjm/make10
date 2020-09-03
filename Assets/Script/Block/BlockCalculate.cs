using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 블럭 머지 연산 클래스
/// </summary>
public class BlockCalculate
{
    //시작 머지 타겟 
    private Block startBlock; 

    //머지 데이터 int = key
    private List<Block> mergeBlock = new List<Block>();

    //머지 움직임 시작한 데이터
    private List<Block> moveBlock = new List<Block>();

    //머지 움직임 완료한 데이터
    private List<Block> moveCompleteBlock = new List<Block>();

    //스타 블럭 주변 블럭
    private List<Block> starBlockEffect = new List<Block>();

    private bool isLast = false;

    //블록 움직임 시간
    private readonly float moveTime = 0.08f;

    public void SetStartBlock(Block block)
    {
        startBlock = block;
        startBlock.data.mergeLast = true;
    }

    public List<Block> GetMergeData()
    {
        return mergeBlock;
    }

    public List<Block> GetStarBlockEffect()
    {
        return starBlockEffect;
    }

    //사방향으로 같은 블록이 있나 검사
    public void CheckBlock(BlockData blockData, bool onlyCheck)
    {
        int nearCount = 0;
        //위
        if (blockData.row > 1)
        {
            if(AddMergeData(CreateCheckData(0, -1, blockData), onlyCheck))
            {
                ++nearCount;
            }
        }

        //아래
        if (blockData.row < Const.GRID_COLUMN_COUNT)
        {
            if (AddMergeData(CreateCheckData(0, 1, blockData), onlyCheck))
            {
                ++nearCount;
            }
        }

        //왼쪽
        if (blockData.column > 1)
        {
            if (AddMergeData(CreateCheckData(-1, 0, blockData), onlyCheck))
            {
                ++nearCount;
            }
        }

        //오른쪽
        if (blockData.column < Const.GRID_ROW_COUNT)
        {
            if (AddMergeData(CreateCheckData(1, 0, blockData), onlyCheck))
            {
                ++nearCount;
            }
        }

        //검사용인지 실제 데이터인지 체크 플래그
        if(!onlyCheck)
        {
            //시작 블록이 아니고 근처에 블록이 있는지 체크

            if (blockData.column != startBlock.data.column
             || blockData.row != startBlock.data.row)
            {
                if (nearCount == 0)
                {
                    blockData.mergeLast = true;
                }
            }
        }
    }

    //머지될떄 움직이고 비활성화
    //public int MergeBlockLastCheck()
    //{
    //    //스타트 블럭 포함해서 계산
    //    if(mergeBlock.Count + 1 >= BlockDefine.MERGE_COUNT)
    //    {
    //        for (int i = 0; i < mergeBlock.Count; ++i)
    //        {
    //            var block = mergeBlock[i];
    //            if (block.data.mergeLast)
    //            {
    //                //마지막 블록이라면 같은 블록 같이 잇는 옆 칸으로 이동
    //                MergeBlockNearMove(block);
    //            }
    //        }
    //    }
    //    return mergeBlock.Count;
    //}

    public bool MergeBlockLastCheck()
    {
        //스타트 블럭 포함해서 계산
        if (mergeBlock.Count + 1 >= BlockDefine.MERGE_COUNT)
        {
            for (int i = 0; i < mergeBlock.Count; ++i)
            {
                var block = mergeBlock[i];
                if (block.data.mergeLast)
                {
                    //마지막 블록이라면 같은 블록 같이 잇는 옆 칸으로 이동
                    MergeBlockNearMove(block);
                }
            }
            return true;
        }
        return false;
    }

    public void MergeBlockNearMove(Block block)
    {
        if (!moveBlock.Contains(block))
        {
            for (int i = mergeBlock.Count - 1; i >= 0; --i)
            {
                var nextBlock = mergeBlock[i];
                if (block != nextBlock)
                {
                    if(!nextBlock.data.mergeLast)
                    {
                        if (NearCheck(block, nextBlock))
                        {
                            moveBlock.Add(block);
                            var tween = block.gameObject.transform.DOMove(nextBlock.transform.position, moveTime);
                            tween.OnComplete(() =>
                            {
                                moveCompleteBlock.Add(block);
                                nextBlock.data.mergeLast = true;
                                block.gameObject.SetActive(false);
                                if(mergeBlock.Count > moveCompleteBlock.Count)
                                {
                                    MergeBlockNearMove(nextBlock);
                                }
                            });
                            return;
                        }
                    }
                }
            }
            //갈대가 없으므로 마지막 블록을 향해 가야댐
            MoveLastBlock(block);
        }
    }

    private void MoveLastBlock(Block block)
    {
        moveBlock.Add(block);
        var lastTween = block.gameObject.transform.DOMove(startBlock.transform.position, moveTime);
        lastTween.OnComplete(() =>
        {
            moveCompleteBlock.Add(block);
            if (moveCompleteBlock.Count >= mergeBlock.Count)
            {
                if(!isLast)
                {
                    isLast = true;
                    mergeBlock.Add(startBlock);
                    GameManager.Instance.MergeCompleteRemoveblock(mergeBlock, startBlock, this);
                }
            }
        });
    }
    
    private bool NearCheck(Block preBlock, Block nearBlock)
    {
        //머지될 블록중에 근처 인지 체크
        //위
        if(nearBlock.data.column == preBlock.data.column
        && nearBlock.data.row == preBlock.data.row -1)
        {
            return true;
        }
        //아래
        else if (nearBlock.data.column == preBlock.data.column
              && nearBlock.data.row == preBlock.data.row + 1)
        {
            return true;
        }
        //왼쪽
        else if (nearBlock.data.column == preBlock.data.column - 1
              && nearBlock.data.row == preBlock.data.row)
        {
            return true;
        }
        //오른쪽
        else if (nearBlock.data.column == preBlock.data.column + 1
              && nearBlock.data.row == preBlock.data.row )
        {
            return true;

        }
        return false;
    }


    //머지 오브젝트 체크
    private bool AddMergeData(BlockData blockData, bool onlyCheck)
    {
        var blockObject = GameManager.Instance.GetBlockObject();
        if (blockObject.ContainsKey(blockData.key))
        {
            Block block = blockObject[blockData.key].GetComponent<Block>();
            if(startBlock != block)
            {
                if (!mergeBlock.Contains(block))
                {
                    if (blockObject.ContainsKey(blockData.key))
                    {
                        if (block.data.blockType == blockData.blockType)
                        {
                            mergeBlock.Add(block);
                            CheckBlock(block.data, onlyCheck);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void StarBlockEffect()
    {
        for(int i = 0; i < mergeBlock.Count; ++i)
        {
            StarBlockCheck(mergeBlock[i].data.key);
        }
    }

    //별 블럭 부셔질때 블럭 검사
    private void StarBlockCheck(int key)
    {
        var gridObject = GameManager.Instance.GetGridObject();
        if (gridObject.ContainsKey(key))
        {
            var grid = gridObject[key].GetComponent<Grid>();
            GridData gridData = grid.data;

            //위
            if (gridData.row > 1)
            {
                StarBlockEffectCheck(CreateCheckData(0, -1, gridData));
            }

            //아래
            if (gridData.row < Const.GRID_ROW_COUNT)
            {
                StarBlockEffectCheck(CreateCheckData(0, 1, gridData));
            }

            //왼쪽
            if (gridData.column > 1)
            {
                StarBlockEffectCheck(CreateCheckData(-1, 0, gridData));
            }

            //오른쪽
            if (gridData.column < Const.GRID_COLUMN_COUNT)
            {
                StarBlockEffectCheck(CreateCheckData(1, 0, gridData));
            }
            
            ////왼쪽 위
            //if (gridData.column > 1 && gridData.row > 1)
            //{
            //    StarBlockEffectCheck(CreateCheckData(-1, -1, gridData));
            //}
            
            ////오른쪽 위
            //if (gridData.column < Const.GRID_COLUMN_COUNT && gridData.row > 1)
            //{
            //    StarBlockEffectCheck(CreateCheckData(1, -1, gridData));
            //}

            ////왼쪽 아래
            //if (gridData.column > 1 && gridData.row < Const.GRID_ROW_COUNT)
            //{
            //    StarBlockEffectCheck(CreateCheckData(-1, 1, gridData));
            //}

            ////오른쪽 아래
            //if (gridData.column < Const.GRID_COLUMN_COUNT && gridData.row < Const.GRID_ROW_COUNT)
            //{
            //    StarBlockEffectCheck(CreateCheckData(1, 1, gridData));
            //}
        }
    }

    //스타 블록 한 개당 사라질 주위블록 체크
    private void StarBlockEffectCheck(GridData gridData)
    {
        var blockObject = GameManager.Instance.GetBlockObject();
        if (blockObject.ContainsKey(gridData.key))
        {
            Block block = blockObject[gridData.key].GetComponent<Block>();
            if (block.data.blockType != E_BLOCK_TYPE.NONE)
            {
                starBlockEffect.Add(block);
            }
        }
    }

    //4방향 머지 될 블럭 검사
    private BlockData CreateCheckData(int addColumn, int addRow, BlockData blockData)
    {
        BlockData tempBlockData = new BlockData
        {
            column = blockData.column + addColumn,
            row = blockData.row + addRow,
            blockType = blockData.blockType,
            key = BlockDefine.GetGridKey(blockData.column + addColumn, blockData.row + addRow),
        };
        return tempBlockData;
    }

    //별블록 용 머지 블록 데이터
    private GridData CreateCheckData(int addColumn, int addRow, GridData gridData)
    {
        GridData tempBlockData = new GridData
        {
            column = gridData.column + addColumn,
            row = gridData.row + addRow,
            blockType = gridData.blockType,
            key = BlockDefine.GetGridKey(gridData.column + addColumn, gridData.row + addRow),
        };
        return tempBlockData;
    }

    //이어하기 할때 중앙 9개 블록 삭제
    public List<int> ContinueDestroyBlock()
    {
        List<int> removeBlockData = new List<int>();
        //중앙 좌표
        Vector2 targetPos = new Vector2(math.ceil((float)Const.GRID_COLUMN_COUNT / 2.0f)
                                      , math.ceil((float)Const.GRID_ROW_COUNT / 2.0f));


        int key = BlockDefine.GetGridKey(targetPos);
        removeBlockData.Add(key);

        //8방향 추가
        removeBlockData.Add(ContinueDestroyBlockKey(targetPos, 0, -1));
        removeBlockData.Add(ContinueDestroyBlockKey(targetPos, 0, 1));
        removeBlockData.Add(ContinueDestroyBlockKey(targetPos, 1, 0));
        removeBlockData.Add(ContinueDestroyBlockKey(targetPos, -1, 0));

        removeBlockData.Add(ContinueDestroyBlockKey(targetPos, -1, -1));
        removeBlockData.Add(ContinueDestroyBlockKey(targetPos, -1, 1));
        removeBlockData.Add(ContinueDestroyBlockKey(targetPos, 1, -1));
        removeBlockData.Add(ContinueDestroyBlockKey(targetPos, 1, 1));

        return removeBlockData;
    }

    //이어하기 할떄 체크할 데이터
    public int ContinueDestroyBlockKey(Vector2 pos, int column, int row)
    {
        int key = BlockDefine.GetGridKey((int)pos.x + column, (int)pos.y + row);
        return key;
    }

    //현재 모양 블록과 빈공간 검사해서 게임 종료 체크
    public bool GameOverCheck(E_BLOCK_SHAPE_TYPE shapeType)
    {
        var gridObjects = GameManager.Instance.GetGridObject();
        foreach (GameObject gridObject in gridObjects.Values)
        {
            var grid = gridObject.GetComponent<Grid>();
            if(grid.data.blockType == E_BLOCK_TYPE.NONE)
            {
                if (GameOverCalc(shapeType, grid.data))
                {
                    return false;
                }
            }
        }
        return true;
    }


    //게임 오버 체크
    private bool GameOverCalc(E_BLOCK_SHAPE_TYPE type, GridData gridData)
    {
        if (type == E_BLOCK_SHAPE_TYPE.ONE)
        {
            if (gridData.blockType == E_BLOCK_TYPE.NONE)
            {
                return true;
            }
            else return false;
        }
        //4방향 1칸 더 있으면 사용가능
        else if (type == E_BLOCK_SHAPE_TYPE.TWO)
        {
            //위
            bool isNone = false;
            if (gridData.row > 1)
            {
                if (GridCheckNoneType(gridData.column, gridData.row - 1))
                {
                    isNone = true;
                }
            }
            //아래
            if (gridData.row < Const.GRID_ROW_COUNT)
            {
                if (GridCheckNoneType(gridData.column, gridData.row + 1))
                {
                    isNone = true;
                }
            }
            //왼족
            if (gridData.column > 1)
            {
                if (GridCheckNoneType(gridData.column - 1, gridData.row))
                {
                    isNone = true;
                }
            }
            //오른쪽
            if (gridData.column < Const.GRID_COLUMN_COUNT)
            {
                if (GridCheckNoneType(gridData.column + 1, gridData.row))
                {
                    isNone = true;
                }
            }
            return isNone;
        }

        //ㄱ자모양 한개라도 있으면 가능
        else if (type == E_BLOCK_SHAPE_TYPE.THREE)
        {
            bool isNone = false;
            //위, 왼쪽  
            if (gridData.row > 1 && gridData.column > 1)
            {
                if (GridCheckNoneType(gridData.column, gridData.row - 1)
                && GridCheckNoneType(gridData.column - 1, gridData.row))
                {
                    isNone = true;
                }
            }
            //위, 오른쪽
            if (gridData.row > 1 && gridData.column < Const.GRID_COLUMN_COUNT)
            {
                if (GridCheckNoneType(gridData.column, gridData.row - 1)
                && GridCheckNoneType(gridData.column + 1, gridData.row))
                {
                    isNone = true;
                }
            }
            //아래, 왼쪽
            if (gridData.row < Const.GRID_ROW_COUNT && gridData.column > 1)
            {
                if (GridCheckNoneType(gridData.column, gridData.row + 1)
                && GridCheckNoneType(gridData.column - 1, gridData.row))
                {
                    isNone = true;
                }
            }
            //아래 오른쪽
            if (gridData.row < Const.GRID_ROW_COUNT && gridData.column < Const.GRID_COLUMN_COUNT)
            {
                if (GridCheckNoneType(gridData.column, gridData.row + 1)
                && GridCheckNoneType(gridData.column + 1, gridData.row))
                {
                    isNone = true;
                }
            }
            return isNone;
        }
        else return false;
    }

    private bool GridCheckNoneType(int column, int row)
    {
        int key = BlockDefine.GetGridKey(column, row);
        var gridObjects = GameManager.Instance.GetGridObject();
        if(gridObjects.ContainsKey(key))
        {
            var grid = gridObjects[key].GetComponent<Grid>();
            if(grid.data.blockType == E_BLOCK_TYPE.NONE)
            {
                return true;
            }
        }
        return false;
    }
}
