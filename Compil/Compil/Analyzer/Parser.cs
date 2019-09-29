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
        private List<string> _listOperator = new List<string> { "+", "-", "*", "^" };
        private List<Operator> _operators = new List<Operator>();

        private Dictionary<TokenType, (NodeType, string)> _keywordsTokenToNodeMatch = new Dictionary<TokenType, (NodeType, string)>()
        {
            {TokenType.DO, (NodeType.DO, "do")},
            {TokenType.IF, (NodeType.IF, "if")},
            {TokenType.FOR, (NodeType.FOR, "for")},
            {TokenType.INT, (NodeType.INT, "int")},
            {TokenType.CASE, (NodeType.CASE, "case")},
            {TokenType.ELSE, (NodeType.ELSE, "else")},
            {TokenType.VOID, (NodeType.VOID, "void")},
            {TokenType.WHILE, (NodeType.WHILE, "while")},
            {TokenType.SWITCH, (NodeType.SWITCH, "switch")},
        };

        private Dictionary<TokenType, (NodeType, string)> _exprTokenToNodeMatch = new Dictionary<TokenType, (NodeType, string)>()
        {
            {TokenType.OR, (NodeType.OR, "|")},
            {TokenType.AND, (NodeType.AND, "&")},
            {TokenType.NOT, (NodeType.NOT, "!")},
            {TokenType.PLUS, (NodeType.PLUS, "+")},
            {TokenType.COMP_EQUAL, (NodeType.COMP_EQUAL, "==")},
            {TokenType.MINUS, (NodeType.MINUS, "-")},
            {TokenType.OP_PLUS, (NodeType.OP_PLUS, "+")},
            {TokenType.OP_MINUS, (NodeType.OP_MINUS, "-")},
            {TokenType.OP_POWER, (NodeType.OP_POWER, "^")},
            {TokenType.OP_DIVIDE, (NodeType.OP_DIVIDE, "/")},
            {TokenType.OP_MODULO, (NodeType.OP_MODULO, "%")},
            {TokenType.OP_MULTIPLY, (NodeType.OP_MULTIPLY, "*")},
            {TokenType.COMP_INFERIOR, (NodeType.COMP_INFERIOR, "<")},
            {TokenType.COMP_DIFFERENT, (NodeType.COMP_DIFFERENT, "!=")},
            {TokenType.COMP_SUPPERIOR, (NodeType.COMP_SUPPERIOR, ">")},
            {TokenType.COMP_INFERIOR_OR_EQUAL, (NodeType.COMP_INFERIOR_OR_EQUAL, "<=")},
            {TokenType.COMP_SUPPERIOR_OR_EQUAL, (NodeType.COMP_SUPPERIOR_OR_EQUAL, ">=")},
        };
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lexicalAnalyser"></param>
        public Parser(LexicalAnalyzer lexicalAnalyser)
        {
            this._lexicalAnalyzer = lexicalAnalyser;
            _operators.Add(new Operator() { Token = });
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
                /// Keywords
                ////////////////////////////////
                if (_keywordsTokenToNodeMatch.ContainsKey(_lexicalAnalyzer.Next().Type))
                {
                    var (nodetype, val) = _keywordsTokenToNodeMatch[_lexicalAnalyzer.Next().Type];
                    node = new Node() {Type = nodetype, Value = val};
                    return node;
                }

                // Constante
                if (_lexicalAnalyzer.Next().Type == TokenType.CONSTANT)
                {
                    node = new Node() {Type = NodeType.CONSTANT, Value = _lexicalAnalyzer.Next().Value.ToString()};
                    //_lexicalAnalyzer.Skip();
                    return node;
                }

                // Parenthese Open
                if (_lexicalAnalyzer.Next().Type == TokenType.PAR_OPEN)
                {
                    node = new Node() { Children = Expression(), Value = "(" };
                    _lexicalAnalyzer.Accept(TokenType.PAR_CLOSE);
                    return node;
                }
                
                ////////////////////////////////
                /// Operators
                ////////////////////////////////
                if (_exprTokenToNodeMatch.ContainsKey(_lexicalAnalyzer.Next().Type))
                {
                    var (nodetype, val) = _exprTokenToNodeMatch[_lexicalAnalyzer.Next().Type];
                    node = new Node() {Type = nodetype, Value = val};
                    node.AddChildren(Expression());
                    return node;
                }
                
                
                if (_lexicalAnalyzer.Next().Type == TokenType.IDENTIFIER)
                {
                    node = new Node() {Type = NodeType.IDENTIFIER, Value = _lexicalAnalyzer.Next().Name};
                    node.AddChildren(Expression());
                    return node;
                }

                throw new NotImplementedException();
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// return node with an expression (xxxx)
        /// </summary>
        /// <param name="pMin"></param>
        /// <returns></returns>
        public Node Expression(int pMin)
        {
            Node A;
            Node A1 = Primary();
            while(true)
            {
                Operator op = searchOp(_lexicalAnalyzer.Next());
                if (op != null || op.Priority < pMin)
                    return A1;
                _lexicalAnalyzer.Skip();
                Node A2 = Expression(op.Priority + op.Association);
                A = new Node() { Type = op.Node.Type };
                A.AddChild(A1);
                A.AddChild(A2);
                A1 = A;
            }
        }


        public Operator searchOp(Token token)
        {
            if (listOperator.Contains(token.Type))
                return new Operator() { Token = token.Type, Node = new Node() { Value = token.Type } };
            else
                return null;
        }

    }
}