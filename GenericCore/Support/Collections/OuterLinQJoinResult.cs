using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Support.Collections
{
    public class OuterLinQJoinResult<TLeft, TRight>
    {
        public TLeft LeftPart { get; set; }
        public TRight RightPart { get; set; }
    }
}
