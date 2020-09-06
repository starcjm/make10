using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 인게임 화면 스크린
/// </summary>
public class MainScreen : MonoBehaviour, IAndroidBackButton
{
    //현재 점수 
    public Text currentScore;
    //최대 점수
    public Text highScore;
    public Text Coin;
    public Text HammerCoin;
    public Text NextBlockCoin;

    public Image FiledGift;

    //그리드
    public GameObject BgGrid;

    public GameObject onHammer;
    public GameObject offHammer;
    public GameObject offGift;
    public GameObject onGift;
    //코인 연출용
    public GameObject coinIcon;
    //인게임 유아이
    public GameObject inGameUI;
    //팝업들
    public GameObject Main;
    public GameObject Gift;
    public GameObject GameOver;
    public GameObject Continue;
    public GameObject Setting;
    public GameObject Pause;
    public GameObject Shop;
    public GameObject MessageBox;

    //획득한 점수
    public int saveScore = 0;

    private void Start()
    {
        if(UserInfo.Instance.isRetry)
        {
            SetMainPopup(false);
            GameStart();
        }
    }

    private void Update()
    {
        OnTouchAndroidBackButton();
    }

    public void OnTouchAndroidBackButton()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (GameManager.Instance.GetState() == E_GAME_STATE.GAME)
                {
                    ShowPausePopup();
                }
                else if (GameManager.Instance.GetState() == E_GAME_STATE.ITEM)
                {
                    OnTouchHammer();
                }
            }
        }
    }

    public void GameStart()
    {
        SetHighScore();
        SetInGameUI(true);
        InitPriceData();
        GameManager.Instance.GameStart();
    }

    private void InitPriceData()
    {
        NextBlockCoin.text = Const.BLOCK_NEXT_PRICE.ToString();
        HammerCoin.text = Const.HAMMER_PRICE.ToString();
        SetCoin(UserInfo.Instance.Coin);
    }

    public void SetInGameUI(bool on)
    {
        inGameUI.SetActive(on);
    }

    public void SetMainPopup(bool on)
    {
        Main.SetActive(on);
    }

    public void OpenShopPopup(PopupShop.E_SHOP_TYPE type)
    {
        Shop.GetComponent<PopupShop>().SetShopType(type);
        GameManager.Instance.SetGameState(E_GAME_STATE.SHOP);
        Shop.SetActive(true);
    }

    public void ShowSettingPopup()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        Main.SetActive(false);
        Setting.SetActive(true);
    }

    public void ShowPausePopup()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        if (GameManager.Instance.GetState() == E_GAME_STATE.GAME)
        {
            GameManager.Instance.SetGameState(E_GAME_STATE.PAUSE);
            Pause.SetActive(true);
        }
    }

    public void ShowShopPopup()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        OpenShopPopup(PopupShop.E_SHOP_TYPE.IN_GAME);
    }

    public void ShowGiftPopup()
    {
        saveScore = 0;
        UpdateGiftIcon();
        var giftPopup = Gift.GetComponent<PopupGift>();
        if(giftPopup)
        {
            giftPopup.DataInit();
        }
        Gift.SetActive(true);
    }

    public void ShowGameOver()
    {
        GameManager.Instance.SetGameState(E_GAME_STATE.PAUSE);
        GameOver.SetActive(true);
        GameOver.GetComponent<PopupGameOver>().SetTimer();
    }

    public void ShowGameEnd()
    {
        if(!MessageBox.activeSelf)
        {
            MessageBox.SetActive(true);
        }
    }

    public void SetCoin(int coin)
    {
        if(Coin)
        {
            Coin.text = coin.ToString();
        }
    }

    public void SetGift()
    {
        FiledGift.fillAmount = (float)saveScore / Const.MAX_GIFT;
    }


    private void UpdateGiftIcon()
    {
        offGift.SetActive(saveScore < Const.MAX_GIFT);
        onGift.SetActive(saveScore >= Const.MAX_GIFT);
        SetGift();
    }

    public void SetScore(int score, int addScore)
    {
        saveScore += addScore;
        UpdateGiftIcon();
        if (currentScore)
        {
            currentScore.text = score.ToString();
        }
        SetHighScore();
    }

    public void SetHighScore()
    {
        if(highScore)
        {
            highScore.text = UserInfo.Instance.HighScore.ToString();
        }
    }

    public void ChangeShapeBlock()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        if (UserInfo.Instance.Coin >= Const.BLOCK_NEXT_PRICE)
        {
            GameManager.Instance.AddCoin(-Const.BLOCK_NEXT_PRICE);
            SetCoin(UserInfo.Instance.Coin);
            GameManager.Instance.ChangeShapeBlock();
        }
        else
        {
            ShowShopPopup();
        }
    }

    public void GameClose()
    {
        UserInfo.Instance.InitUserData();
        Destroy(UserInfo.Instance.gameObject);
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene((int)E_SCENE.SPLASH);
    }

    public void ShowContinuePopup(int score)
    {
        var popUp = Continue.GetComponent<PopupContinue>();
        if(popUp)
        {
            popUp.SetScore(score);
        }
        Continue.SetActive(true);
    }

    public void OnTouchHammer()
    {
        SoundManager.Instance.PlaySFX(E_SFX.BUTTON);
        if (UserInfo.Instance.Coin >= Const.HAMMER_PRICE)
        {
            GameManager.Instance.ShowBlockX();
            GameManager.Instance.SetGameItemState();
        }
        else
        {
            ShowShopPopup();
        }
    }

    public void HammerIconState()
    {
        onHammer.SetActive(GameManager.Instance.GetState() == E_GAME_STATE.ITEM);
        offHammer.SetActive(GameManager.Instance.GetState() != E_GAME_STATE.ITEM);
    }

    //동전 획득 연출
    public void CreateCoinEffect(Vector3 startPos)
    {
        for(int i = 0; i < Const.COIN_EFFECT_COUNT; ++i)
        {
            StartCoroutine(CreateCoin(startPos, coinIcon.transform.position, i));
        }
    }

    IEnumerator CreateCoin(Vector3 startPos, Vector3 endPos, int index)
    {
        yield return new WaitForSeconds(0.1f * index);
        CoinGenerator.Instance.CreateCoinEffect(startPos, endPos);
    }
}
