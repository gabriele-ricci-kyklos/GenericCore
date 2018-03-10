using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCore.Support;
using System.Collections.ObjectModel;

namespace GenericCore.Collections.Trees
{
    public class Node<T> : ITreeNode<T>
    {
        private IList<Node<T>> _children = new List<Node<T>>();

        public bool IsRoot { get; }
        public ReadOnlyCollection<Node<T>> Children
        {
            get
            {
                return _children.AsReadOnly();
            }
        }

        public T Value { get; }

        public Node<T> this[int index]
        {
            get
            {
                return Children.ElementAt(index);
            }
        }

        public bool IsLeaf
        {
            get
            {
                return !Children.Any();
            }
        }

        public Node(T value)
        {
            value.AssertNotNull("value");

            Value = value;
            _children = new List<Node<T>>();
        }

        public void Add(T node)
        {
            _children.Add(new Node<T>(node));
        }

        public void Traverse(Node<T> node, Action<T> predicate)
        {
            predicate(node.Value);

            foreach (Node<T> kid in node.Children)
            {
                Traverse(kid, predicate);
            }
        }
    }
}
