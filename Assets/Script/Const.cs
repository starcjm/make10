﻿using System.Collections;
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
    public const int GIFT_COIN = 100;

    public const int TUTORIAL_COIN = 100;

    public const int COIN_EFFECT_COUNT = 5;

    //가격
    public const int HAMMER_PRICE = 50;
    public const int BLOCK_NEXT_PRICE = 50;

    //샾 데이터
    public const float PRICE_200 = 0.99f;
    public const float PRICE_500 = 1.99f;
    public const float PRICE_1250 = 3.99f;
    public const float PRICE_3500 = 7.99f;
    public const float NO_ADS = 1.99f;

    public const int ADS_COIN = 100;
    public const int COIN_200 = 200;
    public const int COIN_500 = 500;
    public const int COIN_1250 = 1250;
    public const int COIN_3500 = 3500;


    public const string PRODUCT_NO_ADS = "noads";
    public const string PRODUCT_COIN_200 = "coin200";
    public const string PRODUCT_COIN_500 = "coin500";
    public const string PRODUCT_COIN_1250 = "coin1250";
    public const string PRODUCT_COIN_3500 = "coin3500";

    public const string ANDROID_NO_ADS_ID = "remove_ads";
    public const string ANDROID_COIN_200_ID = "200coins";
    public const string ANDROID_COIN_500_ID = "500coins";
    public const string ANDROID_COIN_1250_ID = "1250coins";
    public const string ANDROID_COIN_3500_ID = "3500coins";

    public const string IPHONE_NO_ADS_COIN_ID = "remove_ads";
    public const string IPHONE_COIN_200_ID = "200coins";
    public const string IPHONE_COIN_500_ID = "500coins";
    public const string IPHONE_COIN_1250_ID = "1250coins";
    public const string IPHONE_COIN_3500_ID = "3500coins";

    public const string GOOGLE_READERBOARD_ID = "CgkI44X8lPocEAIQAQ";

    //광고id
    //배너
    public const string ADS_ID_BANNER = "ca-app-pub-3940256099942544/6300978111";
    //전면
    public const string ADS_ID_FRONT = "ca-app-pub-3940256099942544/1033173712";
    //보상형 동영상
    public const string ADS_ID_REWARD = "ca-app-pub-3940256099942544/5224354917";
}
