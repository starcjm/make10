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
    TEN,
    STAR,
    _MAX_,
}

//타겟 블록 타입
public enum E_BLOCK_SHAPE_TYPE
{
    ONE,
    TWO_A,
    TWO_B,
    TWO_C,
    TWO_D,
    THREE_A,
    THREE_B,
    THREE_C,
    THREE_D,
    _MAX_,
}

public static class BlockDefine
{
    //블록 좌표로 키값 만듬
    public static int GetGridKey(int column, int row)
    {
        return column * 1000 + row;
    }
}
