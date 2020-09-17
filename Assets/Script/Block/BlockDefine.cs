using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 를록관련 상수클래스
/// </summary>
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
    SEVEN,
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
    public static int GetGridKey(Vector2 pos)
    {
        return (int)pos.x * 1000 + (int)pos.y;
    }

    //모양 블록 생성하기 위한 그리드에 빈공간 갯수
    public const int SHAPE_BLOCK_TYPE1 = 49;
    public const int SHAPE_BLOCK_TYPE2 = 25;
    public const int SHAPE_BLOCK_TYPE3 = 10;

    //빈공간 갯수에 따른 모양 블록 확률
    public readonly static int[] SHAPE_BLOCK_PERCENT1 = { 25, 70, 100 };
    public readonly static int[] SHAPE_BLOCK_PERCENT2 = { 30, 90, 100 };
    public readonly static int[] SHAPE_BLOCK_PERCENT3 = { 50, 100, 0 };

    //블럭 확률 (합쳐서 100)
    public const int ONE_BLOCK_PERCENT = 20;
    public const int TWO_BLOCK_PERCENT = 60;
    public const int THREE_BLOCK_PERCENT = 5;

    //머지 구간 딜레이 타임
    public const float MERGE_DELAY_TIME = 0.35f;

    //모양 블럭 기본 스케일
    public const float SHAPE_BLOCK_SCALE = 0.8f;
    //모양 블럭 넥스트 스케일
    public const float NEXT_SHAPE_BLOCK_SCALE = 0.4f;
    
    public const float BLOCK_SCALE_SIZE = 1.3f;

    public const int MERGE_COUNT = 3;
}
