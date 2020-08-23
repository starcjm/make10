﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockGenerator : Singleton<BlockGenerator>
{
    //그리드위에 올라갈 단일 블록
    public GameObject gridOverBlock;

    //생성될 타겟 블록 프리팹 
    public GameObject[] prefabBlock;

    //숫자 블록 이미지
    public Sprite[] blockSprite;

    private List<int> blockTempValue = new List<int>();

    public void Init()
    {
    }
    
    //그리드에 올라갈 단일 블록
    public GameObject CreateGridOverBlock(GridData gridData, Transform parent)
    {
        GameObject cloneBlock = (GameObject)Instantiate(gridOverBlock);
        cloneBlock.transform.SetParent(parent.transform);
        cloneBlock.transform.localScale = Vector3.one;
        BlockData blockData = cloneBlock.GetComponent<BlockData>();
        if (blockData)
        {
            blockData.blockType = gridData.blockType;
            blockData.column = gridData.column;
            blockData.row = gridData.row;
        }
        Image image = cloneBlock.GetComponent<Image>();
        if (image)
        {
            int value = 0;
            if (blockData.blockType > E_BLOCK_TYPE.NONE)
            {
                value = (int)gridData.blockType - 1;
            }
            image.sprite = blockSprite[value];
        }
        int key = BlockDefine.GetGridKey(blockData.column, blockData.row);
        GameManager.Instance.AddBlockData(key, cloneBlock);
        return cloneBlock;
    }

    //타입에 맞게 타겟 블록 생성

    public GameObject CreateRandomBlock(E_BLOCK_SHAPE_TYPE type, int range, Transform parent)
    {
        GameObject oBlock = SettingBlockData(type, range, parent);
        return oBlock;
    }

    //모양 블록 데이터 셋팅
    private GameObject SettingBlockData(E_BLOCK_SHAPE_TYPE shppeType, int range, Transform parent)
    {
        CreateBlockValue(shppeType, range);

        GameObject prefabBlock = this.prefabBlock[(int)shppeType];
        GameObject cloneBlock = (GameObject)Instantiate(prefabBlock);
        cloneBlock.transform.SetParent(parent);
        cloneBlock.transform.localScale = Vector3.one;
        int childCnt = cloneBlock.transform.childCount;
        for (int i = 0; i < childCnt; ++i)
        {
            if(i < blockTempValue.Count)
            {
                E_BLOCK_TYPE blockType = (E_BLOCK_TYPE)blockTempValue[i];
                var childObj = cloneBlock.transform.GetChild(i);

                //생성될 블록 값
                BlockData blockData = childObj.GetComponent<BlockData>();
                if (blockData)
                {
                    blockData.blockType = blockType;
                }
                Image image = childObj.GetComponentInChildren<Image>();
                if (image)
                {
                    image.sprite = blockSprite[(int)blockType - 1];
                }
            }
        }
        return cloneBlock;
    }

    //블록 안에 들어갈 데이터 생성
    private void CreateBlockValue(E_BLOCK_SHAPE_TYPE shppeType, int range)
    {
        int value = 0;
        switch (shppeType)
        {
            case E_BLOCK_SHAPE_TYPE.ONE:
                value = Random.Range((int)E_BLOCK_TYPE.ONE, range + 1);
                blockTempValue.Add(value);
                break;
            case E_BLOCK_SHAPE_TYPE.TWO_A:
            case E_BLOCK_SHAPE_TYPE.TWO_B:
            case E_BLOCK_SHAPE_TYPE.TWO_C:
            case E_BLOCK_SHAPE_TYPE.TWO_D:
                for(int i = 0; i < 2; ++i)
                {
                    value = Random.Range((int)E_BLOCK_TYPE.ONE, range + 1);
                    blockTempValue.Add(value);
                }
                break;
            case E_BLOCK_SHAPE_TYPE.THREE_A:
            case E_BLOCK_SHAPE_TYPE.THREE_B:
            case E_BLOCK_SHAPE_TYPE.THREE_C:
            case E_BLOCK_SHAPE_TYPE.THREE_D:
                for (int i = 0; i < 3; ++i)
                {
                    value = Random.Range((int)E_BLOCK_TYPE.ONE, range + 1);
                    blockTempValue.Add(value);
                }
                break;
        }
    }

    //블록 안에 들어갈 데이터 초기화
    public void SettingDataClear()
    {
        blockTempValue.Clear();
    }
}
