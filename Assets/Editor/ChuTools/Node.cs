
using UnityEngine;
using Object = UnityEngine.Object;
namespace ChuTools
{
    public class Node
    {
        private bool active;
        public Rect rect;
        
        GUIStyle normal = new GUIStyle("flow node 0");
        GUIStyle selected = new GUIStyle("flow node 0 on");
        GUIStyle currentStyle;
        public Object[] Data;

        public Node()
        {         
            currentStyle = normal;            
        }

        public Node(Vector2 position) : this()
        { 
            rect = new Rect(position, new Vector2(150, 50));                        
        }
 
        public void Draw(Event e)
        {
            var botrect = rect;
            botrect.position = new Vector2(rect.position.x, rect.position.y + rect.height);

            GUI.Box(rect, new GUIContent { text = rect.position.ToString() }, currentStyle);
            var guistyle = new GUIStyle();
            guistyle.normal.textColor = Color.white;
            GUI.Box(botrect, rect.position.ToString(), guistyle);
        }

        public void OnMouseDrag(Event e)
        {
            if (!active) return;

            var newposition = rect.position + e.delta;

            if (newposition.x < 0)//left
                return;
            if (newposition.y < 0)//top
                return;

            if (newposition.x > Screen.width - rect.width)//right
                return;
            if (newposition.y > Screen.height - rect.height)//bottom
                return;


            rect.position = newposition;
        }

        public void OnMouseDown(Event e)
        {
            if (!rect.Contains(e.mousePosition))
                return;
            active = true;
            currentStyle = selected;
        }

        public void OnMouseUp(Event e)
        {
            active = false;
            currentStyle = normal; 
        }
    }
}