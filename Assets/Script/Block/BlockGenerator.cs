using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockGenerator : Singleton<BlockGenerator>
{
    //그리드위에 올라갈 단일 블록
    public GameObject gridOverBlock;

    //생성될 타겟 블록 프리팹 
    public GameObject[] prefabBlock;
    //public GameObject   prefabBlockOne;
    //public GameObject[] prefabBlockTwo;
    //public GameObject[] prefabBlockThree;

    //숫자 블록 이미지
    public Sprite[] blockSprite;

    private List<int> blockTempValue = new List<int>();

    public void Init()
    {
    }
    
    //그리드에 올라갈 단일 블록
    public GameObject CreateGridOverBlock(E_BLOCK_TYPE blockType)
    {
        GameObject cloneBlock = (GameObject)Instantiate(gridOverBlock);
        Image image = cloneBlock.GetComponent<Image>();
        if (image)
        {
            if (blockType > E_BLOCK_TYPE.NONE)
            {
                image.sprite = blockSprite[(int)blockType];
            }
            else
            {
                image.sprite = blockSprite[(int)E_BLOCK_TYPE.ONE - 1];
            }
        }
        return cloneBlock;
    }

    //타입에 맞게 타겟 블록 생성

    public GameObject CreateRandomBlock(E_BLOCK_SHAPE_TYPE type, int range)
    {
        GameObject oBlock = SettingBlockData(type, range);
        return oBlock;
    }

    //블록 데이터 셋팅
    private GameObject SettingBlockData(E_BLOCK_SHAPE_TYPE shppeType, int range)
    {
        CreateBlockValue(shppeType, range);

        GameObject prefabBlock = this.prefabBlock[(int)shppeType];
        GameObject cloneBlock = (GameObject)Instantiate(prefabBlock);
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
                    blockData.type = blockType;
                }
                Image image = childObj.GetComponentInChildren<Image>();
                if (image)
                {
                    image.sprite = blockSprite[(int)blockType];
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
