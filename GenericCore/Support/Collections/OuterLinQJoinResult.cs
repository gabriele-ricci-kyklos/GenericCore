﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Support.Collections
{
    public class OuterLinqJoinResult<TLeft, TRight>
    {
        public TLeft LeftPart { get; set; }
        public TRight RightPart { get; set; }
    }
}
