using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임관련 상수 클래스
/// </summary>
/// 
public class Const
{
    public const int screenWidth = 720;
    public const int screenHeight = 1280;
    public const int GAME_FRAME_RATE = 60;

    public const int MERGE_COUNT = 3;

    public const int GRID_COLUMN_COUNT = 7;
    public const int GRID_ROW_COUNT = 7;

    public const int START_BLOCK_RANGE = 4;

    //기프트 점수 
    public const int MAX_GIFT = 500;

    //기프트 획득 인
    public const int GIFT_COIN = 10;

    public const int TUTORIAL_COIN = 100;

    public const int COIN_EFFECT_COUNT = 5;

    //가격
    public const int HAMMER_PRICE = 30;
    public const int BLOCK_NEXT_PRICE = 25;

    //샾 데이터
    public const float PRICE_200 = 0.99f;
    public const float PRICE_500 = 1.99f;
    public const float PRICE_1250 = 3.99f;
    public const float PRICE_3500 = 7.99f;
    public const float NO_ADS = 1.99f;

    public const int ADS_COIN = 25;
    public const int COIN_200 = 200;
    public const int COIN_500 = 500;
    public const int COIN_1250 = 1250;
    public const int COIN_3500 = 3500;

    //광고id
    //배너
    public const string ADS_ID_BANNER = "ca-app-pub-3940256099942544/6300978111";
    //전면
    public const string ADS_ID_FRONT = "ca-app-pub-3940256099942544/1033173712";
    //보상형 동영상
    public const string ADS_ID_REWARD = "ca-app-pub-3940256099942544/5224354917";
}
