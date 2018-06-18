using System;
using System.Collections;
using System.Collections.Generic;
using EditorInterfaces;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace BackpackViewerWindow
{
    public class BackpackViewer : IDrawable, IResizable
    {
        private Rect Rect;
        public Rect _Rect
        {
            get { return Rect; }
        }
        
        private BackpackScriptable Data;
        public BackpackScriptable _Data
        {
            get { return Data; }
            set { Data = value; }
        }

        private List<Rect> Slots;
        public List<Rect> _Slots
        {
            get { return Slots; }
        }

        private Rect ResizeRect;
        public Rect DragRect
        {
            get { return ResizeRect; }
        }

        private bool isResizeable;
        public bool IsResizable
        {
            get { return isResizeable; }
        }

        private Vector2 Size;
        public Vector2 Scale
        {
            get { return Size; }
        }

        public BackpackViewer()
        {
            Size = new Vector2(250,250);
        }

        public void Draw()
        {
            Slots = new List<Rect>();
            Rect = new Rect(new Vector2(0, 25), Size);
            GUI.Box(Rect,"");
            if (Data != null)
            {
                for (int i = 0; i < Data._Capacity; i++)
                {
                    var newSlot = new Rect(Rect);
                    newSlot.size = new Vector2(25,25);
                    if (Slots.Count > 0)
                    {
                        newSlot.position = Rect.position;
                        newSlot.position = Slots[i - 1].position + new Vector2(30,0);
                        if (newSlot.xMax >= Rect.xMax)
                        {
                            newSlot.position = new Vector2(Rect.position.x, Slots[i - 1].position.y + 30);
                        }
                    }
                    Slots.Add(newSlot);
                    GUI.backgroundColor = Color.blue;
                    GUI.Box(newSlot, "");                    
                }
            }
            ResizeRect = new Rect(new Vector2(Rect.xMax - 15, Rect.yMax - 15), new Vector2(10, 10));
            GUI.backgroundColor = Color.white;
            GUI.Box(ResizeRect, "");
        }   

        public void EnableResize()
        {            
            if (Event.current.button == 0)
            {
                if (ResizeRect.Contains(Event.current.mousePosition))
                {
                    isResizeable = true;
                }
                Event.current.Use();
            }
        }

        public void Resize()
        {
            var WindowSize = new Vector2();
            if (EditorWindow.focusedWindow != null)
                WindowSize = EditorWindow.focusedWindow.position.size;
            if (isResizeable)
            {
                if (Event.current.delta.x < 0 && Rect.xMax > 0)
                {
                    Size.x += Event.current.delta.x;
                }

                if (Event.current.delta.x > 0 && Rect.position.x + Rect.width < WindowSize.x)
                {
                    Size.x += Event.current.delta.x;
                }

                if (Event.current.delta.y < 0 && Rect.yMax > 0)
                {
                    Size.y += Event.current.delta.y;
                }

                if (Event.current.delta.y > 0 && Rect.position.y + Rect.height < WindowSize.y)
                {
                    Size.y += Event.current.delta.y;
                }
                Event.current.Use();
            }
        }

        public void DisableResize()
        {
            isResizeable = false;
        }
    }
}