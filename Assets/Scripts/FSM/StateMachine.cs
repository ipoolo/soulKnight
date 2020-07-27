using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StateMachine<T>
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
        currState.enter(_stateOwner);
    }
    public void StateMachineUpdate(T t)
    {
       
        if (globalState != null) {
            globalState.execute(t);
        }
        if (currState != null)
        {
            currState.execute(t);
        }
    }

    public void ChangeState(FSMState<T> _newState)
    {
        if (currState != null && (_newState != currState)) { 
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
}
