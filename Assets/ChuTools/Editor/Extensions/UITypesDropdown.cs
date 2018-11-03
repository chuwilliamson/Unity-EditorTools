using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChuTools.Extensions
{
    public class UITypesDropdown
    {
        public List<string> Names => FindAllDerivedTypes<Object>().Select(t => t.Name).ToList();
        public List<GUIContent> Contents => new List<GUIContent>(Names.Select(n => new GUIContent(n)));

        public bool Button(Rect rect)
        {
            rect.MoveDown(rect.y + EditorGUIUtility.singleLineHeight);
            if (GUI.Button(rect, "Types"))
            {
                var gm = new GenericMenu();
                for (var i = 0; i < Names.Count; i++)
                    gm.AddItem(Contents[i], false, RemoveItem, i);

                gm.ShowAsContext();
                Event.current.Use();
            }

            return true;
        }

        public void RemoveItem(object index)
        {
        }

        public static List<Type> TypeDropdownList<T>(Rect rect)
        {
            var types = FindAllDerivedTypes<T>();
            var names = types.Select(t => t.Name).ToList();
            var guiContents = new GUIContent[names.Count];

            for (var i = 0; i < names.Count; i++)
                guiContents[i] = new GUIContent(types[i].Name);

            return types;
        }

        public static List<Type> FindAllDerivedTypes<T>()
        {
            return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
        }

        public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
        {
            var derivedType = typeof(T);
            return assembly.GetTypes().Where(t => t != derivedType && derivedType.IsAssignableFrom(t)).ToList();
        }
    }
}