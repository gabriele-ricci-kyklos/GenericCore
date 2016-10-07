using GenericCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Trees.Common
{
    public class Tree<T>
    {
        private Node<T> _root;

        public Node<T> Root
        {
            get
            {
                return _root;
            }
        }

        public Tree(T rootValue)
        {
            rootValue.AssertNotNull("rootValue");
            _root = new Node<T>(rootValue);
        }
    }
}
