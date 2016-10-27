using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCore.Collections.Trees
{
    public interface ITreeNode<T>
    {
        void Add(T node);
        void Traverse(Node<T> node, Action<T> predicate);
    }
}
