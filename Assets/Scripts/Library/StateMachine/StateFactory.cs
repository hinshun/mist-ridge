using UnityEngine;
using System;
using System.Collections;
using Zenject;

public abstract class StateFactory<TStateMachine, TState, TStateType, TStateFactory>
    where TStateMachine : StateMachine<TStateMachine, TState, TStateType, TStateFactory>
    where TState : State<TStateMachine, TState, TStateType, TStateFactory>
    where TStateType : struct, IComparable, IFormattable, IConvertible
    where TStateFactory : StateFactory<TStateMachine, TState, TStateType, TStateFactory>
{
    protected readonly DiContainer container;

    public StateFactory(DiContainer container)
    {
        this.container = container;
    }

    public abstract TState Create(TStateType stateType, params object[] constructorArgs);
}
