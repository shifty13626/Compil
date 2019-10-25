using System;
using System.Collections.Generic;
using Compil.Nodes;
using Compil.Symbols;

namespace Compil
{
    public class SemanticAnalyzer
    {
        private int _variablesCount = 0;
        private Stack<Dictionary<string, Symbol>> _stack = new Stack<Dictionary<string, Symbol>>();
        
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
            return new Symbol() { Slot = _variablesCount, Id = id };
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
            throw new Exception($"Variable '{id}' does not exist.'");
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
                    var s1 = Declare(node.Value);
                    s1.Type = SymbolType.VARIABLE;
                    s1.Slot = _variablesCount++;
                    break;
                case NodeType.VARIABLE:
                    var s2 = Search(node.Value);
                    if (s2.Type != SymbolType.VARIABLE)
                    {
                        throw new Exception("Type is different from Variable");
                    }

                    node.Slot = s2.Slot;
                    break;
            }
        }
    }
}