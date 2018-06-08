using System;
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
    [System.Serializable]
    public class UIDelegateNode : UIElement
    {
        [NonSerialized]
        private readonly ReorderableList _roMethodObjects;

        public MethodObjects MethodObjects;
        public UIInConnectionPoint In;

        public INode Node { get; set; }

        protected UIDelegateNode()
        {

            Node = new DelegateNode(new InConnection(null));
            In = new UIInConnectionPoint(new Rect(base.rect.position, new Vector2(x: 5, y: 50)), Connect);
            MethodObjects = new MethodObjects { MethodObjectsList = new List<MethodObject>() };
            _roMethodObjects = new ReorderableList(MethodObjects.MethodObjectsList, typeof(MethodObject), true, true, true, true);
            ControlId = GUIUtility.GetControlID(FocusType.Passive, base.rect);
            Base(name: "UIDelegate Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on", rect: rect);
        }

        public UIDelegateNode(Rect rect)
        {

            Node = new DelegateNode(new InConnection(null));
            In = new UIInConnectionPoint(new Rect(base.rect.position, new Vector2(x: 5, y: 50)), Connect);
            MethodObjects = new MethodObjects { MethodObjectsList = new List<MethodObject>() };
            _roMethodObjects = new ReorderableList(MethodObjects.MethodObjectsList, typeof(MethodObject), true, true, true, true);
            ControlId = GUIUtility.GetControlID(FocusType.Passive, base.rect);
            Base(name: "UIDelegate Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on", rect: rect);

        }

        public override void Draw()
        {
            GUI.Box(rect, Content, style: NormalStyle);
            In.rect = new Rect(x: rect.position.x - 55, y: rect.position.y, width: 50, height: 50);
            In?.Draw();


            GUILayout.BeginArea(rect);


            EditorGUILayout.BeginVertical();
            _roMethodObjects?.DoLayoutList();
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("DynamicInvoke")) { MethodObjects.MethodObjectsList.ForEach(mo => mo.DynamicInvoke()); }
            GUILayout.EndArea();
        }

        protected virtual bool Connect(IConnectionOut outConnection)
        {
            if (outConnection == null)
                return false;
            Node = new DelegateNode(new InConnection(outConnection));
            MethodObjects.MethodObjectsList.Add(Node.Value as MethodObject);
            return true;
        }

        public virtual void Disconnect()
        {
            Node = null;
        }
    }
}
