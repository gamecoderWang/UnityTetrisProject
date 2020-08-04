using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
    private FSMState currentstate;                       

    public FSMState currentState
    {
        get
        {
            return currentstate;
        }
    }

    private Dictionary<StateID, FSMState> statesDic;

    public FSMSystem()
    {
        statesDic = new Dictionary<StateID, FSMState>();
    }

    public void AddState(FSMState state)
    {
        if (state == null)
        {
            Debug.LogError("the state you want to add is null");
            return;
        }
        if (statesDic.ContainsKey(state.iD))
        {
            Debug.LogError("the state you want to add is already exist");
            return;
        }
        statesDic.Add(state.iD, state);
    }

    public void DeleteState(FSMState state)
    {
        if (state == null)
        {
            Debug.LogError("the state you want to delete is null");
            return;
        }
        if (statesDic.ContainsKey(state.iD) == false)
        {
            Debug.LogError("the state you want to delete is not exist");
            return;
        }
        statesDic.Remove(state.iD);
    }

    /// <summary>
    /// 控制状态的转换
    /// </summary>
    /// <param name="trans">转换条件</param>
    public void PerformTransition(Transition trans)
    {
        //首先判断传入的转换条件是否为空，为空直接返回
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("the transition is null");
            return;
        }

        //不为空的话，查找此转换条件下能转换成什么状态，返回状态的id
        StateID id = currentstate.GetStateID(trans);

        //如果是空id则返回，说明该状态不能进行这种转换
        if (id == StateID.NullStateID)
        {
            Debug.LogError("this transition will not happen");
            return;
        }

        //以上异常情况都排除，在状态集合中找到相应的状态，并赋值
        FSMState state;
        statesDic.TryGetValue(id, out state);
        currentstate.DoWhileLeaving();
        currentstate = state;
        currentstate.DoWhileEntering();
    }

    /// <summary>
    /// 状态机开始工作，设置一个初状态
    /// </summary>
    /// <param name="id">状态机的id</param>
    public void Start(StateID id)
    {
        if (id == StateID.NullStateID)
        {
            Debug.LogError("id is null");
            return;
        }
        FSMState state;
        if (statesDic.TryGetValue(id, out state))
        {
            currentstate = state;
            state.DoWhileEntering();
            return;
        }
        Debug.LogError("no such state");
    }
}
