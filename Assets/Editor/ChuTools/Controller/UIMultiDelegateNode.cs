using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChuTools.Controller;
using ChuTools.Model;
using Interfaces;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = System.Object;

namespace JeremyTools
{
    [Serializable]
    public class UIMultiDelegateNode : UIElement
    {
        public List<object> results = new List<object>();
        private FieldInfo info;

        [JsonConstructor]
        public UIMultiDelegateNode()
        {
            MethodObjects = new List<MethodObject>();
            _roMethodObjects = new ReorderableList(MethodObjects, typeof(MethodObject), true, true, false, false);
            Base(name: "UIDelegate Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on",
                rect: rect, resize: true);
        }

        public UIMultiDelegateNode(Rect rect)
        {
            InConnectionPoints = new List<UIInConnectionPoint>
            {
                new UIInConnectionPoint(string.Empty, "U2D.pivotDot", "U2D.pivotDotActive",
                    new Rect(rect.position, new Vector2(15, 15)), Connect, DisconnectHandler),
                new UIInConnectionPoint(string.Empty, "U2D.pivotDot", "U2D.pivotDotActive",
                    new Rect(rect.position, new Vector2(15, 15)), Connect, DisconnectHandler),
                new UIInConnectionPoint(string.Empty, "U2D.pivotDot", "U2D.pivotDotActive",
                    new Rect(rect.position, new Vector2(15, 15)), Connect, DisconnectHandler)
            };

            Nodes = new List<INode>
            {
                new DelegateNode(new InConnection(null)),
                new DelegateNode(new InConnection(null)),
                new DelegateNode(new InConnection(null))
            };

            MethodObjects = new List<MethodObject>();
            _roMethodObjects = new ReorderableList(MethodObjects, typeof(MethodObject), true, true, false, false);

            Base(name: "UIDelegate Node", normalStyleName: "flow node 2", selectedStyleName: "flow node 2 on",
                rect: rect, resize: true);

            ControlId = GUIUtility.GetControlID(FocusType.Passive, rect);
        }

        public override void Draw()
        {
            base.Draw();

            for (var i = 0; i < InConnectionPoints.Count; i++)
            {
                InConnectionPoints[i].rect.Set(rect.x - 25, rect.y + 25 * i, 25, 25);
                InConnectionPoints[i].Draw();
            }

            GUILayout.BeginArea(rect);
            if (_roMethodObjects == null)
            {
                GUILayout.EndArea();
                return;
            }

            EditorGUILayout.BeginVertical();

            _roMethodObjects?.DoLayoutList();

            EditorGUILayout.EndVertical();
            EditorGUILayout.LabelField(typeof(CallbackBehaviour).FullName);

            if (GUILayout.Button("DynamicInvoke"))
            {

                MethodObjects.ForEach(mo =>
                {
                    mo.DynamicInvoke();

                });
            }
                

            if (MethodObjects.Count > 0)
            {
                var props = MethodObjects[0]?.Target?.GetType().GetProperties();
                var resultProperty = MethodObjects[0]?.Target?.GetType().GetProperty("ResultTuple");
                var resultValue = resultProperty?.GetValue(MethodObjects[0].Target);
                var resultTuple = resultValue as Tuple<string, object>;
                resultObject = resultTuple?.Item2;

                EditorGUILayout.LabelField(resultObject?.ToString());
                EditorCallbackBehaviour.DrawArray(props);
                EditorGUILayout.Space();
            }

            resultObject = EditorGUILayout.ObjectField(resultObject as GameObject, typeof(GameObject), true);
            GUILayout.EndArea();
        }

        [NonSerialized]
        public object resultObject;


 

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



        [NonSerialized] private readonly ReorderableList _roMethodObjects;

        public List<MethodObject> MethodObjects { get; set; }

        public List<INode> Nodes { get; set; }

        public List<UIInConnectionPoint> InConnectionPoints { get; set; }
    }
}