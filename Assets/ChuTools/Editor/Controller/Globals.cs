using ChuTools.Controller;
using ChuTools.Scripts;

namespace ChuTools
{
    public static class Globals
    {
        public static ISubscribeable DisconnectEvent = new ConnectionEvent();
        public static ISubscribeable ConnectedEvent = new ConnectionEvent();
    }
}