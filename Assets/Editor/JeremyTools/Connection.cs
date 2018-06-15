using Interfaces;

namespace JeremyTools
{
    public partial class Connection
    {
        public Connection(IDrawable inN, IDrawable outN)
        {
            In = inN;
            Out = outN;
        }
    }
}