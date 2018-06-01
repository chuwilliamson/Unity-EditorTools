using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ChuTools
{
    public partial class Node : UIElement
    {

        private readonly Action<Node> _onRemoveNodeAction;
        private readonly int _propid;

        private Vector2 _scrollPosition;

        public bool IsSelected
        {
            get { return GUIUtility.hotControl == ControlId; }
        }

        public bool IsHovered
        {
            get { return GUIUtility.hotControl == ControlId && Rect.Contains(NodeEditorWindow.Current.mousePosition); }
        }
    }
}