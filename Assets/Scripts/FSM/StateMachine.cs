using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StateMachine<T>
{
    T stateOwner;
    public FSMState<T> currState;
    FSMState<T> preState;
    FSMState<T> globalState;

    public void ConfigState(T _stateOwner, FSMState<T> _currState, FSMState<T> _globalState)
    {
        stateOwner = _stateOwner;
        currState = _currState;
        globalState = _globalState;
        currState.Enter(_stateOwner);
    }
    public void StateMachineUpdate(T t)
    {
       
        if (globalState != null) {
            globalState.Execute(t);
        }
        if (currState != null)
        {
            currState.Execute(t);
        }
    }

    public void ChangeState(FSMState<T> _newState)
    {
        if (currState != null && (_newState != currState)) { 
            currState.Exit(stateOwner);
            preState = currState;
            currState = _newState;
            currState.Enter(stateOwner);

        }
    }

    public void RevertToPreviousState()
    {
        ChangeState(preState);
    }

    public bool receiveMessage(Message msg)
    {
        bool result = false;
        if (currState != null)
        {
            result = currState.HandleMessage(msg);
        }

        if (result)
        {//如果已处理则返回
            return result;
        }
        else if(globalState != null)
        {
            result = globalState.HandleMessage(msg);
        }
        return result;
    }
}
