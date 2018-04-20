using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore
{
    // credits: C# in Depth by Jon Skeet - http://csharpindepth.com/Articles/General/Singleton.aspx
    public sealed class Singleton<T> where T : class, new()
    {
        private static readonly T instance = new T();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Singleton()
        {
        }

        private Singleton()
        {
        }

        public static T Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
