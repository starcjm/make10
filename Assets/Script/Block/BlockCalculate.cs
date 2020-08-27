using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 블럭 머지 연산 클래스
/// </summary>
public class BlockCalculate
{
    //타겟 좌표용 x = column  y = row
    private Vector2 targetPos = new Vector2();
    //머지 데이터 int = key
    private List<int> mergeBlock = new List<int>();

    //스타 블럭 주변 블럭
    private List<int> starBlockEffect = new List<int>();

    public Vector2 GetTargetPos()
    {
        return targetPos;
    }

    public void SetTargetBlock(int column, int row)
    {
        targetPos.x = column;
        targetPos.y = row;
    }

    public List<int> GetMergeData()
    {
        return mergeBlock;
    }

    public List<int> GetStarBlockEffect()
    {
        return starBlockEffect;
    }

    //데이터 머지 후에 초기화
    public void DataClear()
    {
        mergeBlock.Clear();
    }

    //사방향으로 같은 블록이 있나 검사
    public void CheckBlock(BlockData blockData)
    {
        //위
        if (blockData.row > 1)
        {
            AddMergeData(CreateCheckData(0, -1, blockData));
        }

        //아래
        if (blockData.row < Const.GRID_COLUMN_COUNT)
        {
            AddMergeData(CreateCheckData(0, 1, blockData));
        }

        //왼쪽
        if (blockData.column > 1)
        {
            AddMergeData(CreateCheckData(-1, 0, blockData));
        }

        //오른쪽
        if (blockData.column < Const.GRID_ROW_COUNT)
        {
            AddMergeData(CreateCheckData(1, 0, blockData));
        }
    }

    //머지 오브젝트 체크
    private void AddMergeData(BlockData blockData)
    {
        var blockObject = GameManager.Instance.GetBlockObject();
        int key = BlockDefine.GetGridKey(blockData.column, blockData.row);
        if(!mergeBlock.Contains(key))
        {
            if (blockObject.ContainsKey(key))
            {
                Block block = blockObject[key].GetComponent<Block>();
                if (block.data.blockType == blockData.blockType)
                {
                    mergeBlock.Add(key);
                    CheckBlock(blockData);
                }
            }
        }
    }

    public void StarBlockEffect()
    {
        for(int i = 0; i < mergeBlock.Count; ++i)
        {
            StarBlockCheck(mergeBlock[i]);
        }
    }

    //별 블럭 부셔질때는 8방향 검사
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
            
            //왼쪽 위
            if (gridData.column > 1 && gridData.row > 1)
            {
                StarBlockEffectCheck(CreateCheckData(-1, -1, gridData));
            }
            
            //오른쪽 위
            if (gridData.column < Const.GRID_COLUMN_COUNT && gridData.row > 1)
            {
                StarBlockEffectCheck(CreateCheckData(1, -1, gridData));
            }

            //왼쪽 아래
            if (gridData.column > 1 && gridData.row < Const.GRID_ROW_COUNT)
            {
                StarBlockEffectCheck(CreateCheckData(-1, 1, gridData));
            }

            //오른쪽 아래
            if (gridData.column < Const.GRID_COLUMN_COUNT && gridData.row < Const.GRID_ROW_COUNT)
            {
                StarBlockEffectCheck(CreateCheckData(1, 1, gridData));
            }
        }
    }

    //스타 블록으로 인해 사라질 주위블록 체크
    private void StarBlockEffectCheck(GridData gridData)
    {
        var blockObject = GameManager.Instance.GetBlockObject();
        int key = BlockDefine.GetGridKey(gridData.column, gridData.row);
        if (blockObject.ContainsKey(key))
        {
            Block block = blockObject[key].GetComponent<Block>();
            if (block.data.blockType != E_BLOCK_TYPE.NONE)
            {
                starBlockEffect.Add(key);
            }
        }
    }

    private BlockData CreateCheckData(int addColumn, int addRow, BlockData blockData)
    {
        BlockData tempBlockData = new BlockData
        {
            column = blockData.column + addColumn,
            row = blockData.row + addRow,
            blockType = blockData.blockType,
        };
        return tempBlockData;
    }

    private GridData CreateCheckData(int addColumn, int addRow, GridData gridData)
    {
        GridData tempBlockData = new GridData
        {
            column = gridData.column + addColumn,
            row = gridData.row + addRow,
            blockType = gridData.blockType,
        };
        return tempBlockData;
    }

    public int ContinueDestroyBlockKey(Vector2 pos, int column, int row)
    {
        int key = BlockDefine.GetGridKey((int)pos.x + column, (int)pos.y + row);
        return key;
    }

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
