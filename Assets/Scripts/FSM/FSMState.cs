using System;
using System.Collections;
using System.Collections.Generic;

public abstract class FSMState<T> : Object
{
    public abstract void Enter(T t);
    public abstract void Execute(T t);
    public abstract void Exit(T t);
    public abstract bool HandleMessage(Message msg);
}

