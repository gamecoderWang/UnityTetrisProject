using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuState : FSMState
{
    private Button startButton;
    private Button restartButton;
    private Button settingButton;
    private Button audioButton;
    private Button closeSettingUIButton;
    private Button rankButton;
    private Button closeRankUIButton;
    private Button clearRankUIDataButton;

    public MenuState(FSMSystem system, Control control) : base(system, control)
    {
        stateID = StateID.Menu;
        startButton = GameObject.Find("View/Canvas/MenuUI/StartButton").GetComponent<Button>();
        restartButton = GameObject.Find("View/Canvas/MenuUI/RestartButton").GetComponent<Button>();
        settingButton = GameObject.Find("View/Canvas/MenuUI/SettingButton").GetComponent<Button>();
        audioButton = GameObject.Find("View/Canvas/SettingUI/AudioButton").GetComponent<Button>();
        closeSettingUIButton = GameObject.Find("View/Canvas/SettingUI").GetComponent<Button>();
        rankButton = GameObject.Find("View/Canvas/MenuUI/RankButton").GetComponent<Button>();
        closeRankUIButton = GameObject.Find("View/Canvas/RankUI").GetComponent<Button>();
        clearRankUIDataButton = GameObject.Find("View/Canvas/RankUI/ClearButton").GetComponent<Button>();
        startButton.onClick.AddListener(Reason);
        restartButton.onClick.AddListener(RestartGame);
        settingButton.onClick.AddListener(ctrl.view.ShowSettingUI);
        audioButton.onClick.AddListener(SetAudio);
        closeSettingUIButton.onClick.AddListener(ctrl.view.HideSettingUI);
        rankButton.onClick.AddListener(ShowRankUI);
        closeRankUIButton.onClick.AddListener(ctrl.view.HideRankUI);
        clearRankUIDataButton.onClick.AddListener(ClearData);
    }

    public override void DoWhileEntering()
    {
        ctrl.view.ShowMenu();
        ctrl.cameraManager.ZoomOut();
    }

    public override void DoWhileLeaving()
    {
        ctrl.view.HideMenu();
    }

    public override void Reason()
    {
        fsmSystem.PerformTransition(Transition.StartClickButton);
    }

    /// <summary>
    /// 设置界面中设置音效
    /// </summary>
    private void SetAudio()
    {
        if(ctrl.audioManager.isMute)
        {
            ctrl.view.SetAudio();
            ctrl.audioManager.isMute = false;
            ctrl.audioManager.PlayMouseClickAudio();
        }
        else
        {
            ctrl.view.SetMute();
            ctrl.audioManager.isMute = true;
        }
    }


    private void ShowRankUI()
    {
        ctrl.view.ShowRankUI(ctrl.model.CurrentScrore, ctrl.model.HighScore, ctrl.model.GameNumbers);
    }

    /// <summary>
    /// 分数栏清除按钮调用
    /// </summary>
    private void ClearData()
    {
        ctrl.model.ClearAllData();
        ctrl.model.SaveData();
        ctrl.view.ResetRankData(ctrl.model.CurrentScrore, ctrl.model.HighScore, ctrl.model.GameNumbers);
        ctrl.view.ShowScore(ctrl.model.CurrentScrore, ctrl.model.HighScore);
    }

    /// <summary>
    /// 重新开始游戏时调用
    /// </summary>
    private void RestartGame()
    {
        ctrl.gameController.ClearAllShape();
        ctrl.model.ReadData();
        fsmSystem.PerformTransition(Transition.RestartToGame);
    }
}
