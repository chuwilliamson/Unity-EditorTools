using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChuTools
{
    public partial class Node : UIElement
    {
        public bool IsSelected { get; set; }
        public bool IsHovered { get; set; }
        public static GUIStyle NodeStyle = new GUIStyle
        {
            normal = {background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D
            },
            border = new RectOffset(12, 12, 12, 12)
        };

        public static GUIStyle SelectedNodeStyle = new GUIStyle
        {
            normal =
            {
                background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D
            },
            border = new RectOffset(12, 12, 12, 12)
        };

        public static GUIStyle InPointStyle = new GUIStyle
        {
            normal =
            {
                background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D
            },
            hover = 
            {
                background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D
            },
            border = new RectOffset(4, 4, 12, 12)
        };

        public static GUIStyle OutPointStyle = new GUIStyle
        {
            normal =
            {
                background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D
            },
            active =
            {
                background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D
            },
            border = new RectOffset(4, 4, 12, 12)
        };
        public static GUIStyle NormalStyle =
            new GUIStyle("flow node 0")
            {
                normal = { textColor = Color.white },
                padding = new RectOffset(5, 5, 5, 5)
            };


        public static GUIStyle SelectedStyle = new GUIStyle("flow node 0 on")
        {
            normal = { textColor = Color.green },
            padding = new RectOffset(5, 5, 5, 5)
        };

        private int _controlId;
        private readonly int _propid;
        private GUIStyle _currentStyle;
        private Vector2 _scrollPosition;
        public int Id;
        public Object Data;

        private readonly Action<ChuTools.Node> _onRemoveNodeAction;
    }
}