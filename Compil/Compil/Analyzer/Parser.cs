using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compil.Utils;
using Compil.Analyzer;

namespace Compil.Analyzer
{
    /// <summary>
    /// Instance of class SyntacticAnalyzer
    /// </summary>
    class Parser
    {
        private LexicalAnalyzer _lexicalAnalyzer;

        public Parser(LexicalAnalyzer lexicalAnalyser)
        {
            this._lexicalAnalyzer = lexicalAnalyser;
        }

        /// <summary>
        /// Build tree of expresion
        /// </summary>
        public Node Primary()
        {
            try
            {
                Node node;
                ////////////////////////////////
                /// Instruction
                ////////////////////////////////
                // if
                if (_lexicalAnalyzer.Next().Type == TokenType.IF)
                {
                    node = new Node() { Type = NodeType.NODE_IF, Value = "if" };
                    return node;
                }
                // else
                if (_lexicalAnalyzer.Next().Type == TokenType.ELSE)
                {
                    node = new Node() { Type = NodeType.NODE_ELSE, Value = "else" };
                    return node;
                }
                // for
                if (_lexicalAnalyzer.Next().Type == TokenType.FOR)
                {
                    node = new Node() { Type = NodeType.NODE_FOR, Value = "for" };
                    return node;
                }
                // while
                if (_lexicalAnalyzer.Next().Type == TokenType.WHILE)
                {
                    node = new Node() { Type = NodeType.NODE_WHILE, Value = "while" };
                    return node;
                }
                // do
                if (_lexicalAnalyzer.Next().Type == TokenType.DO)
                {
                    node = new Node() { Type = NodeType.NODE_DO, Value = "do" };
                    return node;
                }
                // switch
                if (_lexicalAnalyzer.Next().Type == TokenType.SWITCH)
                {
                    node = new Node() { Type = NodeType.NODE_SWITCH, Value = "switch" };
                    return node;
                }
                // case
                if (_lexicalAnalyzer.Next().Type == TokenType.CASE)
                {
                    node = new Node() { Type = NodeType.NODE_CASE, Value = "case" };
                    return node;
                }
                // int
                if (_lexicalAnalyzer.Next().Type == TokenType.INT)
                {
                    node = new Node() { Type = NodeType.NODE_INT, Value = "int" };
                    return node;
                }
                // void
                if (_lexicalAnalyzer.Next().Type == TokenType.VOID)
                {
                    node = new Node() { Type = NodeType.NODE_VOID, Value = "void" };
                    return node;
                }
                


                // Constante
                if (_lexicalAnalyzer.Next().Type == TokenType.TOK_CONST)
                {
                    node = new Node() { Type = NodeType.NODE_CONST, Value = _lexicalAnalyzer.Next().Value.ToString() };
                    //_lexicalAnalyzer.Skip();
                    return node;
                }
                // Parenthese Open
                if (_lexicalAnalyzer.Next().Type == TokenType.PAR_OPEN)
                {
                    _lexicalAnalyzer.Next();
                    node = new Node() { Children = Expression(), Value = "(" };
                    _lexicalAnalyzer.Accept(TokenType.PAR_CLOSE);
                    return node;
                }


                ////////////////////////////////
                /// Binarie Operator Operation
                ////////////////////////////////
                // +
                if(_lexicalAnalyzer.Next().Type == TokenType.OP_PLUS)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_PLUS, Value = "+" };
                    return node;
                }
                // -
                if (_lexicalAnalyzer.Next().Type == TokenType.OP_MINUS)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_MINUS, Value = "-" };
                    return node;
                }
                // *
                if (_lexicalAnalyzer.Next().Type == TokenType.OP_MULTIPLY)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_MULTIPLY, Value = "*" };
                    return node;
                }
                // /
                if (_lexicalAnalyzer.Next().Type == TokenType.OP_DIVIDE)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_DIVIDE, Value = "/" };
                    return node;
                }
                // %
                if (_lexicalAnalyzer.Next().Type == TokenType.OP_MODULO)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_MODULO, Value = "%" };
                    return node;
                }
                // ^
                if (_lexicalAnalyzer.Next().Type == TokenType.OP_POWER)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_OP_POWER, Value = "^" };
                    return node;
                }


                ////////////////////////////////
                /// Binarie Operator Comparator
                ////////////////////////////////
                // ==
                if (_lexicalAnalyzer.Next().Type == TokenType.COMP_EQUAL)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_COMP_EQUAL, Value = "==" };
                    node.AddChildren(Expression());
                    return node;
                }
                // !=
                if (_lexicalAnalyzer.Next().Type == TokenType.COMP_DIFFERENT)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_COMP_NOT, Value = "!=" };
                    node.AddChildren(Expression());
                    return node;
                }
                // <
                if (_lexicalAnalyzer.Next().Type == TokenType.COMP_INFERIOR)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_COMP_INFERIOR, Value = "<" };
                    node.AddChildren(Expression());
                    return node;
                }
                // >
                if (_lexicalAnalyzer.Next().Type == TokenType.COMP_SUPPERIOR)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_COMP_SUPPERIOR, Value = ">" };
                    node.AddChildren(Expression());
                    return node;
                }
                // <=
                if (_lexicalAnalyzer.Next().Type == TokenType.COMP_INFERIOR_OR_EQUAL)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_COMP_INFERIOR_OR_EQUAL, Value = "<=" };
                    node.AddChildren(Expression());
                    return node;
                }
                // >=
                if (_lexicalAnalyzer.Next().Type == TokenType.COMP_SUPPERIOR_OR_EQUAL)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_COMP_SUPPERIOR_OR_EQUAL, Value = ">=" };
                    node.AddChildren(Expression());
                    return node;
                }
                // &&
                if (_lexicalAnalyzer.Next().Type == TokenType.AND)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_AND, Value = "&&" };
                    node.AddChildren(Expression());
                    return node;
                }
                // ||
                if (_lexicalAnalyzer.Next().Type == TokenType.OR)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_OR, Value = "||" };
                    node.AddChildren(Expression());
                    return node;
                }


                ////////////////////////////////
                /// Unaire Operator
                ////////////////////////////////
                // Minus (negatif number)
                if (_lexicalAnalyzer.Next().Type == TokenType.MINUS)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_MINUS, Value = "-" };
                    node.AddChildren(Expression());
                    return node;
                }
                // Plus
                if (_lexicalAnalyzer.Next().Type == TokenType.PLUS)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_PLUS, Value = "+" };
                    node.AddChildren(Expression());
                    return node;
                }
                // !
                if (_lexicalAnalyzer.Next().Type == TokenType.NOT)
                {
                    //_lexicalAnalyzer.Skip();
                    node = new Node() { Type = NodeType.NODE_NOT, Value = "!" };
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

        public List<Node> Expression()
        {
            return new List<Node>();
        }
    }
}
