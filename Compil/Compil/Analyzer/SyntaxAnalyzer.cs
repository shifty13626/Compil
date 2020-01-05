using Compil.Utils;
using Compil.Tokens;
using Compil.Nodes;
using System;
using System.Collections.Generic;
using System.Data;

namespace Compil {
    /// <summary>
    /// Instance of class SyntacticAnalyzer
    /// </summary>
    public class SyntaxAnalyzer {
        public LexicalAnalyzer LexicalAnalyzer { get; }

        // Dictionnary to match token and nodes.
        private readonly Dictionary<TokenType, (NodeType, string, int, int)> _exprTokenToNodeMatch =
            new Dictionary<TokenType, (NodeType, string, int, int)>() {
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
                {TokenType.EQUAL, (NodeType.ASSIGN, "=", 1, 0)}
            };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lexicalAnalyzer"></param>
        public SyntaxAnalyzer(LexicalAnalyzer lexicalAnalyzer) {
            this.LexicalAnalyzer = lexicalAnalyzer;
        }

        /// <summary>
        /// Build tree of expresion
        /// </summary>
        public Node Primary() {
            Node node;

            // Constant
            if (LexicalAnalyzer.Next().Type == TokenType.CONSTANT) {
                node = new Node() {Type = NodeType.CONSTANT, Value = LexicalAnalyzer.Next().Value.ToString()};
                LexicalAnalyzer.Skip();
                return node;
            }

            // Parenthesis Open
            if (LexicalAnalyzer.Next().Type == TokenType.PAR_OPEN) {
                LexicalAnalyzer.Skip();
                node = Expression(0);
                LexicalAnalyzer.Accept(TokenType.PAR_CLOSE);
                return node;
            }

            // Unary operators
            if (LexicalAnalyzer.Next().Type == TokenType.MINUS) {
                var (nodeType, val, _, _) = _exprTokenToNodeMatch[LexicalAnalyzer.Next().Type];
                node = new Node() {Type = NodeType.MINUS, Value = val};
                LexicalAnalyzer.Skip();
                node.AddChild(Expression(7));
                return node;
            }

            if (LexicalAnalyzer.Next().Type == TokenType.PLUS) {
                var (nodeType, val, _, _) = _exprTokenToNodeMatch[LexicalAnalyzer.Next().Type];
                node = new Node() {Type = NodeType.PLUS, Value = val};
                LexicalAnalyzer.Skip();
                node.AddChild(Expression(7));
                return node;
            }
            
            if (LexicalAnalyzer.Next().Type == TokenType.NOT) {
                var (nodeType, val, _, _) = _exprTokenToNodeMatch[LexicalAnalyzer.Next().Type];
                node = new Node() {Type = NodeType.NOT, Value = val};
                LexicalAnalyzer.Skip();
                node.AddChild(Expression(7));
                return node;
            }
            
            // Identifiers handling
            if (LexicalAnalyzer.Next().Type == TokenType.IDENTIFIER)
            {
                var identifierName = LexicalAnalyzer.Next().Name; // Identifier name
                LexicalAnalyzer.Skip();
                
                if (LexicalAnalyzer.Next().Type == TokenType.PAR_OPEN)
                {
                    LexicalAnalyzer.Skip();
                    // We have a function call
                    var callNode = new Node() {Type = NodeType.CALL, Value = identifierName};
                    while (LexicalAnalyzer.Next().Type != TokenType.PAR_CLOSE)
                    {
                        var nodeArg = Expression();
                        callNode.AddChild(nodeArg);
                        
                        if(LexicalAnalyzer.Next().Type != TokenType.PAR_CLOSE)
                            LexicalAnalyzer.Accept(TokenType.COMA);
                    }

                    LexicalAnalyzer.Accept(TokenType.PAR_CLOSE);
                    return callNode;
                }
                
                // Otherwise, we have a variable
                return new Node() {Type = NodeType.VARIABLE, Value = identifierName};
            }

            // Primary not found
            throw new SyntaxErrorException($"Primary expected at line {LexicalAnalyzer.Next().Line}");
        }

