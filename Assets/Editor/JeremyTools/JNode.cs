using Interfaces;
using UnityEditor;
using UnityEngine;

namespace JeremyTools
{
    public partial class JNode 
    {
        // fields
        public ChuTools.IEventSystem EventSystem { get; set; }

        Rect OutRect
        {
            get
            {
                return new Rect(new Vector2(rect.xMax, (rect.center.y - (25 / 2))), new Vector3(25, 25));
            }
        }

        Rect InRect
        {
            get { return new Rect(new Vector2(rect.xMin - 25, (rect.center.y - (25 / 2))), new Vector3(25, 25)); }
        }

        public Vector2 OutCenter
        {
            get
            {
                return OutRect.center;
            }
        }

        public Vector2 InCenter
        {
            get
            {
                return InRect.center;
            }
        }

        public Rect rect;
        public ConnectionPoint outPoint, inPoint;
        public GUIContent content;
        public GUIStyle style;
        private System.Action<JNode> _onNodeDelete;
        public bool isSelected;

        //methods
        public JNode(Rect r, GUIContent c, GUIStyle s, ChuTools.IEventSystem eventSystem, System.Action<JNode> onNodeDelete) : this(r, c, s)
        {
            EventSystem = eventSystem;
            EventSystem.OnMouseDown += OnMouseDown;
            _onNodeDelete = onNodeDelete;
            EventSystem.OnContextClick += onContextClick;
            EventSystem.OnMouseDrag += OnMouseDrag;

            outPoint = new ConnectionPoint(OutRect, "out");
            inPoint = new ConnectionPoint(InRect, "in");
        }

        public JNode(Rect r, GUIContent c, GUIStyle s)
        {
            rect = r;
            content = c;
            style = s;
        }

        ~JNode()
        {
            EventSystem.OnMouseDown -= OnMouseDown;
            EventSystem.OnContextClick -= onContextClick;
            EventSystem = null;
        }

        public void onContextClick(Event e)
        {
            if (!rect.Contains(e.mousePosition)) return;
            var gm = new GenericMenu();
            gm.AddItem(new GUIContent("Delete Node"), false, () =>
            {
                EventSystem.OnMouseDown -= OnMouseDown;
                EventSystem.OnContextClick -= onContextClick;
                _onNodeDelete(this);
            });
            gm.ShowAsContext();
            GUI.changed = true;
        }

        public void OnMouseDown(Event e)
        {
            if (e.button == 0)
            {
                if (rect.Contains(e.mousePosition))
                {
                    Debug.Log("Left Down Node");
                    GUI.changed = true;
                }
            }
        }

        public void OnMouseUp(Event e)
        {
            if (e.button == 0)
            {
                Debug.Log("Left Up Node");
                GUI.changed = true;
            }
        }

        public void OnMouseDrag(Event e)
        {
            if (isSelected)
            {
                rect.position += e.delta;
                inPoint.rect.position += e.delta;
                outPoint.rect.position += e.delta;
            }
        }

    }
}