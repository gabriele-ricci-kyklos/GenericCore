using GenericCore.StateMachine.EventArgs;
using GenericCore.StateMachine.Exceptions;
using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.StateMachine
{
    public class StateMachine<T>
    {
        private IDictionary<T, State<T>> _statesMap;
        private State<T> this[T state]
        {
            get
            {
                if (!_statesMap.ContainsKey(state))
                {
                    return null;
                }

                return _statesMap[state];
            }
        }

        public bool IsTransiting;
        public State<T> CurrentState { get; private set; }

        public event EventHandler<TransitingEventArgs<T>> Transiting;
        public event EventHandler<TransitedEventArgs<T>> Transited;

        protected virtual void OnTransiting(TransitingEventArgs<T> e)
        {
            if (Transiting != null)
            {
                Transiting(this, e);
            }
        }

        protected virtual void OnTransited(TransitedEventArgs<T> e)
        {
            if (Transited != null)
            {
                Transited(this, e);
            }
        }

        public StateMachine()
        {
            _statesMap = new Dictionary<T, State<T>>();
            IsTransiting = false;

            CurrentState = null;
        }

        public void AddState(T stateName)
        {
            if (this[stateName].IsNotNull())
            {
                throw new StateMachineException<T>(ErrorCodes.AlreadyPresenT, stateName);
            }

            State<T> state = new State<T>(stateName);
            _statesMap[stateName] = state;
        }

        public void AddTransition(T source, T target)
        {
            State<T> sourceState = this[source];

            if (sourceState.IsNull())
            {
                throw new StateMachineException<T>(ErrorCodes.UnknownState, source);
            }

            State<T> targetState = this[target];

            if (targetState.IsNull())
            {
                throw new StateMachineException<T>(ErrorCodes.UnknownState, target);
            }

            sourceState.AddTransition(targetState);
        }

        public void AddEnterStateCallback(T target, Action<State<T>> method)
        {
            State<T> targetState = this[target];

            if (targetState.IsNull())
            {
                throw new StateMachineException<T>(ErrorCodes.UnknownState, target);
            }

            targetState.AddStateCallback(method, true);
        }

        public void AddExitStateCallback(T target, Action<State<T>> method)
        {
            State<T> targetState = this[target];

            if (targetState.IsNull())
            {
                throw new StateMachineException<T>(ErrorCodes.UnknownState, target);
            }

            targetState.AddStateCallback(method, false);
        }

        public void AddTransitionCallback(T source, T target, Action<State<T>, State<T>> method)
        {
            State<T> state = this[source];

            if (state.IsNull())
            {
                throw new StateMachineException<T>(ErrorCodes.UnknownState, source);
            }

            state.AddTransitionCallback(target, method);
        }

        public void GoToState(T stateName)
        {
            try
            {
                if (IsTransiting)
                {
                    throw new StateMachineException<T>(ErrorCodes.AlreadyTransiting, stateName);
                }

                State<T> target = this[stateName];

                IsTransiting = true;
                OnTransiting(new TransitingEventArgs<T>(target));

                if (target.IsNull())
                {
                    throw new StateMachineException<T>(ErrorCodes.UnknownState, stateName);
                }

                if (CurrentState.IsNotNull())
                {
                    Transition<T> transition = CurrentState[target];

                    if (transition.IsNull())
                    {
                        throw new StateMachineException<T>(ErrorCodes.InvalidTransition, StateMachineException<T>.MakeArcName(CurrentState.Name, target.Name));
                    }

                    CurrentState.CallExitCallbacks();
                    transition.CallTransitionCallbacks();
                }

                CurrentState = target;
                target.CallEnterCallbacks();
                IsTransiting = false;
                OnTransited(new TransitedEventArgs<T>(CurrentState));
            }
            catch
            {
                IsTransiting = false;
                throw;
            }
        }
    }
}
