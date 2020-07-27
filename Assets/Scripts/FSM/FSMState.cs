using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMState<T> : Object
{
    public abstract void enter(T t);
    public abstract void execute(T t);
    public abstract void exit(T t);
}

