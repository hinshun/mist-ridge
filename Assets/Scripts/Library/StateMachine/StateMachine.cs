using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public abstract class StateMachine<TStateMachine, TState, TStates, TStateFactory> : IInitializable
    where TStateMachine : StateMachine<TStateMachine, TState, TStates, TStateFactory>
    where TState : State<TStateMachine, TState, TStates, TStateFactory>
    where TStates : struct, IComparable, IFormattable, IConvertible
    where TStateFactory : StateFactory<TStateMachine, TState, TStates, TStateFactory>
{
    protected TState state = null;
    protected TState lastState = null;
    protected readonly TStateFactory stateFactory;
    protected float timeEnteredState;

    public StateMachine(TStateFactory stateFactory)
    {
        this.stateFactory = stateFactory;
    }

    public abstract void Initialize();

    public void ChangeState(TStates stateReference, params object[] constructorArgs)
    {
        ChangingState();
        if (state != null)
        {
            state.ExitState();
        }

        state = stateFactory.Create(stateReference, constructorArgs);
        state.EnterState();
    }


    public TStates CurrentStateReference()
    {
        return state.stateReference;
    }

    void ChangingState()
    {
        lastState = state;
        timeEnteredState = Time.time;
    }


    public void Step()
    {
        EarlyGlobalUpdate();

        state.Update();

        LateGlobalUpdate();
    }

    protected virtual void EarlyGlobalUpdate() { }

    protected virtual void LateGlobalUpdate() { }
}
