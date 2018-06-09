using System;
using ChuTools.Controller;
using ChuTools.Model;
using Interfaces;
using JeremyTools;
using Newtonsoft.Json;
using RoslynCompiler;
using UnityEditor;
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
        public UIRoslynNode(Rect rect)
        {
            Node = new MethodNode(new MethodObject
            {
                Target = this,
                Type = typeof(UIRoslynNode),
                MethodName = "DoCompile"
            });

            Out = new UIOutConnectionPoint(new Rect(this.rect.position, new Vector2(50, 50)), new OutConnection(Node));
            Base(rect, "Script Node");
        }

        public void DoCompile()
        {
            switch (output_type_index)
            {
                case 0://INT
                {
                    result = Compile_INT(codeinput).ToString();
                    break;
                }
                case 1://FLOAT
                {
                    result = Compile_FLOAT(codeinput).ToString();
                    break;
                }
                case 2://BOOL
                {
                    result = Compile_BOOL(codeinput).ToString();
                    break;
                }
                case 3://STRING
                {
                    result = Compile_STRING(codeinput);
                    break;
                }
                case 4://OBJECT
                {
                    result = Compile_OBJECT(codeinput).ToString();
                    break;
                }
                default:
                {
                    result = "DEFAULT CASE REACHED, WE SHOULDENT BE HERE...";
                    break;
                }
            }
        }
        ///ToDo: you can have one function handle all of this by passing in a type argument Ex: Compile<T>(string code) with an Evaluate<T> then case on a current enumselection  
        public int Compile_INT(string code)
        {
            return RoslynWrapper.Evaluate<int>(code).Result;
        }

        public float Compile_FLOAT(string code)
        {
            return RoslynWrapper.Evaluate<float>(code).Result;
        }

        public bool Compile_BOOL(string code)
        {
            return RoslynWrapper.Evaluate<bool>(code).Result;
        }

        public string Compile_STRING(string code)
        {
            return RoslynWrapper.Evaluate<string>(code).Result;
        }

        public object Compile_OBJECT(string code)
        {
            return RoslynWrapper.Evaluate<object>(code).Result;
        }

        public override void Draw()
        {
            base.Draw();
            Out.rect = new Rect(rect.position.x + rect.width, rect.position.y, 50, 50);
            Out.Draw();
            GUILayout.BeginArea(rect);

            GUILayout.Space(20);
            ///tryout the EditorGUILayout.EnumPopup
            output_type_index = EditorGUILayout.Popup("OUTPUT TYPE", output_type_index, output_options);
            
            codeinput = GUILayout.TextArea(codeinput);

            EditorGUILayout.LabelField("Result = :: " + result);
            GUILayout.EndArea();
        }

        public UIOutConnectionPoint Out { get; set; }
        public INode Node { get; set; }

        #region Fields

        public string codeinput = "var a = 1; var b = 2; return a + b;";
        public string result = string.Empty;

        //ToDo: you should change these to be an enum like enum OutputType{Int = 0, Float = 1, etc...} doing that will  make the types more strongly typed and meaningful. you could also use the enumpopup selection
        public string[] output_options = {"int", "float", "bool", "string", "object"};
        private int output_type_index;

        #endregion Fields
    }
}