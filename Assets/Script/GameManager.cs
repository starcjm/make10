using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum E_GAME_STATE
{
    GAME,
    ITEM, //해머 아이템
    PAUSE,
}

public class GameManager : Singleton<GameManager>
{
    public float screenAspect = 1.775F;

    //메인 스크린 UI연동
    public MainScreen mainScreen;

    //모양 블록 좌표용 오브젝트
    public GameObject shapeBlockPos;
    public GameObject nextShapeBlockPos;
    public GameObject shapeBlockLayer;
    public GameObject blockLayer;
    public GameObject alphablockLayer;
    public GameObject effectLayer;

    //현재 모양 블럭 
    private GameObject currentBlock;
    //다음 모양 블럭 
    private GameObject NextBlock;
    //현재 생성될 블록의 최대값
    private int blockRange = 4;
    //콤보 저장용 카운트
    private int comboCount = 0;
    //현재 점수
    private int currentScore = 0;

    //해머 상태
    private bool isHammer = false;

    //key = 그리드 키,  value = 블록 오브젝트  현재 배치되 있는 블록
    private Dictionary<int, GameObject> blockObject = new Dictionary<int, GameObject>();
    //key = 그리드 키,  value = 그리드 오브젝트 
    private Dictionary<int, GameObject> gridObject = new Dictionary<int, GameObject>();

    //머지 체크 해야할 블럭
    private List<Block> mergeCheckBlockQueue = new List<Block>();

    //게임 상태(유아이 설정)
    private E_GAME_STATE gameState = E_GAME_STATE.GAME;

    private void Start()
    {
        ScreenInit();
        UserInfo.Instance.LoadUserData();
        SoundManager.Instance.Init();
        SetGameState(E_GAME_STATE.PAUSE);
    }

    public void GameStart()
    {
        mainScreen.SetScore(0, 0);
        CreteGrid();
        SetBlockblockRangeMax(Const.START_BLOCK_RANGE);
        InitCreateShapeBlock();
        SetGameState(E_GAME_STATE.GAME);
    }

    //머지 할 데이터 큐에 저장
    public void AddMergeQueue(Block block)
    {
        if(!mergeCheckBlockQueue.Contains(block))
        {
            mergeCheckBlockQueue.Add(block);
            mergeCheckBlockQueue.Sort(delegate (Block A, Block B)
            {
                Block gridA = A;
                Block gridB = B;
                if (gridA.data.blockType < gridB.data.blockType)
                {
                    return -1;
                }
                else if (gridA.data.blockType > gridB.data.blockType)
                {
                    return 1;
                }
                return 0;
            });
        }
    }

    public void RemoveMergeQueue(Block block)
    {
        if (mergeCheckBlockQueue.Contains(block))
        {
            mergeCheckBlockQueue.Remove(block);
        }
    }

    public void SetGameState(E_GAME_STATE state)
    {
        gameState = state;
    }

    public void SetGameItemState()
    {
        if(gameState == E_GAME_STATE.GAME && blockObject.Count > 0)
        {
            SetGameState(E_GAME_STATE.ITEM);
        }
        else if( gameState == E_GAME_STATE.ITEM)
        {
            SetGameState(E_GAME_STATE.GAME);
        }
    }

    public E_GAME_STATE GetState()
    {
        return gameState;
    }

    public void AddScore(int score, int key)
    {
        Vector3 pos = Vector3.zero;
        currentScore += score;
        UserInfo.Instance.HighScore = currentScore;
        mainScreen.SetScore(currentScore, score);
        if (gridObject.ContainsKey(key))
        {
            pos = gridObject[key].transform.position;
        };
        ScoreGenerator.Instance.CreateAddScore(effectLayer.transform, pos, score);
    }

    private void ComboEffect()
    {
        if(comboCount > 1)
        {
            ScoreGenerator.Instance.CreateComboEffect(effectLayer.transform,
                                                  mainScreen.BgGrid.transform.position, comboCount);
        }
        ComboInit();
    }

