using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 블럭 생성 클래스
/// </summary>
public class BlockGenerator : Singleton<BlockGenerator>
{
    //그리드위에 올라갈 단일 블록
    public GameObject gridOverBlock;
    //생성될 타겟 블록 프리팹 
    public GameObject[] prefabBlock;
    //현재 화면에 올라와 있는 모양 블록
    private GameObject shapeObject;

    //숫자 블록 이미지
    public Sprite[] blockSprite;

    private List<int> blockTempValue = new List<int>();
    
    //그리드에 올라갈 단일 블록
    public GameObject CreateGridOverBlock(GridData gridData, Transform parent)
    {
        GameObject cloneBlock = (GameObject)Instantiate(gridOverBlock);
        cloneBlock.name = string.Format("BLOCK{0}-{1}", gridData.column, gridData.row);
        cloneBlock.transform.SetParent(parent.transform);
        cloneBlock.transform.localScale = Vector3.one;
        Block block = cloneBlock.GetComponent<Block>();
        if (block)
        {
            block.data.blockType = gridData.blockType;
            block.data.column = gridData.column;
            block.data.row = gridData.row;
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
        int key = BlockDefine.GetGridKey(block.data.column, block.data.row);
        GameManager.Instance.AddBlockData(key, cloneBlock);
        return cloneBlock;
    }

    //반투명 블럭 생성
    public GameObject CreateAlphaShapeBlock(GameObject block, Transform parent, Vector3 pos)
    {
        GameObject cloneBlock = (GameObject)Instantiate(block);
        //알파 블록은 충돌 해제
        //cloneBlock.layer = 0;
        cloneBlock.GetComponent<BoxCollider2D>().enabled = false;
        cloneBlock.transform.SetParent(parent);
        cloneBlock.transform.position = pos;
        cloneBlock.transform.localScale = Vector3.one;
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
                Block block = childObj.GetComponent<Block>();
                if (block)
                {
                    block.data.blockType = blockType;
                }
                Image image = childObj.GetComponentInChildren<Image>();
                if (image)
                {
                    image.sprite = blockSprite[(int)blockType - 1];
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
    private void CreateBlockValue(E_BLOCK_SHAPE_TYPE shppeType, int range)
    {
        int value = 0;
        switch (shppeType)
        {
            case E_BLOCK_SHAPE_TYPE.ONE:
                value = Random.Range((int)E_BLOCK_TYPE.ONE, range + 1);
                blockTempValue.Add(value);
                break;
            case E_BLOCK_SHAPE_TYPE.TWO:
                for(int i = 0; i < 2; ++i)
                {
                    value = Random.Range((int)E_BLOCK_TYPE.ONE, range + 1);
                    blockTempValue.Add(value);
                }
                break;
            case E_BLOCK_SHAPE_TYPE.THREE:
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
