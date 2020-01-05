using Compil.Utils;
using Compil.Nodes;
using System;
using System.Collections.Generic;

namespace Compil
{
    /// <summary>
    /// Instance of class Node
    /// </summary>
    public class Node
    {
        #region Properties

        /// <summary>
        /// Type of a Node
        /// </summary>
        public NodeType Type { get; set; }
        /// <summary>
        /// Value of element prensent by a node
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// List of node child for a node
        /// </summary>
        public List<Node> Children { get; set; } = new List<Node>();
        /// <summary>
        /// Index of node on stack machine
        /// </summary>
        public int Slot { get; set; }
        
        public int VariablesCount { get; set; }
        
        #endregion

        #region Operations

        /// <summary>
        /// Add child to the tree.
        /// </summary>
        /// <param name="node"></param>
        public void AddChild(Node node)
        {
            Children.Add(node);
        }

        /// <summary>
        /// Add children to the tree.
        /// </summary>
        /// <param name="nodes"></param>
        public void AddChildren(List<Node> nodes)
        {
            foreach (var node in nodes)
                Children.Add(node);
        }

        /// <summary>
        /// Method to print a tree.
        /// </summary>
        /// <param name="indent"></param>
        /// <param name="last"></param>
        public void Print(string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }
            if (Type == NodeType.CONSTANT)
                // Print a value in the tree
                Console.WriteLine($"VALUE: {Value}");
            else if(Type == NodeType.VARIABLE)
                Console.WriteLine($"VARIABLE: name={Value}");
            else if(Type == NodeType.FUNCTION)
                Console.WriteLine($"FUNCTION DEFINITION: {Value}");
            else if(Type == NodeType.CALL)
                Console.WriteLine($"FUNCTION CALL: {Value}");
            else if(Type == NodeType.DECLARE)
                Console.WriteLine($"DECLARE: name={Value}");
            else
                // Print a node type only
                Console.WriteLine(Type);

            for (var i = 0; i < Children.Count; i++)
                Children[i].Print(indent, i == Children.Count - 1);
        }

        #endregion

    }
}

