using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System.Reflection;
using ChuTools.Controller;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChuTools
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
                {
                    gm.AddItem(Contents[i], false, RemoveItem, i);
                }

                gm.ShowAsContext();
                Event.current.Use();
            }

            return true;
        }

        public void RemoveItem(object index)
        {
            var eindex = (int)index;//unbox
        }

        public static List<Type> TypeDropdownList<T>(Rect rect)
        {
            var types = FindAllDerivedTypes<T>();
            var names = types.Select(t => t.Name).ToList();
            var guiContents = new GUIContent[names.Count];

            for (var i = 0; i < names.Count; i++)
            {
                guiContents[i] = new GUIContent(types[i].Name);
            }

            return types;
        }

        public static List<Type> FindAllDerivedTypes<T>()
        {
            return FindAllDerivedTypes<T>(Assembly.GetAssembly(typeof(T)));
        }

        public static List<Type> FindAllDerivedTypes<T>(Assembly assembly)
        {
            var derivedType = typeof(T);
            return assembly
                .GetTypes()
                .Where(t => t != derivedType && derivedType.IsAssignableFrom(t))
                .ToList();

        }

    }
    public static class Extensions
    {


        public static Rect MoveDown(this Rect rect, float amount)
        {
            var arect = rect;
            arect.position = new Vector2(rect.x, rect.y + amount);
            return arect;
        }

        public static void SetX(this Vector3 v3, float value)
        {
            v3 = new Vector3(value, v3.y, v3.z);
        }

        public static void SetY(this Vector3 v3, float value)
        {
            v3 = new Vector3(v3.x, value, v3.z);
        }

        public static void SetZ(this Vector3 v3, float value)
        {
            v3 = new Vector3(v3.x, v3.y, value);
        }

        public static void SetX(this Vector2 v2, float value)
        {
            v2 = new Vector3(value, v2.y);
        }

        public static void SetY(this Vector2 v2, float value)
        {
            v2 = new Vector3(v2.x, value);
        }
    }
}