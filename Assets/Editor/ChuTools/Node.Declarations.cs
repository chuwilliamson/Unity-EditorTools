using System;
using UnityEngine;

namespace ChuTools
{
    public partial class Node
    {
        private readonly Action<Node> _onRemoveNodeAction;
        private readonly int _propid;
        private Vector2 _scrollPosition;

        public bool IsHovered =>
            GUIUtility.hotControl == ControlId && Rect.Contains(NodeEditorWindow.Current.mousePosition);
    }
}