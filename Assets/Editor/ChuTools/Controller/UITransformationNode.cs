﻿using System;
using ChuTools.Model;
using Interfaces;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace ChuTools.Controller
{
    /// <summary>
    ///     This node will represent a pass through node
    ///     It will receive data from it's in connection
    ///     That data will then be changed by the node implementation
    /// </summary>
    [Serializable]
    public class UITransformationNode : UIElement
    {
        [JsonConstructor]
        public UITransformationNode(Rect rect)
        {
            _display = new DisplayNode(null);
            _in = new UIInConnectionPoint(new Rect(this.rect.position, new Vector2(50, 50)), Connect,
                DisconnectResponse);
            _transformation = new InputNode {Value = 0};
            _input = new InputNode {Value = 0};
            _out = new UIOutConnectionPoint(new Rect(this.rect.position, new Vector2(50, 50)),
                new OutConnection(_transformation));
            _in.rect = new Rect(this.rect.position.x - 55, this.rect.position.y, 50, 50);
            _out.rect = new Rect(this.rect.position.x + this.rect.width, this.rect.position.y, 50, 50);

            Base(name: "Transformation", normalStyleName: "flow node 3", selectedStyleName: "flow node 3 on",
                rect: rect, resize: true);
        }

        private bool DisconnectResponse(UIInConnectionPoint point)
        {
            throw new NotImplementedException();
        }

        public bool Connect(IConnectionOut outConnection, UIInConnectionPoint connectionPoint)
        {
            return Connect(outConnection);
        }

        private bool Connect(IConnectionOut outConnection)
        {
            if(outConnection == null) return false;
            _display = new DisplayNode(new InConnection(outConnection));
            return true;
        }

        public override void Draw()
        {
            base.Draw();
            _in.rect = new Rect(this.rect.position.x - 55, this.rect.position.y, 50, 50);
            _out.rect = new Rect(this.rect.position.x + this.rect.width, this.rect.position.y, 50, 50);

            _in?.Draw();
            _out?.Draw();

            GUILayout.BeginArea(this.rect);

            _input.Value = EditorGUILayout.IntSlider("Modifier: ", Convert.ToInt32(_input.Value), 0, 10);

            _transformation.Value = Convert.ToInt32(_display.Value) + Convert.ToInt32(_input.Value);
            GUILayout.Label("Input: " + _input?.Value);
            GUILayout.Label("Display: " + _display?.Value);
            GUILayout.Label("Output: " + _transformation.Value);
            GUILayout.EndArea();
            var rect = new Rect(this.rect.x - 5 + this.rect.width / 2, this.rect.y - 5 + this.rect.height / 2,
                this.rect.width / 2, this.rect.height / 2);
            GUI.Box(rect, GUIContent.none);
            GUI.Label(rect, "ADD", new GUIStyle(Style) {fontSize = 55, alignment = TextAnchor.MiddleCenter});
        }

        public UIInConnectionPoint _in;

        public UIOutConnectionPoint _out;

        public INode _input { get; set; }

        public INode _transformation { get; set; }

        public INode _display { get; set; }
    }
}