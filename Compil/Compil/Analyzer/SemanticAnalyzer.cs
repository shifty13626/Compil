using System;
using System.Collections.Generic;
using Compil.Exceptions;
using Compil.Nodes;
using Compil.Symbols;

namespace Compil {
    public class SemanticAnalyzer {
        private readonly Stack<Dictionary<string, Symbol>> _stack = new Stack<Dictionary<string, Symbol>>();
        
        public SemanticAnalyzer(SyntaxAnalyzer syntaxAnalyzer) {
            this.SyntaxAnalyzer = syntaxAnalyzer;
        }

        /// <summary>
        /// Getteur global value
        /// </summary>
        public int VariablesCount { get; set; } = 0;
        
        public SyntaxAnalyzer SyntaxAnalyzer { get; }

        /// <summary>
        /// When entering a new scope, this method is called to create the symbols table of the current scope.
        /// </summary>
        private void BeginBlock() {
            _stack.Push(new Dictionary<string, Symbol>());
        }

        /// <summary>
        /// When exiting a scope, this method is called to remove the symbols table from the stack.
        /// </summary>
        private void EndBlock() {
            _stack.Pop();
        }

        /// <summary>
        /// Declare a symbol in the symbols table of the current scope.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Symbol Declare(string id) {
            var s = new Symbol() {Slot = VariablesCount, Id = id};

            if (_stack.Peek().ContainsKey(id)) {
                throw new SemanticErrorException(
                    $"Variable '{id}' is already declared in this scope. Error at line {SyntaxAnalyzer.LexicalAnalyzer.Next().Line}.");
            }

            _stack.Peek().Add(id, s);
            return s;
        }

        /// <summary>
        /// Search for a symbol in the symbols table.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Symbol Search(string id) {
            foreach (var symbolsTable in _stack) {
                if (symbolsTable.TryGetValue(id, out var value)) {
                    return value;
                }
            }

            throw new SemanticErrorException(
                $"Variable '{id}' does not exist. Error at line {SyntaxAnalyzer.LexicalAnalyzer.Next().Line}.");
        }

        /// <summary>
        /// Perform the semantic analysis of the syntax tree.
        /// </summary>
        /// <param name="node"></param>
        public void Analyze(Node node) {
            switch (node.Type) {
                default:
                    foreach (var child in node.Children) {
                        Analyze(child);
                    }

                    break;
                case NodeType.BLOCK:
                    BeginBlock();
                    foreach (var child in node.Children) {
                        Analyze(child);
                    }

                    EndBlock();
                    break;
                case NodeType.DECLARE:
                    var s1 = Declare(node.Value);
                    s1.Type = SymbolType.VARIABLE;
                    s1.Slot = VariablesCount;
                    VariablesCount++; // Increments variable count
                    node.Slot = s1.Slot;
                    Analyze(node.Children[0]);
                    break;
                case NodeType.VARIABLE:
                    var s2 = Search(node.Value);
                    if (s2.Type != SymbolType.VARIABLE) {
                        throw new SemanticErrorException(
                            $"Type is different from Variable. Error at line {SyntaxAnalyzer.LexicalAnalyzer.Next().Line}.");
                    }

                    node.Slot = s2.Slot;
                    break;
            }
        }
    }
}