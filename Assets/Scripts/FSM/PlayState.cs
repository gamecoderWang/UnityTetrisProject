using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayState : FSMState
{
    private Button pauseButton;
    private Button saveButton;
    private Button restartButton;
    private Button homeButton;

    public PlayState(FSMSystem system, Control control) : base(system, control)
    {
        stateID = StateID.Play;
        pauseButton = GameObject.Find("View/Canvas/GameUI/PauseButton").GetComponent<Button>();
        saveButton = GameObject.Find("View/Canvas/GameUI/SaveButton").GetComponent<Button>();
        restartButton = GameObject.Find("View/Canvas/GameOverUI/RestartButton").GetComponent<Button>();
        homeButton = GameObject.Find("View/Canvas/GameOverUI/HomeButton").GetComponent<Button>();
        restartButton.onClick.AddListener(Restart);
        saveButton.onClick.AddListener(Save);
        pauseButton.onClick.AddListener(Pause);
        homeButton.onClick.AddListener(BackToMain);
    }

    public override void DoWhileEntering()
    {
        ctrl.audioManager.PlayMouseClickAudio();
        ctrl.view.ShowGame();
        ctrl.cameraManager.ZoomIn();
        ctrl.gameController.StartGame();
        ctrl.view.ShowScore(ctrl.model.CurrentScrore, ctrl.model.HighScore);
        //ctrl.gameController.ShowCurrentShape();
    }

    public override void DoWhileLeaving()
    {
        ctrl.audioManager.PlayMouseClickAudio();
        ctrl.view.HideGame();
        ctrl.cameraManager.ZoomOut();
        ctrl.view.ShowRestartButton();
        ctrl.gameController.PauseGame();
        //ctrl.gameController.HideCurrentShape();
    }

    /// <summary>
    /// 游戏中按下暂停按钮时触发
    /// </summary>
    private void Pause()
    {
        fsmSystem.PerformTransition(Transition.PauseClickButton);
    }

    /// <summary>
    /// 游戏中按下保存按钮时触发
    /// </summary>
    private void Save()
    {
        ctrl.model.SaveData();
    }

    /// <summary>
    /// 游戏结束后按下重新开始时触发
    /// </summary>
    private void Restart()
    {
        ctrl.view.HideGameOverUI();
        ctrl.gameController.ClearAllShape();
        ctrl.gameController.StartGame();
        ctrl.view.ShowScore(ctrl.model.CurrentScrore, ctrl.model.HighScore);
    }

    /// <summary>
    /// 游戏结束后按下返回主菜单按钮时触发
    /// </summary>
    private void BackToMain()
    {
        ctrl.view.LoadScene();
    }
}
