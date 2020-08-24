﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public float screenAspect = 1.775F;

    //모양 블록 좌표용 오브젝트
    public GameObject shapeBlockPos;
    public GameObject nextShapeBlockPos;
    public GameObject shapeBlockLayer;
    public GameObject blockLayer;

    public MainScreen mainScreen;

    private int currentScore = 0;

    //현재 모양 블럭 
    private GameObject currentBlock;
    //다음 모양 블럭 
    private GameObject NextBlock;
    //다음 모양 블럭 사이즈
    public float nextShapeSize = 0.5f; 

    //현재 생성될 블록의 최대값
    private int range = 4;

    //key = 그리드 키,  value = 블록 오브젝트  현재 배치되 있는 블록
    private Dictionary<int, GameObject> blockObject = new Dictionary<int, GameObject>();

    //key = 그리드 키,  value = 그리드 오브젝트 
    private Dictionary<int, GameObject> gridObject = new Dictionary<int, GameObject>();

    public void AddScore(int score)
    {
        currentScore += score;
        mainScreen.SetScore(currentScore);
    }

    public void AddBlockData(int key, GameObject block)
    {
        blockObject.Add(key, block);
    }

    public void RemoveBlockData(int key)
    {
        //오브젝트 삭제후 딕셔너리에서 삭제
        if(blockObject.ContainsKey(key))
        {
            Destroy(blockObject[key]);
            blockObject.Remove(key);
        }

        //그리드가 가지고 있는 블록 데이터 초기화
        if (gridObject.ContainsKey(key))
        {
            Grid grid = gridObject[key].GetComponent<Grid>();
            AddScore((int)grid.data.blockType);
            grid.data.blockType = E_BLOCK_TYPE.NONE;
        }
    }

    public Dictionary<int, GameObject> GetBlockObject()
    {
        return blockObject;
    }

    public void AddGridData(int key, GameObject block)
    {
        gridObject.Add(key, block);
    }

    public Dictionary<int, GameObject> GetGridObject()
    {
        return gridObject;
    }

    private void Awake()
    {
        ScreenInit();
    }

    private void Start()
    {
        CreteGrid();
        InitCreateShapeBlock();
    }

    private void ScreenInit()
    {
        //Screen.SetResolution(Const.screenWidth, Const.screenHeight, true);
        screenAspect = (((float)Screen.height) / ((float)Screen.width));

        Application.targetFrameRate = Const.GAME_FRAME_RATE;
    }

    private void CreteGrid()
    {
        GridGenerator.Instance.Init();
    }

    private void InitCreateShapeBlock()
    {
        if (shapeBlockPos)
        {
            currentBlock = ShapeBlock();
            currentBlock.transform.localScale = Vector3.one;
            currentBlock.transform.position = shapeBlockPos.transform.position;
            var blockMove = currentBlock.GetComponent<BlockMove>();
            if (blockMove)
            {
                blockMove.SetTouchFlag(true);
            }
        }
        if (shapeBlockPos)
        {
            NextBlock = ShapeBlock();
            NextBlock.transform.localScale = Vector3.one * nextShapeSize;
            NextBlock.transform.position = nextShapeBlockPos.transform.position;
            var blockMove = NextBlock.GetComponent<BlockMove>();
            if (blockMove)
            {
                blockMove.SetTouchFlag(false);
            }
        }
    }

    //모양 블록 생성
    public void NextShapeBlock()
    {
        if (shapeBlockPos)
        {
            currentBlock = NextBlock;
            currentBlock.transform.localScale = Vector3.one;
            currentBlock.transform.position = shapeBlockPos.transform.position;
            var blockMove = currentBlock.GetComponent<BlockMove>();
            if(blockMove)
            {
                blockMove.SetTouchFlag(true);
            }
        }
        if(nextShapeBlockPos)
        {
            NextBlock = ShapeBlock();
            NextBlock.transform.localScale = Vector3.one * nextShapeSize;
            NextBlock.transform.position = nextShapeBlockPos.transform.position;
            var blockMove = NextBlock.GetComponent<BlockMove>();
            if (blockMove)
            {
                blockMove.SetTouchFlag(false);
            }
        }
    }

    //현재 모양 블럭 버리고 넥스트 모양 블럭 가져오고 넥스트 모양 블럭 새로 생성  
    private void CurrentShapeGiveUp()
    {
        Destroy(currentBlock);
        NextShapeBlock();
    }

    private GameObject ShapeBlock()
    {
        BlockGenerator.Instance.SettingDataClear();
        int shape = Random.Range((int)E_BLOCK_SHAPE_TYPE.ONE, (int)E_BLOCK_SHAPE_TYPE._MAX_);
        var shapeBlock = BlockGenerator.Instance.CreateRandomShapeBlock((E_BLOCK_SHAPE_TYPE)shape, range,
                                                                    shapeBlockLayer.transform);
        return shapeBlock;
    }

    //그리드위에 배치할 블록 생성
    public void CreateGridOverBlock(GridData gridData, Vector3 gridPos)
    {
        if (shapeBlockPos)
        {
            var shapeBlock = BlockGenerator.Instance.CreateGridOverBlock(gridData, blockLayer.transform);
            shapeBlock.transform.position = gridPos;
        }
    }

    //현재 화면에 생성될 블록의 최대값 설정
    public void SetBlockRangeMax(int clearNumber)
    {
        if(clearNumber > range)
        {
            if(clearNumber < (int)E_BLOCK_TYPE.STAR)
            {
                range = clearNumber;
            }
        }
    }

    //블록 놓인곳 검사
    public void MergeCheck(int column, int row)
    {
        int key = BlockDefine.GetGridKey(column, row);
        if (blockObject.ContainsKey(key))
        {
            Block block = blockObject[key].GetComponent<Block>();
            if(block)
            {
                BlockMerge blockMerge = new BlockMerge();
                blockMerge.CheckBlock(block.data);
                if(MergeBlock(blockMerge.GetMergeData()))
                {
                    SetBlockRangeMax((int)block.data.blockType + 1);
                    //블록 머지된곳에 새로운 상위값 블록 생성
                    if (block.data.blockType < E_BLOCK_TYPE.TEN) //블록 최대값이면 그냥 삭제
                    {
                        if(gridObject.ContainsKey(key))
                        {
                            //외부에서 그리드에 올릴 블럭 타입 설정 하고 생성
                            var grid = gridObject[key].GetComponent<Grid>();
                            grid.data.blockType = block.data.blockType + 1;
                            CreateGridOverBlock(grid.data, gridObject[key].transform.position);
                            MergeCheck(column, row);
                        }
                    }
                }
                blockMerge.DataClear();
            }
        }
    }

    //머지 데이터가 자신 포함 3개 이상일때 블록 삭제 후 생성
    private bool MergeBlock(List<int> mergeBlock)
    {
        if(mergeBlock.Count > 2)
        {
            for (int i = 0; i < mergeBlock.Count; ++i)
            {
                RemoveBlockData(mergeBlock[i]);
            }
            return true;
        }
        return false;
    }

    public void ChangeShapeBlock()
    {
        CurrentShapeGiveUp();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }

    public void GameClose()
    {
        Application.Quit();
    }
}
