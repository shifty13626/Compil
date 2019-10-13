﻿using Compil.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compil
{
    /// <summary>
    /// Instance of class Node
    /// </summary>
    public class Node
    {
        #region Properties
        public NodeType Type { get; set; }
        public string Value { get; set; }
        public List<Node> Children { get; set; } = new List<Node>();

        #endregion

        #region Operations

        public void AddChild(Node node)
        {
            Children.Add(node);
        }

        public void AddChildren(List<Node> nodes)
        {
            foreach (var node in nodes)
                Children.Add(node);
        }

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
            if(Type == NodeType.CONSTANT)
                // Print a value in the tree
                Console.WriteLine("VALUE: " + Value);
            else
                // Print a node type only
                Console.WriteLine(Type);

            for (var i = 0; i < Children.Count; i++)
                Children[i].Print(indent, i == Children.Count - 1);    
        }
        
        #endregion

    }
}
