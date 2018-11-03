namespace ChuTools.NodeEditor.Interfaces
{
    public interface IConnectionIn
    {
        IConnectionOut Out { get; set; }

        /// <summary>
        ///     this will return the Out.Value
        /// </summary>
        object Value { get; }
    }
}