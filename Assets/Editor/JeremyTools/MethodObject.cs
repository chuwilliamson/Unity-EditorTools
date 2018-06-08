using System.Reflection;

namespace JeremyTools
{
    [System.Serializable]
    public class MethodObject
    {
        public object Target;
        public string MethodName;
        public MethodInfo Info;

        public void Invoke()
        {
            Info.Invoke(Target, new object[] { });
        }
    }
}