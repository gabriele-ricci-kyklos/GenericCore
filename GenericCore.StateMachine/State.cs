using GenericCore.StateMachine.Exceptions;
using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.StateMachine
{
    public class State<T>
    {
        private Dictionary<T, Transition<T>> _transitionsMap;
        private IList<Action<State<T>>> _enterCallbacks;
        private IList<Action<State<T>>> _exitCallbacks;

        public T Name { get; private set; }

        public Transition<T> this[State<T> state]
        {
            get
            {
                if (!_transitionsMap.ContainsKey(state.Name))
                {
                    return null;
                }

                return _transitionsMap[state.Name];
            }
        }

        public State(T stateName)
        {
            Name = stateName;

            _transitionsMap = new Dictionary<T, Transition<T>>();
            _enterCallbacks = new List<Action<State<T>>>();
            _exitCallbacks = new List<Action<State<T>>>();
        }

        public void AddTransition(State<T> state)
        {
            if (_transitionsMap.ContainsKey(state.Name))
            {
                throw new StateMachineException<T>(ErrorCodes.AlreadyPresentArc, StateMachineException<T>.MakeArcName(Name, state.Name));
            }

            Transition<T> transition = new Transition<T>(this, state);
            _transitionsMap[state.Name] = transition;
        }

        public void AddStateCallback(Action<State<T>> method, bool enter)
        {
            if (enter)
            {
                _enterCallbacks.Add(method);
            }
            else
            {
                _exitCallbacks.Add(method);
            }
        }

        public void AddTransitionCallback(T target, Action<State<T>, State<T>> method)
        {
            if (!_transitionsMap.ContainsKey(target))
            {
                throw new StateMachineException<T>(ErrorCodes.UnknownArc, StateMachineException<T>.MakeArcName(Name, target));
            }

            Transition<T> transition = _transitionsMap[target];

            transition.AddTransitionCallback(method);
        }

        public void CallEnterCallbacks()
        {
            _enterCallbacks.ForEach(callback => callback(this));
        }

        public void CallExitCallbacks()
        {
            _exitCallbacks.ForEach(callback => callback(this));
        }
    }
}
