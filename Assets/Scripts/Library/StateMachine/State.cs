using UnityEngine;
using System;
using Zenject;

public abstract class State<TStateMachine, TState, TStateType, TStateFactory>
    where TStateMachine : StateMachine<TStateMachine, TState, TStateType, TStateFactory>
    where TState : State<TStateMachine, TState, TStateType, TStateFactory>
    where TStateType : struct, IComparable, IFormattable, IConvertible
    where TStateFactory : StateFactory<TStateMachine, TState, TStateType, TStateFactory>
{
    protected TStateType stateType;

    protected readonly TStateMachine stateMachine;

    public State(TStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public TStateType StateType
    {
        get
        {
            return stateType;
        }
    }

    public virtual void Initialize()
    {
        // Do Nothing
    }

    public virtual void Update()
    {
        // Do Nothing
    }

    public virtual void EnterState()
    {
        // Do Nothing
    }

    public virtual void ExitState()
    {
        // Do Nothing
    }
}
