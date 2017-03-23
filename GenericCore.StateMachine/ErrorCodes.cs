using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.StateMachine
{
    public enum ErrorCodes
    {
        AlreadyPresenT = 1,
        UnknownState,
        InvalidArc,
        AlreadyPresentArc,
        UnknownArc,
        InvalidTransition,
        //PostedStateAreadySet,
        AlreadyTransiting
    }
}
