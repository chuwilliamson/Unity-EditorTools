namespace Interfaces
{
    public interface IConnection
    {
        INode In { get; set; }
        INode Out { get; set; }
    }
}