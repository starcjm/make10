using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

/// <summary>
/// 블럭 생성 클래스
/// </summary>
public class BlockGenerator : Singleton<BlockGenerator>
{
    public class BlockTempData
    {
        public E_BLOCK_TYPE type = E_BLOCK_TYPE.ONE;
        public bool firstMark = false;
        public BlockTempData(E_BLOCK_TYPE Type, bool FirstMark)
        {
            type = Type;
            firstMark = FirstMark;
        }
    }
    //그리드위에 올라갈 단일 블록
    public GameObject gridOverBlock;
    //생성될 타겟 블록 프리팹 
    public GameObject[] prefabBlock;
    //현재 화면에 올라와 있는 모양 블록
    private GameObject shapeObject;

    //머지 됫을때 효과
    public GameObject mergeEffect;

    //숫자 블록 이미지
    public Sprite[] blockSprite;

    private List<BlockTempData> blockTempValue = new List<BlockTempData>();
    
    //그리드에 올라갈 단일 블록
    public GameObject CreateGridOverBlock(GridData gridData, Transform parent)
    {
        GameObject cloneBlock = Instantiate(gridOverBlock);
        cloneBlock.name = string.Format("BLOCK{0}-{1}", gridData.column, gridData.row);
        cloneBlock.transform.SetParent(parent.transform);
        cloneBlock.transform.localScale = Vector3.one;
        Block block = cloneBlock.GetComponent<Block>();
        if (block)
        {
            block.data.blockType = gridData.blockType;
            block.data.column = gridData.column;
            block.data.row = gridData.row;
            block.data.key = BlockDefine.GetGridKey(gridData.column, gridData.row);
            block.ShowImgX(false);
        }
        Image image = cloneBlock.GetComponent<Image>();
        if (image)
        {
            int value = 0;
            if (block.data.blockType > E_BLOCK_TYPE.NONE)
            {
                value = (int)gridData.blockType - 1;
            }
            image.sprite = blockSprite[value];
        }
        GameManager.Instance.AddBlockData(block.data.key, cloneBlock);
        return cloneBlock;
    }

    //반투명 블럭 생성
    public GameObject CreateAlphaShapeBlock(GameObject block, Transform parent, Vector3 pos)
    {
        GameObject cloneBlock = Instantiate(block);
        //알파 블록은 충돌 해제
        //cloneBlock.layer = 0;
        cloneBlock.GetComponent<BoxCollider2D>().enabled = false;
        cloneBlock.transform.SetParent(parent);
        cloneBlock.transform.position = pos;
        cloneBlock.transform.localScale = Vector3.one;
        cloneBlock.transform.rotation = quaternion.Euler(0.0f, 0.0f, 0.0f);
        var image = cloneBlock.transform.GetComponent<Image>();
        if(image)
        {
            image.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
        }
        return cloneBlock;
    }

    //타입에 맞게 모양 블록 생성
    public GameObject CreateRandomShapeBlock(E_BLOCK_SHAPE_TYPE type, int range, Transform parent)
    {
        GameObject block = ShapeBlockData(type, range, parent);
        return block;
    }

    //모양 블록 데이터 셋팅
    private GameObject ShapeBlockData(E_BLOCK_SHAPE_TYPE shppeType, int range, Transform parent)
    {
        CreateBlockValue(shppeType, range);

        GameObject prefabBlock = this.prefabBlock[(int)shppeType];
        GameObject cloneBlock = Instantiate(prefabBlock);
        cloneBlock.transform.SetParent(parent);
        cloneBlock.transform.localScale = Vector3.one;
        int childCnt = cloneBlock.transform.childCount;
        for (int i = 0; i < childCnt; ++i)
        {
            if(i < blockTempValue.Count)
            {
                E_BLOCK_TYPE blockType = blockTempValue[i].type;
                var childObj = cloneBlock.transform.GetChild(i);

                //생성될 블록 값
                Block block = childObj.GetComponent<Block>();
                if (block)
                {
                    block.data.blockType = blockType;
                    block.mainImg.sprite = blockSprite[(int)blockType - 1];
                    block.firstMark.SetActive(blockTempValue[i].firstMark);
                } 
            }
        }
        shapeObject = cloneBlock;
        return cloneBlock;
    }

    public GameObject GetCurrentShapeBlock()
    {
        return shapeObject;
    }

    public void DestroyCurrentShapeBlock()
    {
        Destroy(shapeObject);
    }

    //블록 안에 들어갈 데이터 생성
    private void CreateBlockValue(E_BLOCK_SHAPE_TYPE shapeType, int range)
    {
        int value = 0;
        switch (shapeType)
        {
            case E_BLOCK_SHAPE_TYPE.ONE:
                {
                    value = GetCreateBlockValue((int)E_BLOCK_TYPE.ONE, range + 1);
                    blockTempValue.Add(new BlockTempData((E_BLOCK_TYPE)value, false));
                }
                break;
            case E_BLOCK_SHAPE_TYPE.TWO:
                {
                    for (int i = 0; i < 2; ++i)
                    {
                        value = GetCreateBlockValue((int)E_BLOCK_TYPE.ONE, range + 1);
                        blockTempValue.Add(new BlockTempData((E_BLOCK_TYPE)value, false));
                    }
                }
                break;
            case E_BLOCK_SHAPE_TYPE.THREE:
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        value = GetCreateBlockValue((int)E_BLOCK_TYPE.ONE, range + 1);
                        blockTempValue.Add(new BlockTempData((E_BLOCK_TYPE)value, false));
                    }
                }
                break;
        }
        FirstMarkCheck(shapeType);
    }

    private void FirstMarkCheck(E_BLOCK_SHAPE_TYPE shapeType)
    {
        if(shapeType == E_BLOCK_SHAPE_TYPE.TWO)
        {
            if(blockTempValue.Count >= 2)
            {
                if (blockTempValue[0].type == blockTempValue[1].type)
                {
                    blockTempValue[0].firstMark = true;
                }
            }
        }
        else if(shapeType == E_BLOCK_SHAPE_TYPE.THREE)
        {
            if (blockTempValue.Count >= 3)
            {
                if (blockTempValue[0].type == blockTempValue[1].type
                 || blockTempValue[0].type == blockTempValue[2].type)
                {
                    blockTempValue[0].firstMark = true;
                }
                else if (blockTempValue[1].type == blockTempValue[2].type)
                {
                    blockTempValue[1].firstMark = true;
                }
            }
        }
    }

    public int GetCreateBlockValue(int min, int max)
    {
        int value = GetRandomValue(min, max);
        if (value == (int)E_BLOCK_TYPE.STAR)
        {
            value = GetRandomValue((int)E_BLOCK_TYPE.FIVE, (int)E_BLOCK_TYPE._MAX_);
        }
        return value;
    }

    public int GetRandomValue(int min, int max)
    {
        int value = UnityEngine.Random.Range(min, max);
        return value;
    }

    //블록 안에 들어갈 데이터 초기화
    public void SettingDataClear()
    {
        blockTempValue.Clear();
    }

    public void CreateMergeEffec(Transform parent, Vector3 pos)
    {
        GameObject cloneEffect = Instantiate(mergeEffect);
        cloneEffect.transform.SetParent(parent);
        cloneEffect.transform.position = pos;
        cloneEffect.transform.localScale = Vector3.one;
        Destroy(cloneEffect, 1.0f);
    }
}
