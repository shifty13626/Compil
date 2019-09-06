using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compil.Utils
{
    class Operator
    {
        public Token Token { get; set; }
        public Node Node { get; set; }
        public int Priority { get; set; }
        public int Association { get; set; }
    }
}
