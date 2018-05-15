 
namespace Contexts.Concrete
{
    [System.Serializable]
    public class AntContext : Context
    { 
        public void Update(object sender)
        { 
            CurrentState.Update(this);
        } 
    }
}