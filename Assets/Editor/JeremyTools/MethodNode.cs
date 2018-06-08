using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Interfaces;
using UnityEngine;

namespace JeremyTools
{
    public class MethodNode : INode
    {
        public object Value { get; set; }
        private MethodInfo methodInfo;
        private object sender;

        public MethodNode(MethodInfo info)
        {
            var t = GetType();
            methodInfo = t.GetMethod("TestMethod");
            sender = this;
        }

        public void TestMethod()
        {
            Debug.Log("im from the methodnode " + methodInfo.Name.ToString());
        }
    }
}