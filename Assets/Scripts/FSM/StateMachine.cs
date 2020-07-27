using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StateMachine<T> : Object
{
    T stateOwner;
    FSMState<T> currState;
    FSMState<T> preState;
    FSMState<T> globalState;

    public void ConfigState(T _stateOwner, FSMState<T> _currState, FSMState<T> _globalState)
    {
        stateOwner = _stateOwner;
        currState = _currState;
        globalState = _globalState;
    }
    public void StateMachineUpdate(T t)
    {
        if (globalState) { 
            globalState.execute(t);
        }
        if (currState)
        {
            globalState.execute(t);
        }
    }

    public void ChangeState(FSMState<T> _newState)
    {
        if (currState) { 
            currState.exit(stateOwner);
            preState = currState;
            currState = _newState;
            currState.enter(stateOwner);


        }
    }

    public void RevertToPreviousState()
    {
        ChangeState(preState);
    }
    //
}
