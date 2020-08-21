﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //타겟 블록 좌표
    public GameObject targetBlockPos;
    public GameObject targetBlockLayer;
    public GameObject blockLayer;

    //현재 생성될 블록의 최대값
    private int range = 4;

    private void Awake()
    {
        CreteGrid();
        CreateTargetBlock();
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
            var targetBlock = BlockGenerator.Instance.CreateRandomBlock((E_BLOCK_SHAPE_TYPE)shape, range);
            targetBlock.transform.SetParent(targetBlockLayer.transform);
            targetBlock.transform.position = targetBlockPos.transform.position;
        }
    }

    //그리드위에 배치할 블록 생성
    public void CreateGridOverBlock(Vector3 gridPos, E_BLOCK_TYPE type)
    {
        if (targetBlockPos)
        {
            var targetBlock = BlockGenerator.Instance.CreateGridOverBlock(type);
            targetBlock.transform.SetParent(blockLayer.transform);
            targetBlock.transform.position = gridPos;
        }
    }

    //현재 화면에 생성될 블록의 최대값 설정
    public void SetBlockRangeMax(int maxRange)
    {
        range = maxRange;
    }
}
