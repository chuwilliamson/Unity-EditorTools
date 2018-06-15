﻿using ChuTools.Controller;
using ChuTools.Model;
using Interfaces;
using JeremyTools;
using Newtonsoft.Json;
using RoslynCompiler;
using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TrentTools
{
    [Serializable]
    public class UIRoslynNode : UIElement
    {
        [JsonConstructor]
        public UIRoslynNode()
        {
            Node = new MethodNode(new MethodObject
            {
                Target = this,
                Type = typeof(UIRoslynNode),
                MethodName = "DoCompile"
            });

            Out = new UIOutConnectionPoint(new Rect(this.rect.position, new Vector2(50, 50)), new OutConnection(Node));
            Base(rect, "Script Node", resize: true);
        }

        public UIRoslynNode(Rect rect)
        {
            Node = new MethodNode(new MethodObject
            {
                Target = this,
                Type = typeof(UIRoslynNode),
                MethodName = "DoCompile"
            });

            Out = new UIOutConnectionPoint(new Rect(this.rect.position, new Vector2(50, 50)), new OutConnection(Node));
            Base(rect, "Script Node", resize: true);

        }

        public void DoCompile()
        {
            switch (selected_output)
            {
                case Output_Options.Int:
                    {
                        result = Compile<int>(codeinput).ToString();
                        break;
                    }
                case Output_Options.Float:
                    {
                        result = Compile<float>(codeinput).ToString();
                        break;
                    }
                case Output_Options.Bool:
                    {
                        result = Compile<bool>(codeinput).ToString();
                        break;
                    }
                case Output_Options.String:
                    {
                        result = Compile<string>(codeinput);
                        break;
                    }
                case Output_Options.Object:
                    {
                        var obj = Compile<object>(codeinput);
                        result = obj.ToString();
                        ResultTuple = new Tuple<string, object>(result, obj);
                        break;
                    }
                default:
                    {
                        result = "DEFAULT CASE REACHED, WE SHOULDENT BE HERE...";
                        break;
                    }
            }
        }

        public static T Compile<T>(string code)
        {
            return RoslynWrapper.Evaluate<T>(code, new System.Collections.Generic.List<Type>() { typeof(GameObject), typeof(Transform) }, new System.Collections.Generic.List<string>() { }).Result;
        }

        public override void Draw()
        {
            base.Draw();
            Out.rect = new Rect(rect.position.x + rect.width, rect.position.y, 50, 50);
            Out.Draw();
            GUILayout.BeginArea(rect);

            GameObjectRef = EditorGUILayout.ObjectField((GameObject)GameObjectRef, typeof(GameObject), true);

            GUILayout.Space(20);

            selected_output = (Output_Options)EditorGUILayout.EnumPopup("OUTPUT", selected_output);

            codeinput = EditorGUILayout.TextArea(codeinput);

            EditorGUILayout.LabelField("Result = :: " + result);

            GUILayout.EndArea();
        }

        public UnityEngine.Object GameObjectRef;
        public UIOutConnectionPoint Out { get; set; }
        public INode Node { get; set; }

        #region Fields
        [SerializeField] private string codeinput = "return new UnityEngine.GameObject()";
        public string CodeInput { get { return codeinput; } set { codeinput = value; } }

        [SerializeField] private string result = string.Empty;
        public string Result { get { return result; } set { result = value; } }
        public Tuple<string, object> ResultTuple { get; set; }
        public enum Output_Options { Int = 0, Float = 1, Bool = 2, String = 3, Object = 4 };
        private Output_Options selected_output = Output_Options.Object;
        public Output_Options SelectedOutput { get { return selected_output; } set { selected_output = value; } }
        #endregion Fields
    }
}