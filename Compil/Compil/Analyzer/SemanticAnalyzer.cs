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
        
        private void BeginBlock()
        {
            _stack.Push(new Dictionary<string, Symbol>());
        }

        private void EndBlock()
        {
            _stack.Pop();
        }

        private Symbol Declare(string id)
        {
            var s = new Symbol() { Slot = _variablesCount, Id = id };
            _stack.Peek().Add(id, s);
            return s;
        }

        private Symbol Search(string id)
        {
            foreach (var symbolsTable in _stack)
            {
                if (symbolsTable.TryGetValue(id, out var value))
                {
                    return value;
                }
            }
            throw new ArgumentNullException($"Variable '{id}' does not exist.'");
        }

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
                    s1.Slot = _variablesCount++;
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