using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

public abstract class StateMachine<TStateMachine, TState, TStateType, TStateFactory>
    where TStateMachine : StateMachine<TStateMachine, TState, TStateType, TStateFactory>
    where TState : State<TStateMachine, TState, TStateType, TStateFactory>
    where TStateType : struct, IComparable, IFormattable, IConvertible
    where TStateFactory : StateFactory<TStateMachine, TState, TStateType, TStateFactory>
{
    protected float timeEnteredState;
    protected TState state = null;
    protected TState lastState = null;
    protected readonly TStateFactory stateFactory;

    public StateMachine(
            TStateFactory stateFactory)
    {
        this.stateFactory = stateFactory;
    }

    public void ChangeState(TStateType stateType)
    {
        ChangingState();
        if (state != null)
        {
            state.ExitState();
        }

        state = stateFactory.Create(stateType);
        ChangedState(stateType);
        state.EnterState();
    }

    public TStateType CurrentStateType()
    {
        return state.StateType;
    }

    public void Tick()
    {
        EarlyGlobalUpdate();

        state.Update();

        LateGlobalUpdate();
    }

    private void ChangingState()
    {
        lastState = state;
        timeEnteredState = Time.time;
    }

    protected virtual void ChangedState(TStateType stateType) { }

    protected virtual void EarlyGlobalUpdate() { }

    protected virtual void LateGlobalUpdate() { }
}
