using System;
using System.Collections.Generic;
using Compil.Utils;
using Compil.Nodes;

namespace Compil.Generator
{
    public class CodeGenerator
    {

        private readonly FileWriter _fileWriter;
        private int countLoop;
        private int countLabel;
        // stack to know who loop is it on the recursif method
        private readonly Stack<int> _stackLoop = new Stack<int>();

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
            countLoop = 0;
            countLabel = 0;
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
                _fileWriter.WriteCommand($"push {node.Value}", true);
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
                    // We do '0 - n'
                    _fileWriter.WriteCommand("push 0", true);
                    GenerateCode(node.Children[0]);
                    _fileWriter.WriteCommand("sub", true);
                    break;
                case NodeType.PLUS:
                    // Little optimisation: we do nothing because, for example, +5 = 5, the '+' disappear.
                    GenerateCode(node.Children[0]);
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

            if (node.Type == NodeType.ASSIGN)
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
                var cpt1 = countLabel++;
                var cpt2 = countLabel++;
                GenerateCode(node.Children[0]);
                _fileWriter.WriteCommand($"jumpf l{cpt1}", false);
                GenerateCode(node.Children[1]);
                if (node.Children.Count > 2) // if there is else
                {
                    _fileWriter.WriteCommand($"jump l{cpt2}", false);
                    _fileWriter.WriteCommand($".l{cpt1}", false);
                    GenerateCode(node.Children[2]);
                    _fileWriter.WriteCommand($".l{cpt2}", false);
                }
                else
                {
                    _fileWriter.WriteCommand($".l{cpt1}", false);
                }
                
            }

            if (node.Type == NodeType.LOOP)
            {
                var l = countLoop++;
                // condition label
                _fileWriter.WriteCommand($".loop{l}", false);
                GenerateCode(node.Children[0]);
                    
                // end labels
                _fileWriter.WriteCommand($"jump loop{l}", false);
                _fileWriter.WriteCommand($".endLoop{_stackLoop.Pop()}", false);
            }

            if (node.Type == NodeType.BREAK)
            {
                _stackLoop.Push(countLoop++);
                _fileWriter.WriteCommand($"jump endLoop{(countLoop - 1)}", false);
            }

            if (node.Type == NodeType.DECLARE) 
            {
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