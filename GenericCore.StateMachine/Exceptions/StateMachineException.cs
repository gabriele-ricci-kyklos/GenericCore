﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.StateMachine.Exceptions
{
    public class StateMachineException<T> : Exception
    {
        public ErrorCodes ErrorCode { get; }

        public StateMachineException(ErrorCodes ec)
            : this(ec, "Unknown error from StateMachine", null)
        {
        }

        public StateMachineException(ErrorCodes ec, T currenT)
            : this(ec, currenT, null)
        {
        }

        public StateMachineException(ErrorCodes ec, string message)
            : this(ec, message, null)
        {
        }

        public StateMachineException(ErrorCodes ec, T currenT, Exception innerException)
            : this(ec, currenT.ToString(), innerException)
        {
            ErrorCode = ec;
        }

        private StateMachineException(ErrorCodes ec, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = ec;
        }

        public static string MakeArcName(T source, T target)
        {
            return string.Format("{0} -> {1}", source, target);
        }
    }
}
