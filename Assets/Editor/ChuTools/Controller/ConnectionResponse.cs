using Interfaces;

namespace ChuTools.Controller
{
    public delegate bool ConnectionResponse(IConnectionOut co, UIInConnectionPoint cp);

    public delegate bool DisconnectResponse(UIInConnectionPoint point);
}