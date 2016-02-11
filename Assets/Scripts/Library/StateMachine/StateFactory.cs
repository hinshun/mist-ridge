using UnityEngine;
using System;
using System.Collections;
using Zenject;

public abstract class StateFactory<TStateMachine, TState, TStates, TStateFactory>
    where TStateMachine : StateMachine<TStateMachine, TState, TStates, TStateFactory>
    where TState : State<TStateMachine, TState, TStates, TStateFactory>
    where TStates : struct, IComparable, IFormattable, IConvertible
    where TStateFactory : StateFactory<TStateMachine, TState, TStates, TStateFactory>
{
    protected readonly DiContainer container;

    public StateFactory(DiContainer container)
    {
        this.container = container;
    }

    public abstract TState Create(TStates stateReference, params object[] constructorArgs);
}
