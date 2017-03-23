using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.StateMachine.EventArgs
{
    public class AbstracTEventArgs<T> : System.EventArgs
    {
        public State<T> State { get; private set; }

        public AbstracTEventArgs(State<T> state)
        {
            state.AssertNotNull("state");
            State = state;
        }
    }
}
