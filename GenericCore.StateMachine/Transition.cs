using GenericCore.StateMachine.Exceptions;
using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.StateMachine
{
    public class Transition<T>
    {
        private IList<Action<State<T>, State<T>>> _callbacks;

        public State<T> Source { get; }
        public State<T> Target { get; }

        public bool IsUnlinked
        {
            get
            {
                return Source.IsNull() || Target.IsNull();
            }
        }

        public Transition(State<T> source, State<T> target)
        {
            Source = source;
            Target = target;

            _callbacks = new List<Action<State<T>, State<T>>>();

            if (IsUnlinked)
            {
                throw new StateMachineException<T>(ErrorCodes.InvalidArc, StateMachineException<T>.MakeArcName(source.Name, target.Name));
            }
        }

        public void AddTransitionCallback(Action<State<T>, State<T>> method)
        {
            _callbacks.Add(method);
        }

        public void CallTransitionCallbacks()
        {
            _callbacks.ForEach(callback => callback(Source, Target));
        }
    }
}
