using System;

namespace JeremyTools
{
    [Serializable]
    public class MethodObject
    {
        public void DynamicInvoke()
        {
            var method = Type.GetMethod(MethodName);
            Result = method?.Invoke(Target, new object[] { });
        }

        public object Result;
        public object Target { get; set; }
        public string MethodName { get; set; }
        public Type Type { get; set; }
    }
}