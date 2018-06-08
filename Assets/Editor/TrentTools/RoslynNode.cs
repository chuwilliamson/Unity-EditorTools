using System;
using ChuTools;
using JeremyTools;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TrentTools
{
    [System.Serializable]
    public class RoslynNode : UIMethodNode
    {
        #region Fields
        public string codeinput = "var a = 1; var b = 2; return a + b;";
        public string result = string.Empty;

        public string[] output_options = new string[] { "int", "float", "bool", "string", "object" };
        private int output_type_index = 0;
        #endregion

        public RoslynNode(Rect @rect)
        {
            Node = new MethodNode(new MethodObject
            {
                Target = this,
                Type = typeof(RoslynNode),
                MethodName = "DoCompile"
            });

            Out = new UIOutConnectionPoint(new Rect(this.rect.position, new Vector2(50, 50)), new OutConnection(Node));
            Base(rect, "Script Node");
        }

        public void DoCompile()
        {
            switch(output_type_index)
            {
                case 0: //INT
                    {
                        result = Compile_INT(codeinput).ToString();
                        break;
                    }
                case 1: //FLOAT
                    {
                        result = Compile_FLOAT(codeinput).ToString();
                        break;
                    }
                case 2: //BOOL
                    {
                        result = Compile_BOOL(codeinput).ToString();
                        break;
                    }
                case 3: //STRING
                    {
                        result = Compile_STRING(codeinput).ToString();
                        break;
                    }
                case 4: //OBJECT
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

        public int Compile_INT(string code)
        {
            return RoslynCompiler.RoslynWrapper.Evaluate<int>(code).Result;
        }
        public float Compile_FLOAT(string code)
        {
            return RoslynCompiler.RoslynWrapper.Evaluate<float>(code).Result;
        }
        public bool Compile_BOOL(string code)
        {
            return RoslynCompiler.RoslynWrapper.Evaluate<bool>(code).Result;
        }
        public string Compile_STRING(string code)
        {
            return RoslynCompiler.RoslynWrapper.Evaluate<string>(code).Result;
        }
        public object Compile_OBJECT(string code)
        {
            return RoslynCompiler.RoslynWrapper.Evaluate<object>(code).Result;
        }

        public override void Draw()
        {
            base.Draw();
            Out.rect = new Rect(rect.position.x + rect.width, rect.position.y, 50, 50);
            Out?.Draw();

            GUILayout.BeginArea(base.rect);

            GUILayout.Space(20);

            output_type_index = EditorGUILayout.Popup("OUTPUT TYPE", output_type_index, output_options);

            codeinput = GUILayout.TextArea(codeinput);
            
            EditorGUILayout.LabelField("Result = :: " + result);
            GUILayout.EndArea();
        }

    }
}