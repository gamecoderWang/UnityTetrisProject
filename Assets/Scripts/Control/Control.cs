using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    [HideInInspector]
    public Model model;
    [HideInInspector]
    public View view;
    [HideInInspector]
    public CameraManager cameraManager;
    [HideInInspector]
    public GameController gameController;
    [HideInInspector]
    public AudioManager audioManager;

    private FSMSystem fsm;

    private void Awake()
    {
        model = GameObject.FindGameObjectWithTag("Model").GetComponent<Model>();
        view = GameObject.FindGameObjectWithTag("View").GetComponent<View>();
        cameraManager = GetComponent<CameraManager>();
        gameController = GetComponent<GameController>();
        audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
        MakeFSM();
    }

    /// <summary>
    /// 生成状态机，将所有状态和转换条件添加进去，并设置初始状态
    /// </summary>
    private void MakeFSM()
    {
        fsm = new FSMSystem();

        MenuState menuState = new MenuState(fsm, this);
        menuState.AddTransition(Transition.StartClickButton, StateID.Play);
        menuState.AddTransition(Transition.RestartToGame, StateID.Play);
        //todo

        PlayState playState = new PlayState(fsm, this);
        playState.AddTransition(Transition.PauseClickButton, StateID.Menu);
        //todo

        fsm.AddState(menuState);
        fsm.AddState(playState);

        fsm.Start(StateID.Menu);
    }
}
