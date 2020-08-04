using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class View : MonoBehaviour
{

    private RectTransform logoName;
    private RectTransform menuUI;
    private RectTransform gameUI;
    private RectTransform gameOverUI;
    private RectTransform settingUI;
    private RectTransform audioMuteUI;
    private RectTransform rankUI;
    private Transform map;
    private GameObject restartButton;
    private Text scoreText;
    private Text highScoreText;
    private Text rankScoreText;
    private Text rankHighScoreText;
    private Text rankGameNumberText;

    // Start is called before the first frame update
    void Awake()
    {
        logoName = transform.Find("Canvas/Logoname") as RectTransform;
        menuUI = transform.Find("Canvas/MenuUI") as RectTransform;
        gameUI = transform.Find("Canvas/GameUI") as RectTransform;
        gameOverUI = transform.Find("Canvas/GameOverUI") as RectTransform;
        settingUI = transform.Find("Canvas/SettingUI") as RectTransform;
        audioMuteUI = transform.Find("Canvas/SettingUI/AudioButton/Mute") as RectTransform;
        rankUI = transform.Find("Canvas/RankUI") as RectTransform;
        map = transform.Find("Map");
        restartButton = transform.Find("Canvas/MenuUI/RestartButton").gameObject;
        scoreText = transform.Find("Canvas/GameUI/ScoreLabel/Text").GetComponent<Text>();
        highScoreText = transform.Find("Canvas/GameUI/HighScoreLabel/Text").GetComponent<Text>();
        rankScoreText = transform.Find("Canvas/RankUI/ScoreLabel/Text").GetComponent<Text>();
        rankHighScoreText = transform.Find("Canvas/RankUI/HighLabel/Text").GetComponent<Text>();
        rankGameNumberText = transform.Find("Canvas/RankUI/GameNumbersLabel/Text").GetComponent<Text>();

    }

    /// <summary>
    /// 显示菜单
    /// </summary>
    public void ShowMenu()
    {
        logoName.gameObject.SetActive(true);
        logoName.DOAnchorPosY(-60.9f, 0.5f);
        menuUI.gameObject.SetActive(true);
        menuUI.DOAnchorPosY(44.5f, 0.5f);
    }

    /// <summary>
    /// 隐藏菜单
    /// </summary>
    public void HideMenu()
    {
        logoName.DOAnchorPosY(69.5f, 0.5f).OnComplete(delegate { logoName.gameObject.SetActive(false); });
        menuUI.DOAnchorPosY(-44.45f, 0.5f).OnComplete(delegate { menuUI.gameObject.SetActive(false); });
    }

    /// <summary>
    /// 显示游戏界面
    /// </summary>
    public void ShowGame()
    {
        gameUI.gameObject.SetActive(true);
        gameUI.DOAnchorPosY(-64.8f, 0.5f);
        map.DOLocalMoveY(-165f, 0.5f);
    }

    /// <summary>
    /// 隐藏游戏界面
    /// </summary>
    public void HideGame()
    {
        gameUI.DOAnchorPosY(64.8f, 0.5f).OnComplete(delegate { gameUI.gameObject.SetActive(false); });
        map.DOLocalMoveY(-161.56f, 0.5f);
    }

    /// <summary>
    /// 显示出重新开始游戏的按钮
    /// </summary>
    public void ShowRestartButton()
    {
        restartButton.SetActive(true);
    }

    /// <summary>
    /// 显示游戏结束时的界面框
    /// </summary>
    /// <param name="score"></param>
    public void ShowGameOverUI(int score)
    {
        gameOverUI.gameObject.SetActive(true);
        Text finalScore = gameOverUI.GetChild(2).GetComponent<Text>();
        finalScore.text = score.ToString();
    }

    /// <summary>
    /// 隐藏游戏结束时的界面框
    /// </summary>
    public void HideGameOverUI()
    {
        gameOverUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示设置界面框
    /// </summary>
    public void ShowSettingUI()
    {
        settingUI.gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏设置界面框
    /// </summary>
    public void HideSettingUI()
    {
        settingUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 音量图标上的斜杠，设置为显示
    /// </summary>
    public void SetMute()
    {
        audioMuteUI.gameObject.SetActive(true);
    }

    /// <summary>
    /// 音量图标上的斜杠，设置为隐藏
    /// </summary>
    public void SetAudio()
    {
        audioMuteUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示游戏分数与次数记录栏
    /// </summary>
    public void ShowRankUI(int score, int highScore, int gameNumber)
    {
        rankUI.gameObject.SetActive(true);
        rankScoreText.text = score.ToString();
        rankHighScoreText.text = highScore.ToString();
        rankGameNumberText.text = gameNumber.ToString();
    }

    /// <summary>
    /// 隐藏游戏分数与次数记录栏
    /// </summary>
    public void HideRankUI()
    {
        rankUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 重置分数栏数据
    /// </summary>
    public void ResetRankData(int score, int highScore, int gameNumbers)
    {
        rankScoreText.text = score.ToString();
        rankHighScoreText.text = highScore.ToString();
        rankGameNumberText.text = gameNumbers.ToString();
    }

    /// <summary>
    /// 重新加载该场景
    /// </summary>
    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //游戏开始读取初始分数和历史最高分数
    public void ShowScore(int currentScore, int highScore)
    {
        scoreText.text = currentScore.ToString();
        highScoreText.text = highScore.ToString();
    }

    /// <summary>
    /// 每次消除一行调用一次，更新分数
    /// </summary>
    /// <param name="currentScore">现在的分数</param>
    public void UpdateScore(int currentScore)
    {
        scoreText.text = currentScore.ToString();
    }

}
