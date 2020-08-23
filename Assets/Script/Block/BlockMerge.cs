using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 블럭 머지 연산 클래스
/// </summary>
public class BlockMerge
{
    //머지 데이터
    private Dictionary<int, BlockData> mergeBlock = new Dictionary<int, BlockData>();

    //사방향으로 같은 블록이 있나 검사
    public void CheckBlock(BlockData blockData)
    {
        //위
        BlockData tempDataUp = new BlockData();
        if (blockData.row > 1)
        {
            tempDataUp.column = blockData.column;
            tempDataUp.row = blockData.row - 1;
            tempDataUp.blockType = blockData.blockType;
            AddMergeData(tempDataUp);
        }

        //아래
        BlockData tempDataDown = new BlockData();
        if (blockData.row < Const.GRID_COLUMN_COUNT)
        {
            tempDataDown.column = blockData.column;
            tempDataDown.row = blockData.row + 1;
            tempDataDown.blockType = blockData.blockType;
            AddMergeData(tempDataDown);
        }

        //왼쪽
        BlockData tempDataLeft = new BlockData();
        if (blockData.column > 1)
        {
            tempDataLeft.column = blockData.column - 1;
            tempDataLeft.row = blockData.row;
            tempDataLeft.blockType = blockData.blockType;
            AddMergeData(tempDataLeft);
        }

        //오른쪽
        BlockData tempDataRight = new BlockData();
        if (blockData.column < Const.GRID_ROW_COUNT)
        {
            tempDataRight.column = blockData.column + 1;
            tempDataRight.row = blockData.row;
            tempDataRight.blockType = blockData.blockType;
            AddMergeData(tempDataRight);
        }
    }

    //머지 오브젝트 체크
    private void AddMergeData(BlockData blockData)
    {
        var blockObject = GameManager.Instance.GetBlockObject();
        int key = BlockDefine.GetGridKey(blockData.column, blockData.row);
        if(!mergeBlock.ContainsKey(key))
        {
            if (blockObject.ContainsKey(key))
            {
                Block block = blockObject[key].GetComponent<Block>();
                if (block.data.blockType == blockData.blockType)
                {
                    mergeBlock.Add(key, blockData);
                    CheckBlock(blockData);
                }
            }
        }
    }
}
