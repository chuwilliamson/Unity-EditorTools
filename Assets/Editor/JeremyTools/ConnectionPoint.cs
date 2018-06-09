using UnityEngine;

namespace JeremyTools
{
    public class ConnectionPoint
    {
        public ConnectionPoint(Rect r, string n)
        {
            rect = r;
            name = n;
        }

        public void Draw()
        {
            GUI.Box(rect, new GUIContent(name, name));
        }

        public string name;
        public Rect rect;
    }
}