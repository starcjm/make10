using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    enum E_CHECK_TYPE
    {
        COLUMN,
        ROW,
    }

    public float screenAspect = 1.775F;

    //타겟 블록 좌표용 오브젝트
    public GameObject targetBlockPos;
    public GameObject targetBlockLayer;
    public GameObject blockLayer;

    //현재 생성될 블록의 최대값
    private int range = 4;

    //key = 그리드 키,  value = 블록 오브젝트
    private Dictionary<int, GameObject> blockObject = new Dictionary<int, GameObject>();

    //key = 그리드 키,  value = 그리드 오브젝트
    private Dictionary<int, GameObject> gridObject = new Dictionary<int, GameObject>();

    //가로 세로 삭제할 블록들
    private List<GameObject> destroyBlock = new List<GameObject>();

    public void AddBlockData(int key, GameObject block)
    {
        blockObject.Add(key, block);
    }

    public void AddGridData(int key, GameObject block)
    {
        gridObject.Add(key, block);
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
            BlockData blockData = blockObject[key].GetComponent<BlockData>();
            if(blockData)
            {
            }
        }
    }

    private void CheckBlock(E_CHECK_TYPE checkType, E_BLOCK_TYPE blockType, int value)
    {
    }

    //private void CheckBlock(E_CHECK_TYPE checkType, E_BLOCK_TYPE blockType, int value)
    //{
    //    //블록 머지용 매치용 임시 키값들
    //    List<int> tempBlockKey = new List<int>();

    //    for(int i = 0; i < Const.GRID_COLUMN_COUNT; ++i)
    //    {
    //        int key = 0;
    //        if(checkType == E_CHECK_TYPE.COLUMN)
    //        {
    //            key = BlockDefine.GetGridKey(value, i + 1);
    //        }
    //        else
    //        {
    //            key = BlockDefine.GetGridKey(i+1, value);
    //        }

    //        if(blockObject.ContainsKey(key))
    //        {
    //            BlockData blockData = blockObject[key].GetComponent<BlockData>();
    //            if(blockData.blockType == blockType)
    //            {
    //                tempBlockKey.Add(key);
    //            }
    //        }
    //        else
    //        {
    //            //연속된 블록 갯수가 3개이상일때는 블록 머지
    //            if(tempBlockKey.Count >= Const.MERGE_COUNT)
    //            {
    //                break;
    //            }
    //            else
    //            {
    //                tempBlockKey.Clear();
    //            }
    //        }
    //    }
    //}
}
