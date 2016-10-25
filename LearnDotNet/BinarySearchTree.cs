using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnDotNet
{
    /// <summary>
    /// Represents a Binary Search Tree. Provides methods to populate the BST, finding height
    /// </summary>
    public class BinarySearchTree
    {
        public Node RootNode { get; set; }

        public BinarySearchTree(int rootValue)
        {
            RootNode = new Node(rootValue);
        }

        /// <summary>
        /// Create nodes from array of values and add them to BST
        /// </summary>
        /// <param name="nodeValues"></param>
        public void PopulateBinarySearchTree(int[] nodeValues)
        {
            foreach (var value in nodeValues)
            {
                InsertNode(RootNode, value);
            }
        }

        /// <summary>
        /// Add node to the BST
        /// </summary>
        /// <param name="node"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Node InsertNode(Node node, int data)
        {
            if (node == null)
            {
                node = new Node(data);
                return node;
            }
            if (data <= node.Data)
            {
                //Fill up left node with data
                node.LeftNode = InsertNode(node.LeftNode, data);
            }
            else if (data >= node.Data)
            {
                //Fill up right node with data
                node.RightNode = InsertNode(node.RightNode, data);
            }
            return node;
        }

        /// <summary>
        /// Height of a Binary tree =  Max(Left subtree, Right subtree) + 1
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int GetHeight(Node root)
        {
            if (root == null) return -1;
            int left = GetHeight(root.LeftNode);
            int right = GetHeight(root.RightNode);
            return Math.Max(left, right) + 1;
        }
    }

    public class Node
    {
        public int Data { get; set; }
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }

        public Node(int data)
        {
            Data = data;
        }
    }
}
