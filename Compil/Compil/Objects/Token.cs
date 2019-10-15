using Compil.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public TokenType Type {get; set;}
        #endregion
    }
}
