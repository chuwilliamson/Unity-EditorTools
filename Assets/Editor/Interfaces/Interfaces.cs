namespace Interfaces
{

    public interface IConnection
    {
        IDrawable In { get; set; }
        IDrawable Out { get; set; }
    }
    public interface INode
    {
        object Value { get; set; }
    }
    public interface IConnectionOut
    {
        /// <summary>
        /// This will return the Node.Value
        /// </summary>
        object Value { get;  }

        INode Node { get; set; }

    }
    public interface IConnectionIn
    {
        IConnectionOut Out { get; set; }
        /// <summary>
        /// this will return the Out.Value
        /// </summary>
        object Value { get;  }

    }
}