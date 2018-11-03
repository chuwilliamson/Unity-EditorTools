using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace ChuTools.Attributes
{
    /// <summary>
    ///     add this attribute to display a dropdown list of all scriptableobject types
    ///     this will disable the ability to select an object reference through the object field property drawer
    /// </summary>
    public class ScriptVariableAttribute : PropertyAttribute
    {
        public readonly ScriptableObject[] Vars;

        public ScriptVariableAttribute(Type type) : this(type.ToString())
        {
        }

        public ScriptVariableAttribute(string typename)
        {
            var filter = "t:" + typename;
#if UNITY_EDITOR
            Vars = AssetDatabase.FindAssets(filter).Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>).Where(b => b).OrderBy(v => v.name).ToArray();
#endif
        }
    }
}