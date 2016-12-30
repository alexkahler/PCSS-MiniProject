using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Server
{
    public class BinaryTree<K, V> : IEnumerable
    {
        private Node<K, V> root;
        private Comparer comparer;
        private int count;

        public BinaryTree()
        {
            root = null;
            count = 0;
            comparer = Comparer.Default;
        }

        public void Clear()
        {
            root = null;
        }

        private Node<K, V> Root
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

        public enum TraversalMethod { Preorder = -1, Inorder = 0, Postorder = 1 };
        public enum TraversalDirection { Forwards = 0, Backwards = 1 };

        private List<K> InorderTraversal(Node<K, V> currentNode, List<K> result)
        {
            if (result == null)
            {
                result = new List<K>();
            }
            if (currentNode != null)
            {
                InorderTraversal(currentNode.Left, result);
                result.Add(currentNode.Key);
                InorderTraversal(currentNode.Right, result);
            }
            return result;
        }

        private List<K> PreorderTraversal(Node<K, V> currentNode, List<K> result)
        {
            if (result == null)
            {
                result = new List<K>();
            }
            if (currentNode != null)
            {
                result.Add(currentNode.Key);
                PreorderTraversal(currentNode.Left, result);
                PreorderTraversal(currentNode.Right, result);
            }
            return result;
        }

        private List<K> PostorderTraversal(Node<K, V> currentNode, List<K> result)
        {
            if (result == null)
            {
                result = new List<K>();
            }
            if (currentNode != null)
            {
                PostorderTraversal(currentNode.Left, result);
                PostorderTraversal(currentNode.Right, result);
                result.Add(currentNode.Key);
            }
            return result;

        }

        public Boolean Contains(K key, V value)
        {
            Node<K, V> current = root;
            int result;
            while (current != null)
            {
                result = comparer.Compare(current.Key, key);
                if (result == 0)
                {
                    result = comparer.Compare(current.Value, value);
                    if(result > 0)
                    {
                        current = current.Left;
                    }
                    else if (result < 0)
                    {
                        current = current.Right;
                    }
                    else
                    {
                        return true;
                    }
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

        public void Add(K key, V value)
        {
            Root = Add(key, value, Root);
        }

        private Node<K, V> Add(K key, V value, Node<K, V> node)
        {
            if (node == null)
            {
                count++;
                return (new Node<K, V>(key, value));
            }

            int keyResult = comparer.Compare(node.Key, key);
            if (keyResult == 0)
            {
                int valueResult = comparer.Compare(node.Value, value);
                if (valueResult > 0)
                {
                    node.Left = Add(key, value, node.Left);
                }
                else if (valueResult < 0)
                {
                    node.Right = Add(key, value, node.Right);
                }
                else
                {
                    return node; // Already exists, so just back out.
                }
            }
            else if (keyResult > 0)
            {
                node.Left = Add(key, value, node.Left);
            }
            else if (keyResult < 0)
            {
                node.Right = Add(key, value, node.Right);
            }

            //Does unfortunately not work with this binary tree, where each node has a Key-Value pair :(
            /*
            node.Height = Max(GetNodeHeight(node.Left), GetNodeHeight(node.Right)) + 1;
            int balance = GetNodeBalance(node);
            
            if (balance > 1 && comparer.Compare(value, node.Left.Key) < 0)
            {
                node = RightRotate(node);
            }
            else if (balance < -1 && comparer.Compare(value, node.Right.Key) > 0)
            {
                node = LeftRotate(node);
            }
            else if (balance > 1 && comparer.Compare(value, node.Left.Key) > 0)
            {
                node.Left = LeftRotate(node.Left);
                node = RightRotate(node);
            }
            else if (balance < -1 && comparer.Compare(value, node.Right.Key) < 0)
            {
                node.Right = RightRotate(node.Right);
                node = LeftRotate(node);
            }*/
            
            return node;
        }

        private Node<K, V> RightRotate(Node<K, V> Q)
        {
            // v.2
            Node<K, V> P = Q.Left; // P = new root
            Node<K, V> B = Q.Left.Right; // B = P's right child (Q grandchild) > Q's left child

            P.Right = Q;
            Q.Left = B;

            Q.Height = Max(GetNodeHeight(Q.Left), GetNodeHeight(Q.Right)) + 1;
            P.Height = Max(GetNodeHeight(P.Left), GetNodeHeight(P.Right)) + 1;

            return P;
        }

        private Node<K, V> LeftRotate(Node<K, V> P)
        {
            Node<K, V> Q = P.Right;
            Node<K, V> B = Q.Left;

            Q.Left = P;
            P.Right = B;

            P.Height = Max(GetNodeHeight(P.Left), GetNodeHeight(P.Right)) + 1;
            Q.Height = Max(GetNodeHeight(Q.Left), GetNodeHeight(Q.Right)) + 1;

            return Q;
        }

        private int GetNodeBalance(Node<K, V> node)
        {
            if (node == null)
                return 0;
            return GetNodeHeight(node.Left) - GetNodeHeight(node.Right);
        }

        private int GetNodeHeight(Node<K, V> node)
        {
            if (node == null)
                return 0;
            return node.Height;
        }

        private int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        public Boolean isBST(K minValue, K maxValue)
        {
            return isBST(Root, minValue, maxValue);
        }

        private Boolean isBST(Node<K, V> node, K minValue, K maxValue)
        {
            if (node == null)
                return true;
            if (comparer.Compare(node.Key, minValue) < 0 || comparer.Compare(node.Key, maxValue) > 0)
            {
                Console.WriteLine("Found an invalid node: " + node.Key);
                return false;
            }
            return isBST(node.Left, minValue, node.Key) && isBST(node.Right, node.Key, maxValue);
        }

        public Boolean Remove(K key)
        {
            if (root == null)
            {
                return false;
            }

            Node<K, V> current = root;
            Node<K, V> parent = null;

            int result = comparer.Compare(current.Key, key);
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
                    result = comparer.Compare(current.Key, key);
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
                    result = comparer.Compare(parent.Key, current.Key);
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
                    result = comparer.Compare(parent.Key, current.Key);
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
                Node<K, V> leftNode = current.Right.Left;
                Node<K, V> leftNodeParent = current.Right;
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
                    result = comparer.Compare(parent.Key, current.Key);
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

        public String PrintTree()
        {
            return PrintTree(TraversalMethod.Inorder, TraversalDirection.Forwards);
        }

        public String PrintTree(TraversalMethod method, TraversalDirection direction)
        {
            String temp = PrintTree(method, direction, Root, "");
            return temp.Remove(temp.Length-2);
        }

        private String PrintTree(TraversalMethod method, TraversalDirection direction, Node<K, V> currentNode, String result)
        {
            if (direction == TraversalDirection.Forwards)
            {
                switch (method)
                {
                    case TraversalMethod.Preorder:
                        if (currentNode != null)
                        {
                            result = result + currentNode.Key + " " + currentNode.Value + ", ";
                            result = PrintTree(method, direction, currentNode.Left, result);
                            result = PrintTree(method, direction, currentNode.Right, result);
                        }
                        break;
                    default:
                    case TraversalMethod.Inorder:
                        if (currentNode != null)
                        {
                            result = PrintTree(method, direction, currentNode.Left, result);
                            result = result + currentNode.Key + " " + currentNode.Value + ", ";
                            result = PrintTree(method, direction, currentNode.Right, result);
                        }
                        break;
                    case TraversalMethod.Postorder:
                        if (currentNode != null)
                        {
                            result = PrintTree(method, direction, currentNode.Left, result);
                            result = PrintTree(method, direction, currentNode.Right, result);
                            result = result + currentNode.Key + " " + currentNode.Value + ", ";
                        }
                        break;
                }
            }
            else
            {
                switch (method)
                {
                    case TraversalMethod.Preorder:
                        if (currentNode != null)
                        {
                            result = result + currentNode.Key + " " + currentNode.Value + ", ";
                            result = PrintTree(method, direction, currentNode.Right, result);
                            result = PrintTree(method, direction, currentNode.Left, result);
                        }
                        break;
                    default:
                    case TraversalMethod.Inorder:
                        if (currentNode != null)
                        {
                            result = PrintTree(method, direction, currentNode.Right, result);
                            result = result + currentNode.Value + " " + currentNode.Key + ", ";
                            result = PrintTree(method, direction, currentNode.Left, result);
                        }
                        break;
                    case TraversalMethod.Postorder:
                        if (currentNode != null)
                        {
                            result = PrintTree(method, direction, currentNode.Right, result);
                            result = PrintTree(method, direction, currentNode.Left, result);
                            result = result + currentNode.Key + " " + currentNode.Value + ", ";
                        }
                        break;
                }
            }
            return result;
        }

        public void CopyTo(K[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex, TraversalMethod.Inorder);
        }

        public void CopyTo(K[] array, int arrayIndex, TraversalMethod method)
        {
            if (Root == null)
            {
                return;
            }
            switch (method)
            {
                case TraversalMethod.Preorder:
                    PreorderTraversal(Root, new List<K>()).CopyTo(array, arrayIndex);
                    break;
                default:
                case TraversalMethod.Inorder:
                    InorderTraversal(Root, new List<K>()).CopyTo(array, arrayIndex);
                    break;
                case TraversalMethod.Postorder:
                    PostorderTraversal(Root, new List<K>()).CopyTo(array, arrayIndex);
                    break;
            }
            Dictionary<string, string> d = new Dictionary<string, string>();
        }

        public IEnumerator<K> GetEnumerator(TraversalMethod method)
        {
            switch (method)
            {
                case TraversalMethod.Preorder:
                    return new BinaryTreeEnumerator<K>(PreorderTraversal(Root, new List<K>()).ToArray());
                default:
                case TraversalMethod.Inorder:
                    return new BinaryTreeEnumerator<K>(InorderTraversal(Root, new List<K>()).ToArray());
                case TraversalMethod.Postorder:
                    return new BinaryTreeEnumerator<K>(PostorderTraversal(Root, new List<K>()).ToArray());
            }
        }

        public IEnumerator<K> GetEnumerator()
        {
            return GetEnumerator(TraversalMethod.Inorder);
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
    public class Node<K, V>
    {
        private K key;
        private V _value;
        private int height;
        private NodeList<K, V> children = null;

        private Node() { } //No default constructor

        public Node(K key, V _value) : this(key, _value, null, null) { }

        public Node(K key, V _value, Node<K, V> left, Node<K, V> right)
        {
            this.Key = key;
            this.Value = _value;
            children = new NodeList<K, V>(2);
            children[0] = left;
            children[1] = right;
            Height = 1;
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

        public V Value
        {
            get
            {
                return _value;
            }
            set
            {
                this._value = value;
            }
        }

        public K Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        private NodeList<K, V> Children
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

        public Node<K, V> Left
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
                    Children = new NodeList<K, V>(2);
                }
                Children[0] = value;
            }
        }

        public Node<K, V> Right
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
                    Children = new NodeList<K, V>(2);
                }
                Children[1] = value;
            }
        }
    }

    public class NodeList<K, V> : Collection<Node<K, V>>
    {
        public NodeList() : base() { }

        public NodeList(int size)
        {
            for (int i = 0; i < size; i++)
            {
                base.Items.Add(default(Node<K, V>));

            }
        }

        public Node<K, V> FindByValue(V value)
        {
            foreach (Node<K, V> node in Items)
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