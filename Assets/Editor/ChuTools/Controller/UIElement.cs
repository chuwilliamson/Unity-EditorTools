using System;
using ChuTools.View;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace ChuTools.Controller
{
    [Serializable]
    public abstract class UIElement : IDrawable, IMouseDownHandler, IMouseUpHandler, IMouseDragHandler
    {
        protected void Base(Rect rect, string name = "default", string normalStyleName = "flow node 0",
            string selectedStyleName = "flow node 0 on", bool resize = false)
        {
            Name = name;
            Rect = new Rect(rect);
            ControlId = GUIUtility.GetControlID(FocusType.Passive, Rect);
            Content = new GUIContent(Name + ": " + ControlId);
            NormalStyle = new GUIStyle(normalStyleName) {alignment = TextAnchor.LowerLeft, fontSize = 10};
            SelectedStyle = new GUIStyle(selectedStyleName) {alignment = TextAnchor.LowerLeft, fontSize = 10};
            Style = NormalStyle;
            NodeEditorWindow.NodeEventSystem.OnMouseDown += OnMouseDown;
            NodeEditorWindow.NodeEventSystem.OnMouseUp += OnMouseUp;
            NodeEditorWindow.NodeEventSystem.OnMouseDrag += OnMouseDrag;
            NodeEditorWindow.NodeEventSystem.OnMouseMove += OnMouseMove;
            NodeEditorWindow.NodeEventSystem.OnDragExited += OnDragExited;
            NodeEditorWindow.NodeEventSystem.OnDragPerform += OnDragPerform;
            NodeEditorWindow.NodeEventSystem.OnDragUpdated += OnDragUpdated;
            NodeEditorWindow.NodeEventSystem.OnContextClick += OnContextClick;


            Resize = resize;
        }

        protected virtual void OnMouseMove(Event e)
        {
        }

        protected virtual void OnContextClick(Event e)
        {
        }

        protected virtual void OnDragExited(Event e)
        {
        }

        protected virtual void OnDragPerform(Event e)
        {
        }

        protected virtual void OnDragUpdated(Event e)
        {
        }

        /// <summary>
        ///     Draw the default box for this ui element
        /// </summary>
        public virtual void Draw()
        {
            Content = new GUIContent(Name + ": " + ControlId);

            GUI.Box(Rect, Content, Style);
            if(Resize)
            {
                GUI.Box(DragRect, GUIContent.none);
                DragId = GUIUtility.GetControlID(FocusType.Passive, DragRect);
            }
        }

        Rect IDrawable.Rect => Rect;

        public virtual void OnMouseDown(Event e)
        {
            if(DragRect.Contains(e.mousePosition) && Resize)
            {
                GUIUtility.hotControl = DragId;
                GUI.changed = true;
                _resizing = true;
            }

            if(Rect.Contains(e.mousePosition) && !_resizing)
            {
                GUIUtility.hotControl = ControlId;
                Style = SelectedStyle;
                GUI.changed = true;
            }
        }

        public virtual void OnMouseDrag(Event e)
        {
            if(GUIUtility.hotControl == DragId && Resize)
            {
                Rect = new Rect(Rect.position, Rect.size + e.delta);
                GUI.changed = true;
                e.Use();
            }

            if(GUIUtility.hotControl == ControlId)
            {
                Rect = new Rect(Rect.position + e.delta, Rect.size);
                GUI.changed = true;
                e.Use();
            }
        }

        public virtual void OnMouseUp(Event e)
        {
            if(GUIUtility.hotControl == ControlId)
            {
                GUIUtility.hotControl = 0;
                Style = NormalStyle;
                GUI.changed = true;
            }

            if(GUIUtility.hotControl == DragId && Resize)
            {
                GUIUtility.hotControl = 0;
                Style = NormalStyle;
                GUI.changed = true;
                _resizing = false;
            }
        }

        public int DragId;

        public Rect Rect;

        private bool _resizing;

        public bool Resize { get; set; }

        private Rect DragRect => new Rect(new Vector2(Rect.xMax - 15, Rect.yMax - 15), new Vector2(15, 15));

        public string Name { get; set; }

        public int ControlId { get; set; }

        [JsonIgnore]
        public GUIStyle SelectedStyle { get; set; }

        [JsonIgnore]
        public GUIStyle NormalStyle { get; set; }

        [JsonIgnore]
        public GUIStyle Style { get; set; }

        [JsonIgnore]
        public GUIContent Content { get; set; }
    }
}