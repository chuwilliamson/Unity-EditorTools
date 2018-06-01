namespace Interfaces
{

    public interface IConnection
    {
        IDrawable In { get; set; }
        IDrawable Out { get; set; }
    }
    public interface INode
    {
        int Value { get; set; }
    }
    public interface IConnectionOut
    {
        /// <summary>
        /// This will return the Node.Value
        /// </summary>
        int Value { get; }

        INode Node { get; set; }

    }
    public interface IConnectionIn
    {
        IConnectionOut Out { get; set; }
        /// <summary>
        /// this will return the Out.Value
        /// </summary>
        int Value { get; set; }

    }
}