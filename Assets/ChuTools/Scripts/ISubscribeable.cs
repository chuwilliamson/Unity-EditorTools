namespace ChuTools.Scripts
{
    public interface ISubscribeable
    {
        void RegisterListener(IListener listener);

        void UnregisterListener(IListener listener);

        void Raise(params object[] args);
    }
}