using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Support
{
    public sealed class Counter
    {
        private int _lastN;
        private static readonly Counter _instance = new Counter();

        public static Counter Instance
        {
            get
            {
                return _instance;
            }
        }

        public static int Next
        {
            get
            {
                return Instance.GetNext();
            }
        }

        public static int Last
        {
            get
            {
                return Instance.GetLast();
            }
        }

        private Counter()
        {
            _lastN = 0;
        }

        static Counter()
        {
        }
        
        public int GetNext()
        {
            return ++_lastN;
        }

        public int GetLast()
        {
            return _lastN;
        }
    }
}
