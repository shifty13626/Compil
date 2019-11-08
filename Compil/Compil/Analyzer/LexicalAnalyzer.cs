using Compil.Utils;
using Compil.Nodes;
using Compil.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using Compil.Exceptions;

namespace Compil
{
    public class LexicalAnalyzer
    {
        private readonly string _code;
        private int _index;
        private int _currentLine;
        private int _currentColumn;
        private Token _currentNextToken;
        private int _currentTokenLength = 0;

        // All code keywords.
        public Dictionary<string, TokenType> LanguageKeywords => new Dictionary<string, TokenType>()
        {
            {"if", TokenType.IF},
            {"else", TokenType.ELSE},
            {"for", TokenType.FOR},
            {"while", TokenType.WHILE},
            {"do", TokenType.DO},
            {"switch", TokenType.SWITCH},
            {"case", TokenType.CASE},
            {"int", TokenType.INT},
            {"void", TokenType.VOID},
            {"var", TokenType.VAR},
            {"return", TokenType.RETURN},
            {"function", TokenType.FUNCTION}
        };

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="index"></param>
        public LexicalAnalyzer(string code, int index)
        {
            this._code = code;
            this._index = index;
            this._currentLine = 0;
            this._currentColumn = 0;
        }

        /// <summary>
        /// Get the next token in the code with caching.
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
        /// Jump to the next token.
        /// </summary>
        public void Skip()
        {
            _index += _currentTokenLength;
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
                    throw new LexicalErrorException($"Bad token: '{type.ToString()}' expected at line {_currentLine}.");
                }

