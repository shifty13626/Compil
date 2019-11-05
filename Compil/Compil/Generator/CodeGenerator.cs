using System;
using System.Collections.Generic;
using Compil.Utils;
using Compil.Nodes;

namespace Compil.Generator
{
    public class CodeGenerator
    {

        private readonly FileWriter _fileWriter;
        private int countIf;

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
        /// Constructor of class
        /// </summary>
        /// <param name="fileWriter"></param>
        public CodeGenerator(SemanticAnalyzer semanticAnalyzer, FileWriter fileWriter)
        {
            _fileWriter = fileWriter;
            countIf = 0;
            _fileWriter.WriteCommand("resn " + semanticAnalyzer.VariablesCount);
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
                _fileWriter.WriteCommand("push " +node.Value, true);
            }

            // Operations
            if (_operatorsToCode.ContainsKey(node.Type))
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand(_operatorsToCode[node.Type], true);
            }

            // Unary operations
            switch (node.Type)
            {
                // Unary operations
                case NodeType.MINUS:
                    _fileWriter.WriteCommand("push 0", true);
                    GenerateCode(node.Children[0]);
                    _fileWriter.WriteCommand("sub", true);
                    break;
                case NodeType.PLUS:
                    _fileWriter.WriteCommand("push 0", true);
                    GenerateCode(node.Children[0]);
                    _fileWriter.WriteCommand("add", true);
                    break;
                case NodeType.NOT:
                    GenerateCode(node.Children[0]);
                    _fileWriter.WriteCommand("not", false);
                    break;
            }

            // Variables
            if (node.Type == NodeType.VARIABLE)
            {
                _fileWriter.WriteCommand($"get {node.Slot}", true);
            }

            if (node.Type == NodeType.AFFECT)
            {
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("dup", false);
                _fileWriter.WriteCommand($"set {node.Children[0].Slot}", false);
            }

            // Block
            if (node.Type == NodeType.BLOCK)
            {
                foreach (var child in node.Children)
                {
                    GenerateCode(child);
                }
            }

            // Expressions
            if (node.Type == NodeType.EXPRESSION)
            {
                GenerateCode(node.Children[0]);
                _fileWriter.WriteCommand("drop");
            }

            // Conditions
            if (node.Type == NodeType.CONDITION)
            {
                var nodeTest = node.Children[0];
                var nodeCode = node.Children[1];

                GenerateCode(nodeTest);

                _fileWriter.WriteCommand("jumpf endIf" +countIf, false);

                foreach (var child in nodeCode.Children)
                {
                    GenerateCode(child);
                }

                _fileWriter.WriteCommand(".endIf" +countIf, false);
                countIf++;
            }

            if(node.Type == NodeType.ELSE)
            {
                var nodeCode = node.Children[0];

                foreach(var child in nodeCode.Children)
                {
                    GenerateCode(child);
                }
            }

            if (node.Type == NodeType.LOOP)
            {
                // condition label
                _fileWriter.WriteCommand(".loop", false);

                var nodeTest = node.Children[0];
                var nodeCode = node.Children[1];

                GenerateCode(nodeTest);

                _fileWriter.WriteCommand("jumpf endWhile", false);

                foreach (var child in nodeCode.Children)
                {
                    GenerateCode(child);
                }

                // end labels
                _fileWriter.WriteCommand("jump loop", false);
                _fileWriter.WriteCommand(".endloop", false);
            }
            

            if(node.Type == NodeType.WHILE)
            {
                // condition label
                _fileWriter.WriteCommand(".conditionWhile", false);

                // get child nodes
                var nodeTest = node.Children[0];
                var nodeCode = node.Children[1];

                GenerateCode(nodeTest);

                _fileWriter.WriteCommand("jumpf endWhile", false);

                foreach (var child in nodeCode.Children)
                {
                    GenerateCode(child);
                }

                // end labels
                _fileWriter.WriteCommand("jump conditionWhile", false);
                _fileWriter.WriteCommand(".endWhile", false);
            }

            if(node.Type == NodeType.FOR)
            {
                // condition label
                _fileWriter.WriteCommand(".conditionFor", false);

                // get child nodes
                var nodeTest = node.Children[0];
                var nodeCode = node.Children[1];

                GenerateCode(nodeTest);

                _fileWriter.WriteCommand("jumpf endFor", false);

                foreach (var child in nodeCode.Children)
                {
                    GenerateCode(child);
                }

                // end labels
                _fileWriter.WriteCommand("jump conditionFor", false);
                _fileWriter.WriteCommand(".endFor", false);
            }


            if (node.Type == NodeType.DECLARE) 
            {
                //_fileWriter.WriteCommand($"resn {node.Children[0].Slot}");
                GenerateCode(node.Children[1]);
            }

            // Comparaison
            // ==
            if (node.Type == NodeType.COMP_EQUAL)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmpeq", true);
            }

            // !=
            if (node.Type == NodeType.COMP_DIFFERENT)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmpne", true);
            }

            // <
            if (node.Type == NodeType.COMP_INFERIOR)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmplt", true);
            }

            // <=
            if (node.Type == NodeType.COMP_INFERIOR_OR_EQUAL)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmple", true);
            }

            // > 
            if (node.Type == NodeType.COMP_SUPPERIOR)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmpgt", true);
            }

            // >=
            if (node.Type == NodeType.COMP_SUPPERIOR_OR_EQUAL)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmpge", true);
            }
        }
    }
}