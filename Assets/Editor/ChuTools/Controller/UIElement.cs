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
            this.rect = new Rect(rect);
            ControlId = GUIUtility.GetControlID(FocusType.Passive, this.rect);
            Content = new GUIContent(Name + ": " + ControlId);
            NormalStyle = new GUIStyle(normalStyleName) {alignment = TextAnchor.LowerLeft, fontSize = 10};
            SelectedStyle = new GUIStyle(selectedStyleName) {alignment = TextAnchor.LowerLeft, fontSize = 10};
            Style = NormalStyle;
            NodeEditorWindow.NodeEvents.OnMouseDown += OnMouseDown;
            NodeEditorWindow.NodeEvents.OnMouseUp += OnMouseUp;
            NodeEditorWindow.NodeEvents.OnMouseDrag += OnMouseDrag;
            NodeEditorWindow.NodeEvents.OnDragExited += OnDragExited;
            NodeEditorWindow.NodeEvents.OnContextClick += OnContextClick;
            NodeEditorWindow.NodeEvents.OnMouseMove += OnMouseMove;
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

        /// <summary>
        ///     Draw the default box for this ui element
        /// </summary>
        public virtual void Draw()
        {
            Content = new GUIContent(Name + ": " + ControlId);

            GUI.Box(rect, Content, Style);
            if(Resize)
            {
                GUI.Box(DragRect, GUIContent.none);
                DragID = GUIUtility.GetControlID(FocusType.Passive, DragRect);
            }
        }

        Rect IDrawable.Rect => rect;

        public virtual void OnMouseDown(Event e)
        {
            if(DragRect.Contains(e.mousePosition) && Resize)
            {
                GUIUtility.hotControl = DragID;
                GUI.changed = true;
                resizing = true;
            }

            if(rect.Contains(e.mousePosition) && !resizing)
            {
                GUIUtility.hotControl = ControlId;
                Style = SelectedStyle;
                GUI.changed = true;
            }
        }

        public virtual void OnMouseDrag(Event e)
        {
            if(GUIUtility.hotControl == DragID && Resize)
            {
                rect = new Rect(rect.position, rect.size + e.delta);
                GUI.changed = true;
                e.Use();
            }

            if(GUIUtility.hotControl == ControlId)
            {
                rect = new Rect(rect.position + e.delta, rect.size);
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

            if(GUIUtility.hotControl == DragID && Resize)
            {
                GUIUtility.hotControl = 0;
                Style = NormalStyle;
                GUI.changed = true;
                resizing = false;
            }
        }

        public int DragID;

        public Rect rect;

        private bool resizing;

        public bool Resize { get; set; }

        private Rect DragRect => new Rect(new Vector2(rect.xMax - 15, rect.yMax - 15), new Vector2(15, 15));

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