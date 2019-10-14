using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compil.Utils;
using Compil;

namespace Compil
{
    /// <summary>
    /// Instance of class SyntacticAnalyzer
    /// </summary>
    class SyntaxAnalyzer
    {
        private LexicalAnalyzer _lexicalAnalyzer;

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

        private Dictionary<TokenType, (NodeType, string, int, int)> _exprTokenToNodeMatch = new Dictionary<TokenType, (NodeType, string, int, int)>()
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
                    node = new Node() {Type = NodeType.CONSTANT, Value = _lexicalAnalyzer.Next().Value.ToString()};
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
                    node = new Node() {Type = nodeType, Value = val};
                    _lexicalAnalyzer.Skip();
                    node.AddChild(Expression(7));
                    return node;
                }
                
                // Throw error, primary not detected.
                throw new Exception("Primary expected.");
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine("Feature not implemented.");
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
            var leftNode = Primary();
            
            while(true) {
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
            if (_exprTokenToNodeMatch.TryGetValue(token.Type, out var vals)) {
                var (nodetype, val, priority, assos) = vals;
                return new Operator() { Token = token, Node = new Node() {Type = nodetype}, Priority = priority, Association = assos};
            }
            return null;
        }

    }
}