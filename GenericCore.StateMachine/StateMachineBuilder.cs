using GenericCore.StateMachine.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.StateMachine
{
    public class StateMachineBuilder<TState>
    {
        private StateMachine<TState> _stateMachine;

        public static StateMachineBuilder<TState> New()
        {
            return new StateMachineBuilder<TState>();
        }

        private StateMachineBuilder()
        {
            _stateMachine = new StateMachine<TState>();
        }

        public StateMachineBuilder<TState> State(TState state)
        {
            _stateMachine.AddState(state);
            return this;
        }

        public StateMachineBuilder<TState> Transition(TState from, TState to)
        {
            _stateMachine.AddTransition(from, to);
            return this;
        }

        public StateMachineBuilder<TState> Entering(TState state, Action<State<TState>> action)
        {
            _stateMachine.AddEnterStateCallback(state, action);
            return this;
        }

        public StateMachineBuilder<TState> Exiting(TState state, Action<State<TState>> action)
        {
            _stateMachine.AddExitStateCallback(state, action);
            return this;
        }

        public StateMachineBuilder<TState> GoingTo(TState from, TState to, Action<State<TState>, State<TState>> action)
        {
            _stateMachine.AddTransitionCallback(from, to, action);
            return this;
        }

        public void GoToState(TState state)
        {
            _stateMachine.GoToState(state);
        }

        public StateMachineBuilder<TState> OnTransiting(Action<object, TransitingEventArgs<TState>> target)
        {
            _stateMachine.Transiting += new EventHandler<TransitingEventArgs<TState>>(target);
            return this;
        }

        public StateMachineBuilder<TState> OnTransited(Action<object, TransitedEventArgs<TState>> target)
        {
            _stateMachine.Transited += new EventHandler<TransitedEventArgs<TState>>(target);
            return this;
        }
    }
}
