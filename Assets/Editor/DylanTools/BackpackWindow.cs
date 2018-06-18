using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackpacViewerWindow;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace BackpackViewerWindow
{
    public class BackpackWindow : EditorWindow
    {
        private readonly BackpackViewer Backpack = new BackpackViewer();
        private readonly EditorEvents _Events = new EditorEvents();
        private readonly List<ItemBackpackVisual> Items = new List<ItemBackpackVisual>();
        private Random random;

        [UnityEditor.MenuItem("Tools/DylanTools/Backpack Viewer")]
        public static void Init()
        {
            var window = ScriptableObject.CreateInstance<BackpackWindow>();
            window.Show();
        }

        public void OnEnable()
        {
            random = new Random();
            _Events.MouseDownEvent = Backpack.EnableResize;
            _Events.MouseUpEvent = Backpack.DisableResize;
            _Events.MouseDragEvent = Backpack.Resize;
        }

        public static List<ItemScriptable> itemScriptables => Resources.LoadAll<ItemScriptable>("").ToList();
        public void OnFocus()
        {
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

                if (exists)
                {
                    break;
                }
                var newItemVisual = new ItemBackpackVisual();
                newItemVisual.Data = item as ItemScriptable;
                float randX = random.Next(0, 250);
                float randY = random.Next(0, 250);
                newItemVisual.Positon = new Vector2(randX, randY);
                Items.Add(newItemVisual);
                _Events.MouseDownEvent += newItemVisual.EnableDragging;
                _Events.MouseUpEvent += newItemVisual.DisableDragging;
                _Events.MouseDragEvent += newItemVisual.Drag;
            }
        }

        public void OnGUI()
        {

            Backpack._Data = EditorGUILayout.ObjectField(Backpack._Data, typeof(BackpackScriptable), false) as BackpackScriptable;
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
                foreach (var slot in Backpack._Slots)
                {
                    if (!visual.IsDraggable)
                    {
                        if (slot.Contains(visual._Rect.position))
                        {
                            visual.Positon = slot.position;
                            visual.Data.TryAddItem(Backpack._Data);
                            break;
                        }
                    }
                    else
                    {
                        Backpack._Data.UnpackItem(visual.Data);
                    }
                }
                visual.Draw();
            }

            _Events.Update();
        }
    }
}