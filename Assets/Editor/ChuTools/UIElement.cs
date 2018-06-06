using System;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public abstract class UIElement : IDrawable, IMouseDownHandler, IMouseUpHandler, IMouseDragHandler
    {
        private readonly string _name;

        private int _controlId;
        public Rect Rect;

        protected UIElement()
        {
            NodeEditorWindow.NodeEvents.OnMouseDown += OnMouseDown;
            NodeEditorWindow.NodeEvents.OnMouseUp += OnMouseUp;
            NodeEditorWindow.NodeEvents.OnMouseDrag += OnMouseDrag;
            ControlId = GUIUtility.GetControlID(FocusType.Passive, Rect);
            Content = new GUIContent(_name + ControlId);
        }

        protected UIElement(string name, Vector2 pos, Vector2 size) : this()
        {
            _name = name;
            Rect = new Rect(pos, size);
            ControlId = GUIUtility.GetControlID(FocusType.Passive, Rect);
            Content = new GUIContent(_name + ControlId);
            SelectedStyle = new GUIStyle("flow node 1 on") {alignment = TextAnchor.LowerLeft, fontSize = 12};
            NormalStyle = new GUIStyle("flow node 1") {alignment = TextAnchor.LowerLeft, fontSize = 12};
            Style = NormalStyle;
        }

        public int ControlId
        {
            get { return _controlId; }
            set
            {
                _controlId = value;
                Content = new GUIContent(_name + ControlId);
            }
        }

        public bool IsSelected { get; private set; }

        public Vector2 Position
        {
            get { return Rect.position; }
            set
            {
                var newp = new Vector2(value.x, value.y);
                Rect.position = newp;
            }
        }

        [JsonIgnore] public GUIStyle SelectedStyle { get; set; }
        [JsonIgnore] public GUIStyle NormalStyle { get; set; }
        [JsonIgnore] public GUIStyle Style { get; set; }
        [JsonIgnore] public GUIContent Content { get; set; }
        Rect IDrawable.Rect => Rect;

        /// <summary>
        ///     Draw the default box for this ui element
        /// </summary>
        public virtual void Draw()
        {
            GUI.Box(Rect, Content, Style);
        }

        public virtual void OnMouseDown(Event e)
        {
            if (Rect.Contains(e.mousePosition))
            {
                IsSelected = true;
                GUIUtility.hotControl = ControlId;
                Style = SelectedStyle;
                GUI.changed = true;
            }
        }

        public virtual void OnMouseDrag(Event e)
        {
            if (GUIUtility.hotControl == ControlId)
            {
                Rect.position += e.delta;
                GUI.changed = true;
                e.Use();
            }
        }

        public virtual void OnMouseUp(Event e)
        {
            IsSelected = false;

            if (GUIUtility.hotControl == ControlId)
            {
                GUIUtility.hotControl = 0;
                Style = NormalStyle;
                GUI.changed = true;
            }
        }
    }
}