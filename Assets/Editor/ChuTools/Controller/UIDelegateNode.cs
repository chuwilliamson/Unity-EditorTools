using System;
using System.Collections.Generic;
using ChuTools.Controller;
using ChuTools.Model;
using ChuTools.View;
using Interfaces;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace JeremyTools
{
    [Serializable]
    public class UIDelegateNode : UIElement
    {
        [JsonConstructor]
        public UIDelegateNode()
        {
            MethodObjects = new MethodObjects { MethodObjectsList = new List<MethodObject>() };
            _roMethodObjects = new ReorderableList(MethodObjects.MethodObjectsList, typeof(MethodObject), true, true, true, true);
            Base(name: "UIDelegate Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on", rect: rect, resize: true);
          
        }

        public UIDelegateNode(Rect rect) : this()
        {
            Node = new DelegateNode(new InConnection(null));
            In = new UIInConnectionPoint(new Rect(this.rect.position, new Vector2(5, 50)), Connect, Disconnect);
            Base(name: "UIDelegate Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on", rect: rect, resize: true);
            ControlId = GUIUtility.GetControlID(FocusType.Passive, this.rect);
        }

        private bool Disconnect(UIInConnectionPoint point)
        {
            if (point != In)
                return false;
            Node = null;
            MethodObjects = new MethodObjects { MethodObjectsList = new List<MethodObject>() };
            _roMethodObjects = new ReorderableList(MethodObjects.MethodObjectsList, typeof(MethodObject), true, true, true, true);
            
            return true;
        }

        public override void Draw()
        {
            base.Draw();

            In.rect = new Rect(rect.position.x - 55, rect.position.y, 50, 50);
            In?.Draw();

            GUILayout.BeginArea(rect);
            if (_roMethodObjects == null)
            {
                GUILayout.EndArea();
                return;
            }

            EditorGUILayout.BeginVertical();

            _roMethodObjects?.DoLayoutList();

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

        public void Disconnect()
        {
            Node = null;
        }

        [NonSerialized] private ReorderableList _roMethodObjects;

        public UIInConnectionPoint In { get; set; }

        public MethodObjects MethodObjects { get; set; }

        public INode Node { get; set; }
    }
}