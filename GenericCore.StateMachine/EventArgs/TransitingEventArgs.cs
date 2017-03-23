using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.StateMachine.EventArgs
{
    public class TransitingEventArgs<T> : AbstracTEventArgs<T>
    {
        public TransitingEventArgs(State<T> state)
            : base(state)
        {
        }
    }
}
