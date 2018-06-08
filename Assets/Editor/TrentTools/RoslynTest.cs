using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TrentTools
{
    public class RoslynTest : EditorWindow
    {
		#region Fields
		private static RoslynTest _window;
		#endregion

        public int DoStuff(string code)
        {
            //CANNOT LOAD ASSEMBLY -> Microsoft.CodeAnalysis.CSharp.Scripting
            //var runit = RoslynWrapper.Evaluate<Globals.Transition>(source, globals);
			return RoslynCompiler.RoslynWrapper.Evaluate<int>(code).Result;
        }

		[MenuItem("Tools/TrentTools/RoslynTest")]
        static void Init()
        {
            _window = EditorWindow.GetWindow(typeof(RoslynTest)) as RoslynTest;
			_window.Show();
        }

		/// <summary>
		/// OnGUI is called for rendering and handling GUI events.
		/// This function can be called multiple times per frame (one call per event).
		/// </summary>
		void OnGUI()
		{
			if (GUILayout.Button("TEST ROSLYN"))
			{
                var code = "var a = 1; var b = 2; if(a > 2){return 5;} else{return a * b;}";
				Debug.Log(DoStuff(code));
			}
		}
    }
} 