        /// <summary>
        /// return node with an expression (xxxx)
        /// </summary>
        /// <param name="pMin"></param>
        /// <returns></returns>
        public Node Expression(int pMin = 0) {
            var leftNode = Primary();

            while (true) {
                if (LexicalAnalyzer.Next() == null)
                    return leftNode;

                var op = SearchOp(LexicalAnalyzer.Next());

                if (op == null || op.Priority < pMin)
                    return leftNode;

                LexicalAnalyzer.Skip();
                var rightNode = Expression(op.Priority + op.Association);
                var tree = new Node() {Type = op.Node.Type};
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
        public Operator SearchOp(Token token) {
            if (_exprTokenToNodeMatch.TryGetValue(token.Type, out var vals)) {
                var (nodetype, val, priority, assos) = vals;
                return new Operator()
                    {Token = token, Node = new Node() {Type = nodetype}, Priority = priority, Association = assos};
            }

            return null;
        }


        public Node Function()
        {
            // Function declare handling
            if (LexicalAnalyzer.Next().Type != TokenType.FUNCTION)
                throw new SyntaxErrorException($"Expected function declaration at line {LexicalAnalyzer.Next().Line}");
            
            LexicalAnalyzer.Skip();
            
            if (LexicalAnalyzer.Next().Type != TokenType.IDENTIFIER)
            {
                throw new SyntaxErrorException($"Expected function name at line {LexicalAnalyzer.Next().Line}");
            }
                
            var functionName = LexicalAnalyzer.Next().Name;
            var nodeFunction = new Node() {Type = NodeType.FUNCTION, Value = functionName};

            LexicalAnalyzer.Skip();
                
            LexicalAnalyzer.Accept(TokenType.PAR_OPEN);

            var nodeBlock = new Node() { Type = NodeType.BLOCK };
                
            while (LexicalAnalyzer.Next().Type != TokenType.PAR_CLOSE)
            {
                var nameArg = LexicalAnalyzer.Next().Name;
                var declare = new Node() {Type = NodeType.DECLARE};
                declare.Value = nameArg;
                
                //declare.AddChild(new Node() {Type = NodeType.VARIABLE, Value = nameArg});
                nodeBlock.AddChild(declare);
                    
                LexicalAnalyzer.Skip();
                    
                if(LexicalAnalyzer.Next().Type != TokenType.PAR_CLOSE)
                    LexicalAnalyzer.Accept(TokenType.COMA);
            }
            LexicalAnalyzer.Accept(TokenType.PAR_CLOSE);

            var nodeInstructions = Instruction();
                
            nodeBlock.AddChild(nodeInstructions);
            nodeFunction.AddChild(nodeBlock);
                
            return nodeFunction;
        }
        
        /// <summary>
        /// Create all nodes for code instructions.
        /// </summary>
        /// <returns></returns>
        public Node Instruction() {
            // If condition handling
            if (LexicalAnalyzer.Next().Type == TokenType.IF) {
                LexicalAnalyzer.Skip(); // We know it is an 'if' statement so we skip this token
                LexicalAnalyzer.Accept(TokenType.PAR_OPEN);
                var aTest = Expression();
                LexicalAnalyzer.Accept(TokenType.PAR_CLOSE);
                var aCode = Instruction();
                var node = new Node() {Type = NodeType.CONDITION};
                node.AddChild(aTest);
                node.AddChild(aCode);

                if (LexicalAnalyzer.Next().Type == TokenType.ELSE) {
                    LexicalAnalyzer.Skip();
                    var aElse = Instruction();
                    node.AddChild(aElse);
                }

                return node;
            }

            // For loop handling
            if (LexicalAnalyzer.Next().Type == TokenType.FOR) {
                LexicalAnalyzer.Skip();
                LexicalAnalyzer.Accept(TokenType.PAR_OPEN);

                // Get all parts of parameters for the 'for' loop
                var initValue = Expression();
                LexicalAnalyzer.Accept(TokenType.SEMICOLON);
                var condition = Expression();
                LexicalAnalyzer.Accept(TokenType.SEMICOLON);
                var step = Expression();
                LexicalAnalyzer.Accept(TokenType.PAR_CLOSE);

                //block commands
                var block = Instruction();

                var loopNode = new Node() {Type = NodeType.LOOP};
                var conditionNode = new Node() {Type = NodeType.CONDITION};
                var variableBlock = new Node() {Type = NodeType.BLOCK};
                var loopBlockNode = new Node() {Type = NodeType.BLOCK};
                var breakLoop = new Node() {Type = NodeType.BREAK};

                variableBlock.AddChild(initValue);
                variableBlock.AddChild(loopNode);

                loopNode.AddChild(conditionNode);

                conditionNode.AddChild(condition);
                conditionNode.AddChild(loopBlockNode);
                conditionNode.AddChild(breakLoop);

                loopBlockNode.AddChild(block);
                loopBlockNode.AddChild(step);

                return variableBlock;
            }

            // While loop handling
            if (LexicalAnalyzer.Next().Type == TokenType.WHILE) {
                LexicalAnalyzer.Skip();
                LexicalAnalyzer.Accept(TokenType.PAR_OPEN);
                var aTest = Expression();
                LexicalAnalyzer.Accept(TokenType.PAR_CLOSE);
                var aCode = Instruction();
                var node = new Node() {Type = NodeType.LOOP};
                var cond = new Node() {Type = NodeType.CONDITION};
                cond.AddChild(aTest);
                cond.AddChild(aCode);
                cond.AddChild(new Node() {Type = NodeType.BREAK});

                node.AddChild(cond);
                return node;
            }

            // Do loop handling
            if (LexicalAnalyzer.Next().Type == TokenType.DO)
            {
                LexicalAnalyzer.Skip();
                var aCode = Instruction();
                LexicalAnalyzer.Accept(TokenType.WHILE);
                LexicalAnalyzer.Accept(TokenType.PAR_OPEN);
                var aTest = Expression();
                LexicalAnalyzer.Accept(TokenType.PAR_CLOSE);
                LexicalAnalyzer.Accept(TokenType.SEMICOLON);
                var node = new Node() { Type = NodeType.LOOP };
                
                
                
                var cond = new Node() { Type = NodeType.CONDITION };
                cond.AddChild(aTest);
                cond.AddChild(new Node() {Type = NodeType.BLOCK});
                cond.AddChild(new Node() {Type = NodeType.BREAK});
                
                aCode.AddChild(cond);
                node.AddChild(aCode);
                
                return node;
                
            }
            
            // Block tokens handling
            if (LexicalAnalyzer.Next().Type == TokenType.BRACKET_OPEN) {
                var node = new Node() {Type = NodeType.BLOCK};
                LexicalAnalyzer.Accept(TokenType.BRACKET_OPEN);
                while (LexicalAnalyzer.Next().Type != TokenType.BRACKET_CLOSE) {
                    var x = Instruction();
                    node.AddChild(x);
                }

                LexicalAnalyzer.Accept(TokenType.BRACKET_CLOSE);
                return node;
            }

            // Node return
            if (LexicalAnalyzer.Next().Type == TokenType.RETURN)
            {
                LexicalAnalyzer.Skip();
                var nodeReturn = new Node() {Type = NodeType.RETURN};
                var ex = Expression();
                LexicalAnalyzer.Accept(TokenType.SEMICOLON);
                nodeReturn.AddChild(ex);
                return nodeReturn;
            }

            // Send
            if (LexicalAnalyzer.Next().Type == TokenType.SEND) {
                LexicalAnalyzer.Skip();
                var nodeSend = new Node() {Type = NodeType.SEND};
                var ex = Expression();
                LexicalAnalyzer.Accept(TokenType.SEMICOLON);
                nodeSend.AddChild(ex);
                return nodeSend;
            }

            // 'VAR' token handling
            if (LexicalAnalyzer.Next().Type == TokenType.VAR) {
                LexicalAnalyzer.Skip();

                if (LexicalAnalyzer.LanguageKeywords.ContainsValue(LexicalAnalyzer.Next().Type)) {
                    throw new SyntaxErrorException(
                        $"Cannot declare a variable with name which is language keyword. Error at line {LexicalAnalyzer.Next().Line}");
                }
                
                if (LexicalAnalyzer.Next().Type == TokenType.IDENTIFIER) {
                    var variableName = LexicalAnalyzer.Next().Name;
                    var nodeVariable = new Node() {Type = NodeType.DECLARE};
                    nodeVariable.Value = variableName;
                    var ex = Expression();

                    LexicalAnalyzer.Accept(TokenType.SEMICOLON);

                    if (ex.Type != NodeType.ASSIGN) {
                        if (ex.Children.Count != 0) {
                            throw new SyntaxErrorException(
                                $"No variable assignment at line {LexicalAnalyzer.Next().Line}");
                        }
                    }

                    if(ex.Type != NodeType.VARIABLE)
                        nodeVariable.AddChild(ex);

                    return nodeVariable;
                }

                LexicalAnalyzer.Accept(TokenType.SEMICOLON);
                throw new SyntaxErrorException(
                    $"Tried to declare a variable without name at line {LexicalAnalyzer.Next().Line}");
            }

            
            
            // Other tokens
            {
                var ex = Expression();
                LexicalAnalyzer.Accept(TokenType.SEMICOLON);
                var node = new Node() {Type = NodeType.EXPRESSION};
                node.AddChild(ex);
                return node;
            }
        }
    }
}