using System;
using System.Collections.Generic;
using ChuTools.CustomInspectors;
using ChuTools.NodeEditor.Interfaces;
using ChuTools.NodeEditor.Model;
using ChuTools.Scripts;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ChuTools.NodeEditor.Controller
{
    [Serializable]
    public class UIMultiDelegateNode : UIElement
    {
        [JsonConstructor]
        public UIMultiDelegateNode()
        {
            MethodObjects = new List<MethodObject>();
            RoMethodObjects = new ReorderableList(MethodObjects, typeof(MethodObject), true, true, false, false);
            Base(name: "UIDelegate Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on",
                rect: Rect, resize: true);
            View.NodeEditor.NodeEventSystem.OnDragPerform += OnDragPerform;
        }

        public UIMultiDelegateNode(Rect rect)
        {
            InConnectionPoints = new List<UIInConnectionPoint>
            {
                new UIInConnectionPoint(new Rect(rect.position, new Vector2(15, 15)), Connect, DisconnectHandler),
                new UIInConnectionPoint(new Rect(rect.position, new Vector2(15, 15)), Connect, DisconnectHandler),
                new UIInConnectionPoint(new Rect(rect.position, new Vector2(15, 15)), Connect, DisconnectHandler)
            };

            Nodes = new List<INode>
            {
                new DelegateNode(new InConnection(null)),
                new DelegateNode(new InConnection(null)),
                new DelegateNode(new InConnection(null))
            };

            MethodObjects = new List<MethodObject>();
            RoMethodObjects = new ReorderableList(MethodObjects, typeof(MethodObject), true, true, false, false);

            Base(name: "UIDelegate Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on",
                rect: rect, resize: true);

            ControlId = GUIUtility.GetControlID(FocusType.Passive, rect);
        }


        private ReorderableList RoMethodObjects { get; }
        public object ResultObject { get; set; }
        public List<MethodObject> MethodObjects { get; set; }
        public List<INode> Nodes { get; set; }
        public List<UIInConnectionPoint> InConnectionPoints { get; set; }

        protected override void OnDragPerform(Event @event)
        {
            var data = DragAndDrop.GetGenericData("OutData");
            var ui = DragAndDrop.GetGenericData("UIInConnectionPoint");
            Connect(data as IConnectionOut, ui as UIInConnectionPoint);
        }

        public override void Draw()
        {
            base.Draw();

            for (var i = 0; i < InConnectionPoints.Count; i++)
            {
                var re = InConnectionPoints[i].Rect;
                InConnectionPoints[i].Rect.Set(Rect.x - 25, Rect.y + 25 * i, re.width, re.height);
                InConnectionPoints[i].Draw();
            }

            GUILayout.BeginArea(Rect);
            if (RoMethodObjects == null)
            {
                GUILayout.EndArea();
                return;
            }

            EditorGUILayout.BeginVertical();

            RoMethodObjects?.DoLayoutList();

            EditorGUILayout.EndVertical();
            EditorGUILayout.LabelField(typeof(CallbackBehaviour).FullName);

            if (GUILayout.Button("DynamicInvoke"))
                MethodObjects.ForEach(mo => { mo.DynamicInvoke(); });


            if (MethodObjects.Count > 0)
            {
                var props = MethodObjects[0]?.Target?.GetType().GetProperties();
                var resultProperty = MethodObjects[0]?.Target?.GetType().GetProperty("ResultTuple");
                var resultValue = resultProperty?.GetValue(MethodObjects[0].Target);
                var resultTuple = resultValue as Tuple<string, object>;
                ResultObject = resultTuple?.Item2;

                EditorGUILayout.LabelField(ResultObject?.ToString());
                CallbackBehaviourEditor.DrawArray(props);
                EditorGUILayout.Space();
            }

            ResultObject = EditorGUILayout.ObjectField(ResultObject as GameObject, typeof(GameObject), true);
            GUILayout.EndArea();
        }


        public bool Connect(IConnectionOut outConnection, UIInConnectionPoint inConnectionPoint)
        {
            if (outConnection == null)
                return false;
            var node = new DelegateNode(new InConnection(outConnection));
            MethodObjects.Add(node.Value as MethodObject);
            return true;
        }

        public bool DisconnectHandler(UIInConnectionPoint inConnectionPoint)
        {
            if (!InConnectionPoints.Contains(inConnectionPoint))
                return false;
            var index = InConnectionPoints.IndexOf(inConnectionPoint);
            Nodes.RemoveAt(index);
            MethodObjects.RemoveAt(index);
            return true;
        }
    }
}