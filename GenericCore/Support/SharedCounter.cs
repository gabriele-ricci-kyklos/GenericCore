using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GenericCore.Support
{
    public static class SharedCounter
    {
        private static long _counter = 0;
        private static string _dailyReset = null;

        public static long NextId
        {
            get
            {
                Interlocked.Increment(ref _counter);
                return _counter;
            }
        }

        public static long DailyNextId
        {
            get
            {
                if (_counter > 0 && DateTime.Parse(_dailyReset) != DateTime.Now.Date)
                {
                    Interlocked.Exchange(ref _counter, 0);
                    Interlocked.Exchange(ref _dailyReset, null);
                }

                if (_dailyReset.IsNullOrEmpty())
                {
                    Interlocked.Exchange(ref _dailyReset, DateTime.Now.ToShortDateString());
                }

                Interlocked.Increment(ref _counter);

                return _counter;
            }
        }

        public static DateTime? CurrentDay
        {
            get
            {
                return _dailyReset.IsNullOrEmpty() ? null : new DateTime?(DateTime.Parse(_dailyReset));
            }
        }
    }

}
