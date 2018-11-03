using System;

namespace ChuTools.NodeEditor.Model
{
    [Serializable]
    public class MethodObject
    {
        public object Result;
        public object Target { get; set; }
        public string MethodName { get; set; }
        public Type Type { get; set; }

        public void DynamicInvoke()
        {
            var method = Type.GetMethod(MethodName);
            Result = method?.Invoke(Target, new object[] { });
        }
    }
}