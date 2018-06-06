using System;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace ChuTools
{
    [Serializable]
    public abstract class UIElement : IDrawable, IMouseDownHandler, IMouseUpHandler, IMouseDragHandler
    {
        private int _controlId;
        private Rect _uRect;
        public string Name { get; set; }

        public Rect uRect
        {
            get { return _uRect; }
            set { _uRect = value; }
        }

        public int ControlId
        {
            get { return _controlId; }
            set { _controlId = value; }
        }

        [JsonIgnore] public GUIStyle SelectedStyle { get; set; }
        [JsonIgnore] public GUIStyle NormalStyle { get; set; }
        [JsonIgnore] public GUIStyle Style { get; set; }
        [JsonIgnore] public GUIContent Content { get; set; }
        public Rect Rect
        {
            get { return uRect; }
        }

        public void Base(string name, string normalStyleName, string selectedStyleName, Rect rect)
        {
            Name = name;
            uRect = new Rect(rect);
            _controlId = GUIUtility.GetControlID(FocusType.Passive, uRect);
            Content = new GUIContent(Name + ": " + ControlId);
            NormalStyle = new GUIStyle(normalStyleName) { alignment = TextAnchor.LowerLeft, fontSize = 10 };
            SelectedStyle = new GUIStyle(selectedStyleName) { alignment = TextAnchor.LowerLeft, fontSize = 10 };
            Style = NormalStyle;
            NodeEditorWindow.NodeEvents.OnMouseDown += OnMouseDown;
            NodeEditorWindow.NodeEvents.OnMouseUp += OnMouseUp;
            NodeEditorWindow.NodeEvents.OnMouseDrag += OnMouseDrag;
         
        }
 

        /// <summary>
        ///     Draw the default box for this ui element
        /// </summary>
        public virtual void Draw()
        {
            Content = new GUIContent(Name + ": " + ControlId);
            GUI.Box(uRect, Content, Style);
        }

        public virtual void OnMouseDown(Event e)
        {
            if (uRect.Contains(e.mousePosition))
            {
                GUIUtility.hotControl = ControlId;
                Style = SelectedStyle;
                GUI.changed = true;
            }
        }

        public virtual void OnMouseDrag(Event e)
        {
            if (GUIUtility.hotControl == ControlId)
            {
                uRect = new Rect(uRect.position + e.delta, uRect.size);
                GUI.changed = true;
                e.Use();
            }
        }

        public virtual void OnMouseUp(Event e)
        {
            if (GUIUtility.hotControl == ControlId)
            {
                GUIUtility.hotControl = 0;
                Style = NormalStyle;
                GUI.changed = true;
            }
        }
    }
}