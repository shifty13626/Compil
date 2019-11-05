using Compil.Utils;
using Compil.Tokens;

namespace Compil
{
    /// <summary>
    /// Instance of class Token
    /// </summary>
    public class Token
    {
        #region properties
        /// <summary>
        /// Name of token
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Value of token (for const)
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// Line of token on source code
        /// </summary>
        public int Line { get; set; }
        /// <summary>
        /// Column of token on source code
        /// </summary>
        public int Column { get; set; }
        /// <summary>
        /// Type of token found
        /// </summary>
        public TokenType Type { get; set; }
        #endregion
    }
}
