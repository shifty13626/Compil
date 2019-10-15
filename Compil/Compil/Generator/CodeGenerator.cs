using System;
using System.Collections.Generic;
using Compil.Utils;

namespace Compil.Generator
{
    public class CodeGenerator
    {
        private readonly Dictionary<NodeType, string> _operatorsToCode = new Dictionary<NodeType, string>()
        {
            { NodeType.OP_PLUS, "add" },
            { NodeType.OP_MINUS, "sub" },
            { NodeType.OP_MULTIPLY, "mul" },
            { NodeType.OP_DIVIDE, "div" },
            { NodeType.OP_MODULO, "mod" },
            { NodeType.AND, "and" },
            { NodeType.OR, "or" }
        };
        
        /// <summary>
        /// Method to generate code from expression tree.
        /// </summary>
        /// <param name="node"></param>
        public void GenerateCode(Node node)
        {
            // Constants
            if (node.Type == NodeType.CONSTANT)
            {
                Console.WriteLine($"push {node.Value}");
            }

            // Operations
            if (_operatorsToCode.ContainsKey(node.Type))
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                Console.WriteLine(_operatorsToCode[node.Type]);
            }

            // Unary operations
            switch (node.Type)
            {
                // Unary operations
                case NodeType.MINUS:
                    Console.WriteLine("push 0");
                    GenerateCode(node.Children[0]);
                    Console.WriteLine("sub");
                    break;
                case NodeType.PLUS:
                    Console.WriteLine("push 0");
                    GenerateCode(node.Children[0]);
                    Console.WriteLine("add");
                    break;
            }
        }
    }
}