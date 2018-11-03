namespace ChuTools.NodeEditor.Interfaces
{
    public interface IConnectionOut
    {
        /// <summary>
        ///     This will return the Node.Value
        /// </summary>
        object Value { get; }

        /// <summary>
        ///     Out node from an input type node
        /// </summary>
        INode Node { get; set; }
    }
}