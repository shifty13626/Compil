using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compil.Utils;

namespace Compil.Analyzer
{
    /// <summary>
    /// Instance of class SyntacticAnalyzer
    /// </summary>
    class Parser
    {
        private LexicalAnalyser _lexicalAnalyser;

        public Parser(LexicalAnalyser lexicalAnalyser)
        {
            this._lexicalAnalyser = lexicalAnalyser;
        }

        /// <summary>
        /// Build tree of expresion
        /// </summary>
        public Node Primary()
        {
            try
            {
                Node node;
                // Constante
                if (_lexicalAnalyser.Next().Type == TokenType.TOK_CONST)
                {
                    node = new Node() { Type = NodeType.NODE_CONST, Value = _lexicalAnalyser.Next().Value };
                    _lexicalAnalyser.Skip();
                    return node;
                }
                // Parenthese Open
                if (_lexicalAnalyser.Next().Type == TokenType.PAR_OPEN)
                {
                    _lexicalAnalyser.Next();
                    node = Expression();
                    _lexicalAnalyser.Accept(TokenType.PAR_CLOSE);
                    return node;
                }


                ////////////////////////////////
                /// Binarie Operator Operation
                ////////////////////////////////
                // +
                if(_lexicalAnalyser.Next().Type == TokenType.OP_PLUS)
                {
                    _lexicalAnalyser.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_PLUS };
                    return node;
                }
                // -
                if (_lexicalAnalyser.Next().Type == TokenType.OP_MINUS)
                {
                    _lexicalAnalyser.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_MINUS };
                    return node;
                }
                // *
                if (_lexicalAnalyser.Next().Type == TokenType.OP_MULTIPLY)
                {
                    _lexicalAnalyser.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_MULTIPLY };
                    return node;
                }
                // /
                if (_lexicalAnalyser.Next().Type == TokenType.OP_DIVIDE)
                {
                    _lexicalAnalyser.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_DIVIDE };
                    return node;
                }
                // %
                if (_lexicalAnalyser.Next().Type == TokenType.OP_MODULO)
                {
                    _lexicalAnalyser.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_MODULO };
                    return node;
                }
                // ^
                if (_lexicalAnalyser.Next().Type == TokenType.OP_POWER)
                {
                    _lexicalAnalyser.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_POWER };
                    return node;
                }


                ////////////////////////////////
                /// Binarie Operator Comparator
                ////////////////////////////////
                // ==
                if (_lexicalAnalyser.Next().Type == TokenType.COMP_EQUAL)
                {
                    _lexicalAnalyser.Skip();
                    _lexicalAnalyser.Skip();
                    node = new Node() { Type = NodeType.NODE_MINUS };
                    node.AddChildren(Expression());
                    return node;
                }



                ////////////////////////////////
                /// Unaire Operator
                ////////////////////////////////
                // Minus (negatif number)
                if (_lexicalAnalyser.Next().Type == TokenType.MINUS)
                {
                    _lexicalAnalyser.Skip();
                    node = new Node() { Type = NodeType.NODE_MINUS };
                    node.AddChildren(Expression());
                    return node;
                }
                // Plus
                if (_lexicalAnalyser.Next().Type == TokenType.PLUS)
                {
                    _lexicalAnalyser.Skip();
                    node = new Node() { Type = NodeType.NODE_PLUS };
                    node.AddChildren(Expression());
                    return node;
                }
                // Plus
                if (_lexicalAnalyser.Next().Type == TokenType.NOT)
                {
                    _lexicalAnalyser.Skip();
                    node = new Node() { Type = NodeType.NODE_NOT };
                    node.AddChildren(Expression());
                    return node;
                }
                throw new NotImplementedException();
            }
            catch(NotImplementedException e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }                
        }

        public Node Expression()
        {
            return new Node();
        }
    }
}
