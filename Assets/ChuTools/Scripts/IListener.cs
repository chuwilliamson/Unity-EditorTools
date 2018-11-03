namespace ChuTools.Scripts
{
    public interface IListener
    {
        void OnEventRaised(object[] args);

        void Subscribe();

        void Unsubscribe();
    }
}