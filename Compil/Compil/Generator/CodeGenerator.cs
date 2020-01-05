using System;
using System.Collections.Generic;
using Compil.Utils;
using Compil.Nodes;
using System.Linq;
using Compil.Exceptions;

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
                _fileWriter.WriteCommand($"push {node.Value}", false);
            }

            // Power
            if (node.Type == NodeType.OP_POWER) {
                var callPowNode = new Node() {Type = NodeType.CALL, Value = "pow"};
                callPowNode.AddChild(node.Children[0]);
                callPowNode.AddChild(node.Children[1]);
                node = callPowNode;
                GenerateCode(node);
            }
            
            // Operations
            if (_operatorsToCode.ContainsKey(node.Type))
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand(_operatorsToCode[node.Type], false);
            }

            // Unary operations
            switch (node.Type)
            {
                // Unary operations
                case NodeType.MINUS:
                    // We do '0 - n'
                    _fileWriter.WriteCommand("push 0", false);
                    GenerateCode(node.Children[0]);
                    _fileWriter.WriteCommand("sub", false);
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
                _fileWriter.WriteCommand($"get {node.Slot}", false);
            }

            if (node.Type == NodeType.ASSIGN)
            {
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("dup", false);
                _fileWriter.WriteCommand($"set {node.Children[0].Slot}", false);
            }

            // Call function
            if(node.Type == NodeType.CALL)
            {
                _fileWriter.WriteCommand("prep " + node.Value, false);
                foreach (var expression in node.Children)
                    GenerateCode(expression);
                _fileWriter.WriteCommand("call " + node.Children.Count, false);
            }

            // Function
            if (node.Type == NodeType.FUNCTION)
            {
                _fileWriter.DeclareFunction(node.Value);
                _fileWriter.WriteCommand("resn " + (node.VariablesCount - (node.Children[0].Children.Count - 1)), false);
                GenerateCode(node.Children[0]);
                _fileWriter.WriteCommand("push 0", false);
                _fileWriter.WriteCommand("ret", false);
            }

            // Return
            if(node.Type == NodeType.RETURN)
            {
                GenerateCode(node.Children[0]);
                _fileWriter.WriteCommand("ret", false);
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

            // Send
            if (node.Type == NodeType.SEND) {
                GenerateCode(node.Children[0]);
                _fileWriter.WriteCommand("dup");
                _fileWriter.WriteCommand("send");
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
                _stackLoop.Push(countLoop);
                countLoop += 2;
                
                _fileWriter.WriteCommand($".loop{_stackLoop.Peek()}", false);
                GenerateCode(node.Children[0]);
                    
                // end labels
                _fileWriter.WriteCommand($"jump loop{_stackLoop.Peek()}", false);
                _fileWriter.WriteCommand($".loop{_stackLoop.Peek() + 1}", false);
                _stackLoop.Pop();
            }

            if (node.Type == NodeType.BREAK)
            {
                if (!_stackLoop.Any())
                    throw new SyntaxErrorException ("Break out of loop");
                _fileWriter.WriteCommand($"jump loop{(_stackLoop.Peek() + 1)}", false);
            }

            if (node.Type == NodeType.CONTINUE)
            {
                if (!_stackLoop.Any())
                    throw new SyntaxErrorException ("Break out of loop");
                _fileWriter.WriteCommand($"jump loop{(_stackLoop.Peek())}", false);
            }

            
            if (node.Type == NodeType.DECLARE) 
            {
                if(node.Children.Count >= 1)
                    GenerateCode(node.Children[0]);
            }

            // Comparaison
            // ==
            if (node.Type == NodeType.COMP_EQUAL)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmpeq", false);
            }

            // !=
            if (node.Type == NodeType.COMP_DIFFERENT)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmpne", false);
            }

            // <
            if (node.Type == NodeType.COMP_INFERIOR)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmplt", false);
            }

            // <=
            if (node.Type == NodeType.COMP_INFERIOR_OR_EQUAL)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmple", false);
            }

            // > 
            if (node.Type == NodeType.COMP_SUPPERIOR)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmpgt", false);
            }

            // >=
            if (node.Type == NodeType.COMP_SUPPERIOR_OR_EQUAL)
            {
                GenerateCode(node.Children[0]);
                GenerateCode(node.Children[1]);
                _fileWriter.WriteCommand("cmpge", false);
            }
        }
    }
}