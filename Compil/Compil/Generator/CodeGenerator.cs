using System;
using System.Collections.Generic;
using Compil.Utils;
using Compil.Nodes;

namespace Compil.Generator
{
    public class CodeGenerator
    {

        private static FileWriter _fileWriter;
        private static bool _debug;

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
        

        public CodeGenerator(FileWriter fileWriter, bool debug)
        {
            _fileWriter = fileWriter;
            _debug = debug;
        }


        /// <summary>
        /// Method to generate code from expression tree.
        /// </summary>
        /// <param name="node"></param>
        public void GenerateCode(Node node)
        {
            // Constants
            if (node.Type == NodeType.CONSTANT)
            {
                _fileWriter.WriteCommand("push " +node.Value, _debug);
            }

            // Operations
            if (_operatorsToCode.ContainsKey(node.Type))
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand(_operatorsToCode[node.Type], _debug);
            }

            // Unary operations
            switch (node.Type)
            {
                // Unary operations
                case NodeType.MINUS:
                    _fileWriter.WriteCommand("push 0", _debug);
                    GenerateCode(node.Children[0]);
                    _fileWriter.WriteCommand("sub", _debug);
                    break;
                case NodeType.PLUS:
                    _fileWriter.WriteCommand("push 0", _debug);
                    GenerateCode(node.Children[0]);
                    _fileWriter.WriteCommand("add", _debug);
                    break;
            }

            if (node.Type == NodeType.VARIABLE)
            {
                Console.WriteLine("get 0");
            }

            if (node.Type == NodeType.AFFECT)
            {
                GenerateCode(node.Children[1]);
                Console.WriteLine("dup");
                Console.WriteLine("set 0");
            }

            if (node.Type == NodeType.BLOCK)
            {
                foreach (var child in node.Children)
                {
                    GenerateCode(child);
                }
            }

            if (node.Type == NodeType.EXPRESSION)
            {
                GenerateCode(node.Children[0]);
                Console.WriteLine("drop");
            }

            if (node.Type == NodeType.CONDITION)
            {
                GenerateCode(node.Children[0]);
            }

            if (node.Type == NodeType.COMP_EQUAL)
            {
                // TODO: gérer les branchements
            }
        }
    }
}