                Skip();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Detect and return the next token in the code
        /// </summary>
        /// <returns></returns>
        private Token DetectNext()
        {
            var builder = new StringBuilder();

            if (_index >= _code.Length - 1)
            {
                return new Token() {Type = TokenType.END_OF_FILE};
            }

            while (_code[_index] == ' ' || _code[_index] == '\t' || _code[_index] == '\n' || _code[_index] == '\r')
            {
                if (_code[_index] == '\n' || _code[_index] == '\r')
                {
                    _currentLine++;
                    _currentColumn = 0;
                }

                if (_code[_index] == ' ' || _code[_index] == '\t')
                {
                    _currentColumn++;
                }
                
                _index++;
            }

            // Constants handle
            if (char.IsDigit(_code[_index]))
            {
                _currentTokenLength = 1;

                builder.Append(_code[_index].ToString());

                if (_index == _code.Length - 1)
                {
                    return new Token()
                    {
                        Type = TokenType.CONSTANT, Value = int.Parse(builder.ToString()), Line = _currentLine,
                        Column = _currentColumn
                    };
                }

                var i = _index + 1;
                while (i < _code.Length && char.IsDigit(_code[i]))
                {
                    builder.Append(_code[i].ToString());
                    i++;
                    _currentTokenLength++;
                }

                return new Token()
                {
                    Type = TokenType.CONSTANT, Value = int.Parse(builder.ToString()), Line = _currentLine,
                    Column = _currentColumn
                };
            }

            // Identifier and keywords handle
            if (char.IsLetter(_code[_index]))
            {
                _currentTokenLength = 1;
                builder.Append(_code[_index].ToString());

                if (_index == _code.Length - 1)
                {
                    // Look into keywords dictionnary to get the adequate token type
                    if (LanguageKeywords.ContainsKey(builder.ToString()))
                    {
                        return new Token() {Type = LanguageKeywords[builder.ToString()], Name = builder.ToString()};
                    }

                    return new Token()
                    {
                        Type = TokenType.IDENTIFIER, Name = builder.ToString(), Line = _currentLine,
                        Column = _currentColumn
                    };
                }

                var i = _index + 1;
                while (i < _code.Length && (char.IsLetter(_code[i]) || char.IsDigit(_code[i])))
                {
                    builder.Append(_code[i].ToString());
                    i++;
                    _currentTokenLength++;
                }

                // Look into keywords dictionnary to get the adequate token type
                if (LanguageKeywords.ContainsKey(builder.ToString()))
                {
                    return new Token()
                    {
                        Type = LanguageKeywords[builder.ToString()], Name = builder.ToString(), Line = _currentLine,
                        Column = _currentColumn
                    };
                }

                return new Token()
                {
                    Type = TokenType.IDENTIFIER, Name = builder.ToString(), Line = _currentLine, Column = _currentColumn
                };
            }

            // ==
            if (_code[_index] == '=')
            {
                _currentTokenLength = 1;
                builder.Append(_code[_index].ToString());

                if (_index == _code.Length - 1)
                {
                    return new Token() {Type = TokenType.EQUAL, Line = _currentLine, Column = _currentColumn};
                }

                if (_code[_index + 1] == '=')
                {
                    _currentTokenLength++;
                    return new Token() {Type = TokenType.COMP_EQUAL, Line = _currentLine, Column = _currentColumn};
                }

                return new Token() {Type = TokenType.EQUAL, Line = _currentLine, Column = _currentColumn};
            }

            // >= and >
            if (_code[_index] == '>')
            {
                _currentTokenLength = 1;
                builder.Append(_code[_index].ToString());

                if (_index == _code.Length - 1)
                {
                    return new Token() {Type = TokenType.COMP_SUPPERIOR, Line = _currentLine, Column = _currentColumn};
                }

                if (_code[_index + 1] == '=')
                {
                    _currentTokenLength++;
                    return new Token()
                        {Type = TokenType.COMP_SUPPERIOR_OR_EQUAL, Line = _currentLine, Column = _currentColumn};
                }

                return new Token() {Type = TokenType.COMP_SUPPERIOR, Line = _currentLine, Column = _currentColumn};
            }

            // <= and <
            if (_code[_index] == '<')
            {
                _currentTokenLength = 1;
                builder.Append(_code[_index].ToString());

                if (_index == _code.Length - 1)
                {
                    return new Token() {Type = TokenType.COMP_INFERIOR, Line = _currentLine, Column = _currentColumn};
                }

                if (_code[_index + 1] == '=')
                {
                    _currentTokenLength++;
                    return new Token()
                        {Type = TokenType.COMP_INFERIOR_OR_EQUAL, Line = _currentLine, Column = _currentColumn};
                }

                return new Token() {Type = TokenType.COMP_INFERIOR, Line = _currentLine, Column = _currentColumn};
            }

            // ! and !=
            if (_code[_index] == '!')
            {
                _currentTokenLength = 1;
                builder.Append(_code[_index].ToString());

                if (_index == _code.Length - 1)
                {
                    return new Token() {Type = TokenType.NOT, Line = _currentLine, Column = _currentColumn};
                }

                if (_code[_index + 1] == '=')
                {
                    _currentTokenLength++;
                    return new Token() {Type = TokenType.COMP_DIFFERENT, Line = _currentLine, Column = _currentColumn};
                }

                return new Token() {Type = TokenType.NOT, Line = _currentLine, Column = _currentColumn};
            }

            _currentTokenLength++;
            switch (_code[_index])
            {
                case '+':
                    return new Token() {Type = TokenType.PLUS, Line = _currentLine, Column = _currentColumn};
                case '-':
                    return new Token() {Type = TokenType.MINUS, Line = _currentLine, Column = _currentColumn};
                case '*':
                    return new Token() {Type = TokenType.MULTIPLY, Line = _currentLine, Column = _currentColumn};
                case '/':
                    return new Token() {Type = TokenType.DIVIDE, Line = _currentLine, Column = _currentColumn};
                case '%':
                    return new Token() {Type = TokenType.MODULO, Line = _currentLine, Column = _currentColumn};
                case '^':
                    return new Token() {Type = TokenType.POWER, Line = _currentLine, Column = _currentColumn};
                case '(':
                    return new Token() {Type = TokenType.PAR_OPEN, Line = _currentLine, Column = _currentColumn};
                case ')':
                    return new Token() {Type = TokenType.PAR_CLOSE, Line = _currentLine, Column = _currentColumn};
                case '{':
                    return new Token() {Type = TokenType.BRACKET_OPEN, Line = _currentLine, Column = _currentColumn};
                case '}':
                    return new Token() {Type = TokenType.BRACKET_CLOSE, Line = _currentLine, Column = _currentColumn};
                case '&':
                    return new Token() {Type = TokenType.AND, Line = _currentLine, Column = _currentColumn};
                case '|':
                    return new Token() {Type = TokenType.OR, Line = _currentLine, Column = _currentColumn};
                case ';':
                    return new Token() {Type = TokenType.SEMICOLON, Line = _currentLine, Column = _currentColumn};
                case ',':
                    return new Token() {Type = TokenType.COMA, Line = _currentLine, Column = _currentColumn};
            }

            throw new LexicalErrorException($"Token not recognized at line {_currentLine}");
        }
    }
}