namespace Compil.Symbols
{
    public class Symbol
    {
        /// <summary>
        /// Id of symbol
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Type of Symbol
        /// </summary>
        public SymbolType Type { get; set; }
        /// <summary>
        /// Index on stack of machine
        /// </summary>
        public int Slot { get; set; }
    }
}