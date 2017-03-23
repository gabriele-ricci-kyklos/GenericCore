using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.StateMachine.EventArgs
{
    public class TransitedEventArgs<T> : AbstracTEventArgs<T>
    {
        public TransitedEventArgs(State<T> state)
            : base(state)
        {
        }
    }
}
