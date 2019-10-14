using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compil.Utils;

namespace Compil
{
    class LexicalAnalyzer
    {
        private readonly string code;
        public int index;

        public Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
        {
            {"if", TokenType.IF},
            {"else", TokenType.ELSE},
            {"for", TokenType.FOR},
            {"while", TokenType.WHILE},
            {"do", TokenType.DO},
            {"switch", TokenType.SWITCH},
            {"case", TokenType.CASE},
            {"int", TokenType.INT},
            {"void", TokenType.VOID}
        };


        private Token _currentNextToken;
        private int _currentTokenLength = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="index"></param>
        public LexicalAnalyzer(string code, int index)
        {
            this.code = code;
            this.index = index;
        }


        /// <summary>
        /// Get next token
        /// </summary>
        /// <returns></returns>
        public Token Next()
        {
            if (_currentNextToken != null)
            {
                return _currentNextToken;
            }

            var result = DetectNext();
            _currentNextToken = result;
            return result;
        }

        /// <summary>
        /// Jump next token
        /// </summary>
        public void Skip()
        {
            index += _currentTokenLength;
            _currentTokenLength = 0;
            _currentNextToken = null;
        }

        /// <summary>
        /// if next token is searching token -> ok
        /// else error
        /// </summary>
        public void Accept(TokenType type)
        {
            try
            {
                if (Next().Type != type)
                {
                    throw new ArgumentNullException($"Bad token: '{type.ToString()}' expected.");
                }

                Skip();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Search nent token on code
        /// </summary>
        /// <returns></returns>
        private Token DetectNext()
        {
            if (index == code.Length)
            {
                return new Token() {Type = TokenType.END_OF_FILE};
            }

            while (code[index] == ' ' || code[index] == '\t' || code[index] == '\n')
            {
                index++;
            }

            // Constants handle
            if (char.IsDigit(code[index]))
            {
                _currentTokenLength = 1;
                var buffer = code[index].ToString();

                if (index == code.Length - 1)
                {
                    return new Token() {Type = TokenType.CONSTANT, Value = int.Parse(buffer)};
                }

                var i = index + 1;
                while (i < code.Length && char.IsDigit(code[i]))
                {
                    buffer += code[i].ToString();
                    i++;
                    _currentTokenLength++;
                }

                return new Token() {Type = TokenType.CONSTANT, Value = int.Parse(buffer)};
            }

            // Identifier and keywords handle
            if (char.IsLetter(code[index]))
            {
                _currentTokenLength = 1;
                var buffer = code[index].ToString();

                if (index == code.Length - 1)
                {
                    // Look into keywords dictionnary to get the adequate token type
                    if (keywords.ContainsKey(buffer))
                    {
                        return new Token() {Type = keywords[buffer], Name = buffer};
                    }

                    return new Token() {Type = TokenType.IDENTIFIER, Name = buffer};
                }

                var i = index + 1;
                while (i < code.Length && (char.IsLetter(code[i]) || char.IsDigit(code[i])))
                {
                    buffer += code[i].ToString();
                    i++;
                    _currentTokenLength++;
                }

                // Look into keywords dictionnary to get the adequate token type
                if (keywords.ContainsKey(buffer))
                {
                    return new Token() {Type = keywords[buffer], Name = buffer};
                }

                return new Token() {Type = TokenType.IDENTIFIER, Name = buffer};
            }

            // ==
            if (code[index] == '=')
            {
                _currentTokenLength = 1;
                var buffer = code[index].ToString();

                if (index == code.Length - 1)
                {
                    return new Token() {Type = TokenType.EQUAL};
                }

                if (code[index + 1] == '=')
                {
                    _currentTokenLength++;
                    return new Token() {Type = TokenType.COMP_EQUAL};
                }

                return new Token() {Type = TokenType.EQUAL};
            }

            _currentTokenLength++;
            switch (code[index])
            {
                case '+':
                    return new Token() {Type = TokenType.PLUS};
                case '-':
                    return new Token() {Type = TokenType.MINUS};
                case '*':
                    return new Token() {Type = TokenType.MULTIPLY};
                case '/':
                    return new Token() {Type = TokenType.DIVIDE};
                case '%':
                    return new Token() {Type = TokenType.MODULO};
                case '^':
                    return new Token() {Type = TokenType.POWER};
                case '(':
                    return new Token() {Type = TokenType.PAR_OPEN};
                case ')':
                    return new Token() {Type = TokenType.PAR_CLOSE};
                case '{':
                    return new Token() {Type = TokenType.BRACKET_OPEN};
                case '}':
                    return new Token() {Type = TokenType.BRACKET_CLOSE};
                case '&':
                    return new Token() {Type = TokenType.AND};
                case '|':
                    return new Token() {Type = TokenType.OR};
                case ';':
                    return new Token() {Type = TokenType.SEMICOLON};
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}