    public void AddCoin(int coin)
    {
        UserInfo.Instance.Coin += coin;
        mainScreen.SetCoin(UserInfo.Instance.Coin);
    }

    public void AddBlockData(int key, GameObject block)
    {
        blockObject.Add(key, block);
    }

    //블럭 위에 x이미지 
    public void ShowBlockX()
    {
        if (blockObject.Count > 0)
        {
            isHammer = !isHammer;
            foreach (GameObject blockObject in blockObject.Values)
            {
                var block = blockObject.GetComponent<Block>();
                if (block)
                {
                    block.ShowImgX(isHammer);
                }
            }
        }
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
            currentBlock.transform.localScale = Vector3.one * BlockDefine.SHAPE_BLOCK_SCALE;
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
            NextBlock.transform.localScale = Vector3.one * BlockDefine.NEXT_SHAPE_BLOCK_SCALE;
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
            currentBlock.transform.localScale = Vector3.one * BlockDefine.SHAPE_BLOCK_SCALE;
            currentBlock.transform.position = shapeBlockPos.transform.position;
            var blockMove = currentBlock.GetComponent<BlockMove>();
            if (blockMove)
            {
                blockMove.SetTouchFlag(true);
            }
        }
        if (nextShapeBlockPos)
        {
            NextBlock = ShapeBlock();
            NextBlock.transform.localScale = Vector3.one * BlockDefine.NEXT_SHAPE_BLOCK_SCALE;
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
        if(gameState == E_GAME_STATE.GAME)
        {
            Destroy(currentBlock);
            NextShapeBlock();
            //현재 블록으로 검사
            GameOverCheck(currentBlock);
        }
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
    public GameObject CreateGridOverBlock(GridData gridData, Vector3 gridPos, bool scaleAni = false)
    {
        if (shapeBlockPos)
        {
            var block = BlockGenerator.Instance.CreateGridOverBlock(gridData, blockLayer.transform);
            block.transform.position = gridPos;
            if(scaleAni)
            {
                block.transform.localScale = Vector3.one * BlockDefine.BLOCK_SCALE_SIZE;
                block.transform.DOScale(Vector3.one, 0.3f);
            }
            return block;
        }
        return null;
    }

    //현재 화면에 생성될 블록의 최대값 설정
    public void SetBlockblockRangeMax(int clearNumber)
    {
        if (clearNumber > blockRange)
        {
            if(clearNumber < (int)E_BLOCK_TYPE._MAX_)
            {
                blockRange = clearNumber;
            }
        }
    }

    public void ComboInit()
    {
        comboCount = 0;
    }

    //머지 체크 시작
    public void MergeCheckStart()
    {
        if(mergeCheckBlockQueue.Count > 0)
        {
            var block = mergeCheckBlockQueue[0];
            RemoveMergeQueue(block);
            MergeDelayCheck(block);
        }
        else
        {
            ComboEffect();
            NextShapeBlock();
            GameOverCheck(currentBlock);
        }
    }

    //여러 블럭이 동시에 합쳐질 경우 연출을 위한 딜레이
    public void MergeDelayCheck(Block block)
    {
        BlockCalculate blockCalculate = new BlockCalculate();
        blockCalculate.CheckBlock(block.data, true);
        if (MergeCheck(blockCalculate.GetMergeData()))
        {
            StartCoroutine(MergeBlockDelay(block,
                BlockDefine.MERGE_DELAY_TIME));
        }
        else
        {
            //머지할 블록이 없다면 다음 블록
            MergeCheckStart();
        }
    }

    public void MergeCheck(Block block)
    {
        BlockCalculate blockCalculate = new BlockCalculate();
        blockCalculate.SetStartBlock(block);
        blockCalculate.CheckBlock(block.data, false);
        if (blockCalculate.MergeBlockLastCheck())
        {
            //머지가 된다면 콤보 추가
            ++comboCount;
        }
    }

    IEnumerator MergeBlockDelay(Block block, float time)
    {
        yield return new WaitForSeconds(time);
        MergeCheck(block);
    }

    //머지된 블럭들 연출후 실제 데이터 삭제
    public void MergeCompleteRemoveblock(List<Block> mergeData, Block block, BlockCalculate calc)
    {
        MergeBlockRemove(mergeData);
        SetBlockblockRangeMax((int)block.data.blockType + 1);
        //블록 머지된곳에 새로운 상위값 블록 생성
        if (block.data.blockType < E_BLOCK_TYPE.STAR)
        {
            if (gridObject.ContainsKey(block.data.key))
            {
                //그리드에 올릴 블럭 타입 설정 하고 생성
                var grid = gridObject[block.data.key].GetComponent<Grid>();
                grid.data.blockType = block.data.blockType + 1;
                var gridOverBlock = CreateGridOverBlock(grid.data, gridObject[block.data.key].transform.position, true);


                //새로운 블록 머지 큐에 넣어주고 머지 스타트
                AddMergeQueue(gridOverBlock.GetComponent<Block>());
                MergeCheckStart();
            }
        }
        else
        {
            //별모양 터트렸을때 효과
            NextShapeBlock();
            calc.StarBlockEffect();
            var starBlocks = calc.GetStarBlockEffect();
            ChangeStarblock(starBlocks);
            StartCoroutine(StarBlockRemoveDelay(starBlocks));
        }
    }

    //별모양 터트릴때 주변 블록들 별모양으로 변경
    private void ChangeStarblock(List<Block> blocks)
    {
        for(int i = 0; i < blocks.Count; ++i)
        {
            var block = blocks[i];
            if(block)
            {
                block.ChangeImage(E_BLOCK_TYPE.STAR);
            }
        }
    }

    //블록 관련 타이밍 조절 함수들
    IEnumerator StarBlockRemoveDelay(List<Block> mergeBlock)
    {
        yield return new WaitForSeconds(BlockDefine.MERGE_DELAY_TIME * 2.0f);
        MergeBlockRemove(mergeBlock);
    }

    //머지 데이터 체크용
    private bool MergeCheck(List<Block> mergeBlock)
    {
        if (mergeBlock.Count >= BlockDefine.MERGE_COUNT)
        {
            return true;
        }
        return false;
    }

    //머지 데이터가 자신 포함 3개 이상일때 블록 삭제 후 생성
    private void MergeBlockRemove(List<Block> mergeBlock)
    {
        if(mergeBlock.Count > 0)
        {
            int totalScore = 0;
            SoundManager.Instance.PlaySFX(E_SFX.BLOCK_MERGE);
            for (int i = 0; i < mergeBlock.Count; ++i)
            {
                var blockTyp = mergeBlock[i].data.blockType;
                totalScore += (int)blockTyp;
                RemoveMergeQueue(mergeBlock[i]);
                RemoveBlockData(mergeBlock[i].data.key);
            }
            AddScore(totalScore, mergeBlock[0].data.key);
        }
    }

    //이어하기 했을때 가운데 3 * 3 블럭 삭제
    public void ContinueDestryBlock()
    {
        BlockCalculate blockCalculate = new BlockCalculate();
        List<int> removeBlockData = blockCalculate.ContinueDestroyBlock();
        for(int i = 0; i < removeBlockData.Count; ++i)
        {
            RemoveBlockData(removeBlockData[i]);
        }
    }

    public void ChangeShapeBlock()
    {
        CurrentShapeGiveUp();
    }

    public void ShowContinuePopup()
    {
        mainScreen.ShowContinuePopup(currentScore);
    }

    //게임 종료 체크
    private void GameOverCheck(GameObject shapeBlock)
    {
        var blockMove = shapeBlock.GetComponent<BlockMove>();
        BlockCalculate blockCalculate = new BlockCalculate();
        if(blockCalculate.GameOverCheck(blockMove.shapeType))
        {
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        mainScreen.ShowGameOver();
    }

    public void Retry()
    {
        UserInfo.Instance.isRetry = true;
        SceneManager.LoadScene(1);
    }

    public void GameClose()
    {
        Application.Quit();
    }
}
