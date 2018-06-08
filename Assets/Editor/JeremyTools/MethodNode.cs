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

        public MethodNode(MethodObject methodObject)
        {
            Value = methodObject;
        }
    }
}