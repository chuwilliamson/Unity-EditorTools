namespace ChuTools.NodeEditor.Interfaces
{
    public interface IConnection
    {
        IDrawable In { get; set; }
        IDrawable Out { get; set; }
    }
}