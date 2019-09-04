using Compil.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compil
{
    /// <summary>
    /// Instance of class Node
    /// </summary>
    public class Node
    {
        #region Properties
        public NodeType Type { get; set; }
        public float Value { get; set; }
        public List<Node> Children { get; set; }

        #endregion

        #region Operations

        public void AddChildren(Node node)
        {
            Children.Add(node);
        }

        #endregion

    }
}
