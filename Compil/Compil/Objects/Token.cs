using Compil.Utils;

namespace Compil
{
    /// <summary>
    /// Instance of class Token
    /// </summary>
    public class Token
    {
        #region properties
        public string Name { get; set; }
        public int Value { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public TokenType Type { get; set; }
        #endregion
    }
}
