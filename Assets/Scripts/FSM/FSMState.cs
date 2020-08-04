using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Transition
{
    NullTransition,
    StartClickButton,               
    PauseClickButton,
    RestartToGame
}

public enum StateID
{
    NullStateID,
    Menu,
    Play,
    Pause,
    GameOver 
}

public abstract class FSMState 
{
    protected StateID stateID;                

    public StateID iD
    {
        get
        {
            return stateID;
        }
    }

    protected Dictionary<Transition, StateID> map;                               //在该状态下能进行哪几种转换，每一个转换条件对应一个id，也就是触发该条件时会转换到哪种状态

    protected FSMSystem fsmSystem;                                                        //一个FSMSystem的引用，表示该状态是由哪个状态机来管理，构造函数中赋值

    protected Control ctrl;

    public FSMState(FSMSystem system, Control control)
    {
        ctrl = control;
        fsmSystem = system;
        map = new Dictionary<Transition, StateID>();
    }

    public void AddTransition(Transition trans, StateID id)
    {
        if (trans == Transition.NullTransition || id == StateID.NullStateID)
        {
            Debug.LogError("transition or id is null");
            return;
        }

        if (map == null)
            Debug.LogError("map is null");
        if (map.ContainsKey(trans))
        {
            Debug.LogError("transition is already in dictionary");
            return;
        }

        map.Add(trans, id);
    }

    public void DeleteTransition(Transition trans)
    {
        if (map.ContainsKey(trans) == false)
        {
            Debug.LogWarning("this transition did not exist");
            return;
        }
        map.Remove(trans);
    }

    public StateID GetStateID(Transition trans)
    {
        if (map.ContainsKey(trans))
            return map[trans];
        return StateID.NullStateID;
    }

    /// <summary>
    /// 进入该状态时调用
    /// </summary>
    public virtual void DoWhileEntering() { }

    /// <summary>
    /// 出状态时调用
    /// </summary>
    public virtual void DoWhileLeaving() { }

    /// <summary>
    /// 状态中每时每刻调用
    /// </summary>
    public virtual void Act() { }

    /// <summary>
    /// 转换的条件
    /// </summary>
    public virtual void Reason() { }
}