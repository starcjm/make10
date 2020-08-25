using System.Collections;
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
    public GameObject alphablockLayer;

    public MainScreen mainScreen;

    private int currentScore = 0;

    //현재 모양 블럭 
    private GameObject currentBlock;
    //다음 모양 블럭 
    private GameObject NextBlock;
    //다음 모양 블럭 사이즈
    public float nextShapeSize = 0.5f; 
    //현재 생성될 블록의 최대값
    private int blockRange = 4;

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
    }

    private void Start()
    {
        ScreenInit();
        CreteGrid();
        SetBlockblockRangeMax(BlockDefine.START_BLOCK_RANGE);
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

    //확률에 따라 블록 생성
    private E_BLOCK_SHAPE_TYPE GetShapeBlockType()
    {
        E_BLOCK_SHAPE_TYPE type = E_BLOCK_SHAPE_TYPE.ONE;
        int range = Random.Range(0, 100);
        if(range < BlockDefine.ONE_BLOCK_PERCENT)
        {
            type = E_BLOCK_SHAPE_TYPE.ONE;
        }
        else if(range < BlockDefine.ONE_BLOCK_PERCENT + BlockDefine.TWO_BLOCK_PERCENT)
        {
            type = E_BLOCK_SHAPE_TYPE.TWO;
        }
        else if (range < BlockDefine.ONE_BLOCK_PERCENT + BlockDefine.TWO_BLOCK_PERCENT
                       + BlockDefine.THREE_BLOCK_PERCENT)
        {
            type = E_BLOCK_SHAPE_TYPE.THREE;
        }
        return type;
    }

    private GameObject ShapeBlock()
    {
        BlockGenerator.Instance.SettingDataClear();
        var shapeBlock = BlockGenerator.Instance.CreateRandomShapeBlock(GetShapeBlockType(), blockRange,
                                                                    shapeBlockLayer.transform);
        return shapeBlock;
    }

    public GameObject CreateAlphaShapeBlock(GameObject block, Vector3 pos)
    {
        return BlockGenerator.Instance.CreateAlphaShapeBlock(block, alphablockLayer.transform, pos);
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
    public void SetBlockblockRangeMax(int clearNumber)
    {
        if (clearNumber > blockRange)
        {
            if(clearNumber < (int)E_BLOCK_TYPE._MAX_ + 1)
            {
                blockRange = clearNumber;
            }
        }
    }

    //여러 블럭이 동시에 합쳐질 경우 연출을 위한 딜레이
    public void MergeDelayCheck(List<GameObject> tempGrids)
    {
        int mergeCount = 0;
        for (int i = 0; i < tempGrids.Count; ++i)
        {
            Grid grid = tempGrids[i].GetComponent<Grid>();
            int key = BlockDefine.GetGridKey(grid.data.column, grid.data.row);
            if (blockObject.ContainsKey(key))
            {
                Block block = blockObject[key].GetComponent<Block>();
                if (block)
                {
                    BlockMerge blockMerge = new BlockMerge();
                    blockMerge.CheckBlock(block.data);
                    if (MergeCheckBlock(blockMerge.GetMergeData()))
                    {
                        ++mergeCount;
                        StartCoroutine(MergeBlockDelay(grid.data.column, grid.data.row,
                            BlockDefine.MERGE_DELAY_TIME * (mergeCount)));
                    }
                }
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
                    SetBlockblockRangeMax((int)block.data.blockType + 1);
                    //블록 머지된곳에 새로운 상위값 블록 생성
                    if (block.data.blockType < E_BLOCK_TYPE._MAX_) //블록 최대값이면 그냥 삭제
                    {
                        if(gridObject.ContainsKey(key))
                        {
                            //외부에서 그리드에 올릴 블럭 타입 설정 하고 생성
                            var grid = gridObject[key].GetComponent<Grid>();
                            grid.data.blockType = block.data.blockType + 1;
                            CreateGridOverBlock(grid.data, gridObject[key].transform.position);
                            StartCoroutine(MergeBlockDelay(column, row, BlockDefine.MERGE_DELAY_TIME));
                        }
                    }
                    else
                    {
                        //별모양 터트렸을때 효과
                        Debug.Log("별모양 터트렸음");
                    }
                }
                blockMerge.DataClear();
            }
        }
    }

    IEnumerator MergeBlockDelay(int column, int row, float time)
    {
        yield return new WaitForSeconds(time);
        MergeCheck(column, row);
    }

    //머지 데이터 체크용
    private bool MergeCheckBlock(List<int> mergeBlock)
    {
        if (mergeBlock.Count > 2)
        {
            return true;
        }
        return false;
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
