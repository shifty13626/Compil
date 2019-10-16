namespace Compil.Utils
{
    public class Operator
    {
        public Token Token { get; set; }
        public Node Node { get; set; }
        public int Priority { get; set; }
        public int Association { get; set; }
    }
}
