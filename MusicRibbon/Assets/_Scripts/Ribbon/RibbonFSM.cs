using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonFSM<TContext> {
    //Finite State manager may not be necessary, but this is a stub in case we need it

    //states need access to the objects whose state they represent
    private readonly TContext _context;

    private readonly Dictionary<Type, State> _stateCache = new Dictionary<Type, State>();

    public State CurrentState { get; private set; }

    //don't want to set new states immediately
    private State _pendingState;

    //constructor
    public RibbonFSM (TContext context) {
        _context = context;
    }

    public void Update() {

        //In case another method called TransitionTo externally
        PerformPendingTransition();

        Debug.Assert(CurrentState != null, "Updating State Machine with null current state. Need to transition to Starting State?");

        CurrentState.Update();


        PerformPendingTransition();

    }

    public void TransitionTo<TState>() where TState : State {
        _pendingState = GetOrCreateState<TState>();
    }

    private void PerformPendingTransition() {
        if (_pendingState != null) {
            if (CurrentState != null) CurrentState.OnExit();
            CurrentState = _pendingState;
            CurrentState.OnEnter();
            _pendingState = null;
        }
    }

    //helper method for managing the caching of state instances
    private TState GetOrCreateState<TState>() where TState : State {

        State state;

        if (_stateCache.TryGetValue (typeof (TState), out state)) {
            return (TState)state;
        }
        else {
            //required to create state instances using only the type
            var newState = Activator.CreateInstance<TState>();
            newState.Parent = this;
            newState.Init();
            _stateCache[typeof(TState)] = newState;
            return newState;
        }


    }

    public abstract class State {

        internal RibbonFSM<TContext> Parent { get; set; }

        public void Init() {

        }

        public void OnEnter() {

        }

        public void Update() {

        }

        public void OnExit() {

        }

    }
}
