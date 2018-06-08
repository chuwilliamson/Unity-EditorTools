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
            result = Compile(codeinput).ToString();

        }
        public int Compile(string code)
        {
            //CANNOT LOAD ASSEMBLY -> Microsoft.CodeAnalysis.CSharp.Scripting
            //var runit = RoslynWrapper.Evaluate<Globals.Transition>(source, globals);
            return RoslynCompiler.RoslynWrapper.Evaluate<int>(code).Result;
        }

        public string codeinput = "var a = 1; var b = 2; if(a > 2){return 5;} else{return a * b;}";
        public string result = string.Empty;
        public override void Draw()
        {
            base.Draw();
            Out.rect = new Rect(rect.position.x + rect.width, rect.position.y, 50, 50);
            Out?.Draw();
            GUILayout.BeginArea(base.rect);
            GUILayout.Space(35);
            codeinput = GUILayout.TextArea(codeinput);
            if (GUILayout.Button("TEST ROSLYN"))
            {
                result = Compile(codeinput).ToString();
            }
            EditorGUILayout.LabelField("Result = :: " + result);
            GUILayout.EndArea();
        }

    }
}