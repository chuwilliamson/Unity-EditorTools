using ChuTools.NodeEditor.Interfaces;
using ChuTools.NodeEditor.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ChuTools.NodeEditor.Controller
{
    [Serializable]
    public class UIDelegateNode : UIElement
    {
        [NonSerialized] private ReorderableList m_roMethodObjects;

        [JsonConstructor]
        public UIDelegateNode()
        {
            MethodObjects = new MethodObjects { MethodObjectsList = new List<MethodObject>() };
            m_roMethodObjects = new ReorderableList(MethodObjects.MethodObjectsList, typeof(MethodObject), true, true,
                true, true);
            Base(name: "UIDelegate Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on",
                rect: Rect, resize: true);
        }

        public UIDelegateNode(Rect rect)
        {
            Node = new DelegateNode(new InConnection(null));
            In = new UIInConnectionPoint(new Rect(rect.position, new Vector2(15, 15)));
            Base(name: "UIDelegate Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on",
                rect: rect, resize: true);
            ControlId = GUIUtility.GetControlID(FocusType.Passive, Rect);
        }

        public UIInConnectionPoint In { get; set; }

        public MethodObjects MethodObjects { get; set; }

        public INode Node { get; set; }

        private bool DisconnectHandler(UIInConnectionPoint point)
        {
            if (point != In)
                return false;
            Node = null;
            MethodObjects = new MethodObjects { MethodObjectsList = new List<MethodObject>() };
            m_roMethodObjects = new ReorderableList(MethodObjects.MethodObjectsList, typeof(MethodObject), true, true,
                true, true);

            return true;
        }

        public override void Draw()
        {
            base.Draw();

            In.Rect.Set(Rect.x - 25, Rect.y + 25, 25, 25);
            In.Draw();

            GUILayout.BeginArea(Rect);
            if (m_roMethodObjects == null)
            {
                GUILayout.EndArea();
                return;
            }

            EditorGUILayout.BeginVertical();

            m_roMethodObjects?.DoLayoutList();

            EditorGUILayout.EndVertical();

            if (GUILayout.Button("DynamicInvoke"))
                MethodObjects.MethodObjectsList?.ForEach(mo => mo.DynamicInvoke());

            GUILayout.EndArea();
        }

        public bool Connect(IConnectionOut outConnection, UIInConnectionPoint connectionPoint)
        {
            if (outConnection == null)
                return false;
            Node = new DelegateNode(new InConnection(outConnection));
            MethodObjects.MethodObjectsList.Add(Node.Value as MethodObject);
            return true;
        }

        public void DisconnectHandler()
        {
            Node = null;
        }
    }
}