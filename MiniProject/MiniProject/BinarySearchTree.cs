using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;


namespace MiniProject
{
    public class BinaryTree<T> : ICollection<T>, IEnumerable, IEnumerable<T>
    {
        private Node<T> root;
        private Comparer<T> comparer;
        private int count;

        public BinaryTree()
        {
            root = null;
            count = 0;
            comparer = Comparer<T>.Default;
        }

        public void Clear()
        {
            root = null;
        }

        private Node<T> Root
        {
            get
            {
                return root;
            }
            set
            {
                root = value;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public enum TraversalMethods { Preorder = -1, Inorder = 0, Postorder = 1 };

        private List<T> InorderTraversal(Node<T> currentNode, List<T> result)
        {
            if (result == null)
            {
                result = new List<T>();
            }
            if (currentNode != null)
            {
                InorderTraversal(currentNode.Left, result);
                result.Add(currentNode.Value);
                InorderTraversal(currentNode.Right, result);
            }
            return result;
        }

        private List<T> PreorderTraversal(Node<T> currentNode, List<T> result)
        {
            if (result == null)
            {
                result = new List<T>();
            }
            if (currentNode != null)
            {
                result.Add(currentNode.Value);
                PreorderTraversal(currentNode.Left, result);
                PreorderTraversal(currentNode.Right, result);
            }
            return result;
        }

        private List<T> PostorderTraversal(Node<T> currentNode, List<T> result)
        {
            if (result == null)
            {
                result = new List<T>();
            }
            if (currentNode != null)
            {
                PostorderTraversal(currentNode.Left, result);
                PostorderTraversal(currentNode.Right, result);
                result.Add(currentNode.Value);
            }
            return result;

        }

        public Boolean Contains(T data)
        {
            Node<T> current = root;
            int result;
            while (current != null)
            {
                result = comparer.Compare(current.Value, data);
                if (result == 0)
                {
                    return true;
                }
                else if (result > 0)
                {
                    current = current.Left;
                }
                else if (result < 0)
                {
                    current = current.Right;
                }
            }
            return false;
        }

        public void Add(T data)
        {
            Root = Add(data, Root);
        }

        private Node<T> Add(T data, Node<T> node)
        {
            if (node == null)
            {
                count++;
                return (new Node<T>(data));
            }

            int result = comparer.Compare(node.Value, data);
            if (result == 0)
            {
                node.Occurency++;
                return node; // Already exists, so just back out.
            }
            else if (result > 0)
            {
                node.Left = Add(data, node.Left);
            }
            else if (result < 0)
            {
                node.Right = Add(data, node.Right);
            }
            
            node.Height = Max(GetNodeHeight(node.Left), GetNodeHeight(node.Right)) + 1;
            int balance = GetNodeBalance(node);
            
            if (balance > 1 && comparer.Compare(data, node.Left.Value) < 0)
            {
                node = RightRotate(node);
            }
            else if (balance < -1 && comparer.Compare(data, node.Right.Value) > 0)
            {
                node = LeftRotate(node);
            }
            else if (balance > 1 && comparer.Compare(data, node.Left.Value) > 0)
            {
                node.Left = LeftRotate(node.Left);
                node = RightRotate(node);
            }
            else if (balance < -1 && comparer.Compare(data, node.Right.Value) < 0)
            {
                node.Right = RightRotate(node.Right);
                node = LeftRotate(node);
            }
            return node;
        }

        private Node<T> RightRotate(Node<T> Q)
        {
            // v.2
            Node<T> P = Q.Left; // P = new root
            Node<T> B = Q.Left.Right; // B = P's right child (Q grandchild) > Q's left child

            P.Right = Q;
            Q.Left = B;
              
            Q.Height = Max(GetNodeHeight(Q.Left), GetNodeHeight(Q.Right)) + 1;
            P.Height = Max(GetNodeHeight(P.Left), GetNodeHeight(P.Right)) + 1;

            return P;
        }

        private Node<T> LeftRotate(Node<T> P)
        {
            Node<T> Q = P.Right;
            Node<T> B = Q.Left;

            Q.Left = P;
            P.Right = B;

            P.Height = Max(GetNodeHeight(P.Left), GetNodeHeight(P.Right)) + 1;
            Q.Height = Max(GetNodeHeight(Q.Left), GetNodeHeight(Q.Right)) + 1;

            return Q;
        }

        private int GetNodeBalance(Node<T> node)
        {
            if (node == null)
                return 0;
            return GetNodeHeight(node.Left) - GetNodeHeight(node.Right);
        }

        private int GetNodeHeight(Node<T> node)
        {
            if (node == null)
                return 0;
            return node.Height;
        }

        private int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        public Boolean isBST(T minValue, T maxValue)
        {
            return isBST(Root, minValue, maxValue);
        }

        private Boolean isBST(Node<T> node, T minValue, T maxValue)
        {
            if (node == null)
                return true;
            if (comparer.Compare(node.Value, minValue) < 0 || comparer.Compare(node.Value, maxValue) > 0)
            {
                Console.WriteLine("Found an invalid node: " + node.Value);
                return false;
            }
            return isBST(node.Left, minValue, node.Value) && isBST(node.Right, node.Value, maxValue);
        }

        public Boolean Remove(T data)
        {
            if (root == null)
            {
                return false;
            }

            Node<T> current = root;
            Node<T> parent = null;

            int result = comparer.Compare(current.Value, data);
            while (result != 0)
            {
                if (result > 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Right;
                }
                if (current == null)
                {
                    return false;
                }
                else
                {
                    result = comparer.Compare(current.Value, data);
                }
            }
            count--;

            if (current.Right == null)
            {
                if (parent == null)
                {
                    root = current.Left;
                }
                else
                {
                    result = comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                    {
                        parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        parent.Right = current.Left;
                    }
                }
            }
            else if (current.Right.Left == null)
            {
                current.Right.Left = current.Left;

                if (parent == null)
                {
                    root = current.Right;
                }
                else
                {
                    result = comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                    {
                        parent.Left = current.Right;
                    }
                    else if (result < 0)
                    {
                        parent.Right = current.Right;
                    }
                }
            }
            else
            {
                Node<T> leftNode = current.Right.Left;
                Node<T> leftNodeParent = current.Right;
                while (leftNode.Left != null)
                {
                    leftNodeParent = leftNode;
                    leftNode = leftNode.Left;
                }

                leftNodeParent.Left = leftNode.Right;

                leftNode.Left = current.Left;
                leftNode.Right = current.Right;

                if (parent == null)
                {
                    root = leftNode;
                }
                else
                {
                    result = comparer.Compare(parent.Value, current.Value);
                    if (result > 0)
                    {
                        parent.Left = leftNode;
                    }
                    else if (result < 0)
                    {
                        parent.Right = leftNode;
                    }
                }
            }
            return true;
        }

        public String PrintTree(TraversalMethods method)
        {
            return PrintTree(method, Root, "");
        }

        public String PrintTree(TraversalMethods method, Node<T> currentNode, String result)
        {
            switch (method)
            {
                case TraversalMethods.Preorder:
                    if (currentNode != null)
                    {
                        result = result + currentNode.Value + " " + currentNode.Occurency + ", ";
                        result = PrintTree(method, currentNode.Left, result);
                        result = PrintTree(method, currentNode.Right, result);
                    }
                    break;
                default:
                case TraversalMethods.Inorder:
                    if (currentNode != null)
                    {
                        result = PrintTree(method, currentNode.Left, result);
                        result = result + currentNode.Value + " " + currentNode.Occurency + ", ";
                        result = PrintTree(method, currentNode.Right, result);
                    }
                    break;
                case TraversalMethods.Postorder:
                    if (currentNode != null)
                    {
                        result = PrintTree(method, currentNode.Left, result);
                        result = PrintTree(method, currentNode.Right, result);
                        result = result + currentNode.Value + " " + currentNode.Occurency + ", ";
                    }
                    break;
            }
            return result.TrimEnd(new char[] { ',' });
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex, TraversalMethods.Inorder);
        }

        public void CopyTo(T[] array, int arrayIndex, TraversalMethods method)
        {
            if (Root == null)
            {
                return;
            }
            switch (method)
            {
                case TraversalMethods.Preorder:
                    PreorderTraversal(Root, new List<T>()).CopyTo(array, arrayIndex);
                    break;
                default:
                case TraversalMethods.Inorder:
                    InorderTraversal(Root, new List<T>()).CopyTo(array, arrayIndex);
                    break;
                case TraversalMethods.Postorder:
                    PostorderTraversal(Root, new List<T>()).CopyTo(array, arrayIndex);
                    break;
            }
        }

        public IEnumerator<T> GetEnumerator(TraversalMethods method)
        {
            switch (method)
            {
                case TraversalMethods.Preorder:
                    return new BinaryTreeEnumerator<T>(PreorderTraversal(Root, new List<T>()).ToArray());
                default:
                case TraversalMethods.Inorder:
                    return new BinaryTreeEnumerator<T>(InorderTraversal(Root, new List<T>()).ToArray());
                case TraversalMethods.Postorder:
                    return new BinaryTreeEnumerator<T>(PostorderTraversal(Root, new List<T>()).ToArray());
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(TraversalMethods.Inorder);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }

    public class BinaryTreeEnumerator<T> : IEnumerator<T>
    {
        private T[] list;

        // Enumerators are positioned before the first element until first moveNext() is called.
        private int position = -1;

        public BinaryTreeEnumerator(T[] list)
        {
            this.list = list;
        }

        public T Current
        {
            get
            {
                try
                {
                    return list[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        /**
         * Not implemented as class doesn't use any managed resources.
         */
        public void Dispose()
        { }

        public bool MoveNext()
        {
            position++;
            return (position < list.Length);
        }

        public void Reset()
        {
            position = -1;
        }
    }

    /*
     * Node class, which is used in BinaryTree
     */
    public class Node<T>
    {
        private T data;
        private int height;
        private int occurency;
        private NodeList<T> children = null;

        private Node() { } //No default constructor

        public Node(T data) : this(data, null, null) { }

        public Node(T data, Node<T> left, Node<T> right)
        {
            this.data = data;
            children = new NodeList<T>(2);
            children[0] = left;
            children[1] = right;
            Height = 1;
            Occurency = 1;
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public int Occurency
        {
            get
            {
                return occurency;
            }
            set
            {
                occurency = value;
            }
        }

        public T Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        private NodeList<T> Children
        {
            get
            {
                return children;
            }
            set
            {
                children = value;
            }
        }

        public Node<T> Left
        {
            get
            {
                if (Children == null)
                {
                    return null;
                }
                else
                {
                    return Children[0];
                }
            }
            set
            {
                if (Children == null)
                {
                    Children = new NodeList<T>(2);
                }
                Children[0] = value;
            }
        }

        public Node<T> Right
        {
            get
            {
                if (Children == null)
                {
                    return null;
                }
                else
                {
                    return Children[1];
                }
            }
            set
            {
                if (Children == null)
                {
                    Children = new NodeList<T>(2);
                }
                Children[1] = value;
            }
        }
    }

    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList() : base() { }

        public NodeList(int size)
        {
            for (int i = 0; i < size; i++)
            {
                base.Items.Add(default(Node<T>));

            }
        }

        public Node<T> FindByValue(T value)
        {
            foreach (Node<T> node in Items)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                }
            }
            return null;
        }
    }
}

