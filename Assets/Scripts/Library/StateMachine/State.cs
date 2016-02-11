using UnityEngine;
using System;
using Zenject;

public abstract class State<TStateMachine, TState, TStates, TStateFactory>
    where TStateMachine : StateMachine<TStateMachine, TState, TStates, TStateFactory>
    where TState : State<TStateMachine, TState, TStates, TStateFactory>
    where TStates : struct, IComparable, IFormattable, IConvertible
    where TStateFactory : StateFactory<TStateMachine, TState, TStates, TStateFactory>
{
    public TStates stateReference { get; protected set; }

    protected readonly TStateMachine stateMachine;

    public State(TStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Update();

    public abstract void EnterState();

    public abstract void ExitState();
}
