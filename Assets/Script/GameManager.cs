using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float screenAspect = 1.775F;

    //타겟 블록 좌표용 오브젝트
    public GameObject targetBlockPos;
    public GameObject targetBlockLayer;
    public GameObject blockLayer;

    //현재 생성될 블록의 최대값
    private int range = 4;

    //key = 그리드 키,  value = 블록 오브젝트  현재 배치되 있는 블록
    private Dictionary<int, GameObject> blockObject = new Dictionary<int, GameObject>();

    //key = 그리드 키,  value = 그리드 오브젝트 
    private Dictionary<int, GameObject> gridObject = new Dictionary<int, GameObject>();

    public void AddBlockData(int key, GameObject block)
    {
        blockObject.Add(key, block);
    }

    public void RemoveBlockData(int key)
    {
        //오브젝트 삭제후 딕셔너리에서 삭제
        Destroy(blockObject[key]);
        blockObject.Remove(key);

        //그리드가 가지고 있는 블록 데이터 초기화
        if (gridObject.ContainsKey(key))
        {
            Grid grid = gridObject[key].GetComponent<Grid>();
            grid.data.column = 0;
            grid.data.row = 0;
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
        CreateTargetBlock();
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

    //타겟블록 생성
    public void CreateTargetBlock()
    {
        if (targetBlockPos)
        {
            BlockGenerator.Instance.SettingDataClear();
            int shape = Random.Range((int)E_BLOCK_SHAPE_TYPE.ONE, (int)E_BLOCK_SHAPE_TYPE._MAX_);
            var targetBlock = BlockGenerator.Instance.CreateRandomBlock((E_BLOCK_SHAPE_TYPE)shape, range,
                                                                        targetBlockLayer.transform);
            targetBlock.transform.position = targetBlockPos.transform.position;
        }
    }

    //그리드위에 배치할 블록 생성
    public void CreateGridOverBlock(GridData gridData, Vector3 gridPos)
    {
        if (targetBlockPos)
        {
            var targetBlock = BlockGenerator.Instance.CreateGridOverBlock(gridData, blockLayer.transform);
            targetBlock.transform.position = gridPos;
        }
    }

    //현재 화면에 생성될 블록의 최대값 설정
    public void SetBlockRangeMax(int clearNumber)
    {
        if(clearNumber > range)
        {
            range = clearNumber;
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
            }
        }
    }
}
