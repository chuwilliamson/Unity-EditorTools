using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ChuTools;
using Interfaces;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace JeremyTools
{
    public class UIDelegateNode : UIElement
    {
        private readonly ReorderableList _roMethodObjects;
        public List<MethodObject> MethodObjects;
        public UIInConnectionPoint In;

        public INode Node { get; set; }

        public UIDelegateNode(Rect rect)
        {
            ControlId = GUIUtility.GetControlID(FocusType.Passive, base.rect);
            Node = new DelegateNode(new InConnection(null));
            In = new UIInConnectionPoint(new Rect(this.rect.position, new Vector2(x: 5, y: 50)), Connect);
            MethodObjects = new List<MethodObject>();
            _roMethodObjects = new ReorderableList(MethodObjects, typeof(MethodObject), true, true, true, true);
            Base(name: "Delegate Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on", rect: rect);
        }

        public override void Draw()
        {
            GUI.Box(rect, Content, style: NormalStyle);
            In.rect = new Rect(x: rect.position.x - 55, y: rect.position.y, width: 50, height: 50);
            In?.Draw();
            if (MethodObjects.Count < 1)
                return;
            GUILayout.BeginArea(rect);

            EditorGUILayout.LabelField("no methods");
            EditorGUILayout.BeginVertical();
            _roMethodObjects?.DoLayoutList();
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("Invoke")) { MethodObjects.ForEach(mo => mo.Invoke()); }
            GUILayout.EndArea();
        }

        public static void AddMethod(object sender, MethodInfo methodInfo) { }
        protected virtual bool Connect(IConnectionOut outConnection)
        {
            if (outConnection == null)
                return false;
            Node = new DelegateNode(new InConnection(outConnection));
            MethodObjects.Add(Node.Value as MethodObject);
            return true;
        }

        public virtual void Disconnect()
        {
            Node = null;
        }
    }
}
