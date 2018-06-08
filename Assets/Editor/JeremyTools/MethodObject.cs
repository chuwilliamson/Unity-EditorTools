

namespace JeremyTools
{
    [System.Serializable]
    public class MethodObject
    {
        public object Target { get; set; }
        public string MethodName { get; set; }
        public System.Type Type { get; set; }

        public void DynamicInvoke()
        {
            var method = Type.GetMethod(MethodName);
            method?.Invoke(Target, new object[] { });
        }
    }
}