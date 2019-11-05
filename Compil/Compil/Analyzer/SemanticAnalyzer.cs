using System;
using System.Collections.Generic;
using Compil.Nodes;
using Compil.Symbols;

namespace Compil
{
    public class SemanticAnalyzer
    {
        private int _variablesCount = 0;
        private readonly Stack<Dictionary<string, Symbol>> _stack = new Stack<Dictionary<string, Symbol>>();

        /// <summary>
        /// Getteur global value
        /// </summary>
        public int VariablesCount
        {
            get => _variablesCount;
            set => _variablesCount = value;
        }

        /// <summary>
        /// To detect begin of a block
        /// </summary>
        private void BeginBlock()
        {
            _stack.Push(new Dictionary<string, Symbol>());
        }

        /// <summary>
        /// To detect end of block
        /// </summary>
        private void EndBlock()
        {
            _stack.Pop();
        }

        /// <summary>
        /// Declare a symbol identify a variable
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Symbol Declare(string id)
        {
            var s = new Symbol() { Slot = _variablesCount, Id = id };

            if (_stack.Peek().ContainsKey(id))
            {
                throw new Exception($"Variable '{id}' is already declared in this scope.");
            }
            
            _stack.Peek().Add(id, s);
            return s;
        }

        /// <summary>
        /// Search a symbole on symbol list to found a value (variable)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Symbol Search(string id)
        {
            foreach (var symbolsTable in _stack)
            {
                if (symbolsTable.TryGetValue(id, out var value))
                {
                    return value;
                }
            }
            throw new ArgumentNullException($"Variable '{id}' does not exist.");
        }

        /// <summary>
        /// To check good element of a variable declaration
        /// </summary>
        /// <param name="node"></param>
        public void Analyze(Node node)
        {
            switch (node.Type)
            {
                default:
                    foreach (var child in node.Children)
                    {
                        Analyze(child);
                    }
                    break;
                case NodeType.BLOCK:
                    BeginBlock();
                    foreach (var child in node.Children)
                    {
                        Analyze(child);
                    }
                    EndBlock();
                    break;
                case NodeType.DECLARE:
                    var s1 = Declare(node.Children[0].Value);
                    s1.Type = SymbolType.VARIABLE;
                    s1.Slot = _variablesCount;
                    _variablesCount++; // Increments variable count
                    node.Children[0].Slot = s1.Slot;
                    Analyze(node.Children[1]);
                    break;
                case NodeType.VARIABLE:
                    var s2 = Search(node.Value);
                    if (s2.Type != SymbolType.VARIABLE)
                    {
                        throw new ArgumentNullException("Type is different from Variable");
                    }

                    node.Slot = s2.Slot;
                    break;
            }
        }
    }
}