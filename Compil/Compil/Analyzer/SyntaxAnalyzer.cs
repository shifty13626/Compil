using Compil.Utils;
using Compil.Tokens;
using Compil.Nodes;
using System;
using System.Collections.Generic;

namespace Compil
{
    /// <summary>
    /// Instance of class SyntacticAnalyzer
    /// </summary>
    public class SyntaxAnalyzer
    {
        private readonly LexicalAnalyzer _lexicalAnalyzer;

        private readonly Dictionary<TokenType, (NodeType, string, int, int)> _exprTokenToNodeMatch = new Dictionary<TokenType, (NodeType, string, int, int)>()
        {
            {TokenType.OR, (NodeType.OR, "|", 2, 1)},
            {TokenType.AND, (NodeType.AND, "&", 3, 1)},
            {TokenType.NOT, (NodeType.NOT, "!", 3, 1)},
            {TokenType.COMP_EQUAL, (NodeType.COMP_EQUAL, "==", 4, 1)},
            {TokenType.PLUS, (NodeType.OP_PLUS, "+", 5, 1)},
            {TokenType.MINUS, (NodeType.OP_MINUS, "-", 5, 1)},
            {TokenType.POWER, (NodeType.OP_POWER, "^", 7, 0)},
            {TokenType.DIVIDE, (NodeType.OP_DIVIDE, "/", 6, 1)},
            {TokenType.MODULO, (NodeType.OP_MODULO, "%", 6, 1)},
            {TokenType.MULTIPLY, (NodeType.OP_MULTIPLY, "*", 6, 1)},
            {TokenType.COMP_INFERIOR, (NodeType.COMP_INFERIOR, "<", 4, 1)},
            {TokenType.COMP_DIFFERENT, (NodeType.COMP_DIFFERENT, "!=", 4, 1)},
            {TokenType.COMP_SUPPERIOR, (NodeType.COMP_SUPPERIOR, ">", 4, 1)},
            {TokenType.COMP_INFERIOR_OR_EQUAL, (NodeType.COMP_INFERIOR_OR_EQUAL, "<=", 4, 1)},
            {TokenType.COMP_SUPPERIOR_OR_EQUAL, (NodeType.COMP_SUPPERIOR_OR_EQUAL, ">=", 4, 1)},
            {TokenType.EQUAL, (NodeType.AFFECT, "=", 1, 0)}
        };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lexicalAnalyser"></param>
        public SyntaxAnalyzer(LexicalAnalyzer lexicalAnalyser)
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

                // Constante
                if (_lexicalAnalyzer.Next().Type == TokenType.CONSTANT)
                {
                    node = new Node() { Type = NodeType.CONSTANT, Value = _lexicalAnalyzer.Next().Value.ToString() };
                    _lexicalAnalyzer.Skip();
                    return node;
                }

                // Parenthese Open
                if (_lexicalAnalyzer.Next().Type == TokenType.PAR_OPEN)
                {
                    _lexicalAnalyzer.Skip();
                    node = Expression(0);
                    _lexicalAnalyzer.Accept(TokenType.PAR_CLOSE);
                    return node;
                }

                // Unary operators
                if (_lexicalAnalyzer.Next().Type == TokenType.MINUS ||
                    _lexicalAnalyzer.Next().Type == TokenType.PLUS ||
                    _lexicalAnalyzer.Next().Type == TokenType.NOT)
                {
                    var (nodeType, val, _, _) = _exprTokenToNodeMatch[_lexicalAnalyzer.Next().Type];
                    node = new Node() { Type = nodeType, Value = val };
                    _lexicalAnalyzer.Skip();
                    node.AddChild(Expression(7));
                    return node;
                }

                // Identifiers handling
                if (_lexicalAnalyzer.Next().Type == TokenType.IDENTIFIER)
                {
                    node = new Node() { Type = NodeType.VARIABLE, Value = _lexicalAnalyzer.Next().Name.ToString() };
                    _lexicalAnalyzer.Skip();
                    return node;
                }

                // Primary not found
                throw new ArgumentNullException("Primary expected.");
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine("Feature not implemented.");
                Console.WriteLine(e.StackTrace);
                return null;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// return node with an expression (xxxx)
        /// </summary>
        /// <param name="pMin"></param>
        /// <returns></returns>
        public Node Expression(int pMin = 0)
        {
            var leftNode = Primary();

            while (true)
            {
                if (_lexicalAnalyzer.Next() == null)
                    return leftNode;

                var op = SearchOp(_lexicalAnalyzer.Next());

                if (op == null || op.Priority < pMin)
                    return leftNode;

                _lexicalAnalyzer.Skip();
                var rightNode = Expression(op.Priority + op.Association);
                var tree = new Node() { Type = op.Node.Type };
                tree.AddChild(leftNode);
                tree.AddChild(rightNode);
                leftNode = tree;
            }
        }

        /// <summary>
        /// Method to return an operator if the node contains an operator.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Operator SearchOp(Token token)
        {
            if (_exprTokenToNodeMatch.TryGetValue(token.Type, out var vals))
            {
                var (nodetype, val, priority, assos) = vals;
                return new Operator() { Token = token, Node = new Node() { Type = nodetype }, Priority = priority, Association = assos };
            }
            return null;
        }

        public Node Instruction()
        {
            if (_lexicalAnalyzer.Next().Type == TokenType.IF)
            {
                _lexicalAnalyzer.Skip(); // We know it is an if statement
                _lexicalAnalyzer.Accept(TokenType.PAR_OPEN);
                var aTest = Expression();
                _lexicalAnalyzer.Accept(TokenType.PAR_CLOSE);
                var aCode = Instruction();
                var node = new Node() {Type = NodeType.CONDITION};
                node.AddChild(aTest);
                node.AddChild(aCode);
                return node;
            }
            else if (_lexicalAnalyzer.Next().Type == TokenType.BRACKET_OPEN)
            {
                var node = new Node() {Type = NodeType.BLOCK};
                _lexicalAnalyzer.Accept(TokenType.BRACKET_OPEN);
                while (_lexicalAnalyzer.Next().Type != TokenType.BRACKET_CLOSE)
                {
                    var x = Instruction();
                    node.AddChild(x);
                }
                _lexicalAnalyzer.Accept(TokenType.BRACKET_CLOSE);
                return node;
            }
            else
            {
                var ex = Expression();
                _lexicalAnalyzer.Accept(TokenType.SEMICOLON);
                var node = new Node() {Type = NodeType.EXPRESSION};
                node.AddChild(ex);
                return node;
            }
        }
        
    }
}