using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//블록 넘버 타입
public enum E_BLOCK_TYPE
{
    NONE = 0,
    ONE,
    TWO,
    TRHEE,
    FOUR,
    FIVE,
    SIX,
    EIGHT,
    NINE,
    STAR,
    _MAX_,
}

//타겟 블록 타입
public enum E_BLOCK_SHAPE_TYPE
{
    ONE,
    TWO,
    THREE,
    _MAX_,
}

public static class BlockDefine
{
    //블록 좌표로 키값 만듬
    public static int GetGridKey(int column, int row)
    {
        return column * 1000 + row;
    }

    public const int START_BLOCK_RANGE = 4;

    //블럭 확률 (합쳐서 100)
    public const int ONE_BLOCK_PERCENT = 40;
    public const int TWO_BLOCK_PERCENT = 40;
    public const int THREE_BLOCK_PERCENT = 20;

    public const float MERGE_DELAY_TIME = 0.2f;
}
