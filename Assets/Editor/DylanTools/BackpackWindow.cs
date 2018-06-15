using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace BackpacViewerWindow
{
    public class BackpackWindow : EditorWindow
    {         
        private BackpackViewer Backpack = new BackpackViewer();        
        private EditorEvents _Events = new EditorEvents();
        private List<ItemBackpackVisual> Items = new List<ItemBackpackVisual>();
        Random r = new Random();

        [UnityEditor.MenuItem("Tools/Backpack Viewer")]
        public static void Init()
        {
            var window = ScriptableObject.CreateInstance<BackpackWindow>();
            window.Show();
        }

        void OnEnable()
        {
            _Events.MouseDownEvent = Backpack.EnableResize;
            _Events.MouseUpEvent = Backpack.DisableResize;
            _Events.MouseDragEvent = Backpack.Resize;
        }

        public void OnGUI()
        {
            var itemScriptables = Resources.FindObjectsOfTypeAll(typeof(ItemScriptable)).ToList();
            bool exists = false;
            foreach (var item in itemScriptables)
            {
                foreach (var data in Items)
                {
                    if (item == data.Data)
                    {
                        exists = true;
                        break;
                    }
                }
                if(exists)
                    break;
                var newItemVisual = new ItemBackpackVisual();
                newItemVisual.Data = item as ItemScriptable;
                float randX = r.Next(0, 250);
                float randY = r.Next(0, 250);
                newItemVisual.Positon = new Vector2(randX, randY);
                Items.Add(newItemVisual);
                _Events.MouseDownEvent += newItemVisual.EnableDragging;
                _Events.MouseUpEvent += newItemVisual.DisableDragging;
                _Events.MouseDragEvent += newItemVisual.Drag;
            }
            Backpack.Data = EditorGUILayout.ObjectField(Backpack.Data, typeof(BackpackScriptable), false) as BackpackScriptable;
            Backpack.Draw();
            foreach (var visual in Items)
            {
                foreach (var vis in Items)
                {
                    if (visual._Rect.Contains(vis._Rect.position) && vis != visual && !vis.IsDraggable && !visual.IsDraggable)
                    {
                        vis.Positon += vis._Rect.size;
                    }
                }
                foreach (var slot in Backpack.Slots)
                {
                    if (!visual.IsDraggable)
                    {
                        if (slot.Contains(visual._Rect.position))
                        {
                            visual.Positon = slot.position;
                            visual.Data.TryAddItem(Backpack.Data);
                            break;
                        }                        
                    }
                    else
                    {
                        Backpack.Data.UnpackItem(visual.Data);
                    }
                }
                visual.Draw();
            }            

            _Events.Update();
        }
    }
}