using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Support
{
    public class PatternMatcher<TOutput>
    {
        private IList<Tuple<Predicate<object>, Func<object, TOutput>>> _cases;

        public PatternMatcher()
        {
            _cases = new List<Tuple<Predicate<object>, Func<object, TOutput>>>();
        }

        public PatternMatcher<TOutput> Case(Predicate<object> condition, Func<object, TOutput> function)
        {
            condition.AssertNotNull("condition");
            function.AssertNotNull("function");

            _cases.Add(new Tuple<Predicate<object>, Func<object, TOutput>>(condition, function));
            return this;
        }

        public PatternMatcher<TOutput> Case<T>(Predicate<T> condition, Func<T, TOutput> function)
        {
            return 
                Case
                (
                    o => o is T && condition((T)o),
                    o => function((T)o)
                );
        }

        public PatternMatcher<TOutput> Case<T>(Func<T, TOutput> function)
        {
            return 
                Case
                (
                    o => o is T,
                    o => function((T)o)
                );
        }

        public PatternMatcher<TOutput> Case<T>(Predicate<T> condition, TOutput o)
        {
            return Case(condition, x => o);
        }

        public PatternMatcher<TOutput> Case<T>(TOutput o)
        {
            return Case<T>(x => o);
        }

        public PatternMatcher<TOutput> Default(Func<object, TOutput> function)
        {
            return Case(o => true, function);
        }

        public PatternMatcher<TOutput> Default(TOutput o)
        {
            return Default(x => o);
        }

        public TOutput Match(object o)
        {
            foreach (var tuple in _cases)
            {
                if (tuple.Item1(o))
                {
                    return tuple.Item2(o);
                }
            }
                
            throw new Exception("Failed to match");
        }
    }
}
