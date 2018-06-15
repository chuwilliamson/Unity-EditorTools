using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BackpacViewerWindow;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Tizen;

namespace ItemWindow
{
    public class ItemCreatorWindow : EditorWindow
    {
        #region Resource View        
        private float ResourcesWidth = 100;
        private Rect ResouceViewScalar;
        private bool ResourceIsDragging;
        private Dictionary<ItemScriptable, Rect> ResourceRects;        
        #endregion

        private readonly EditorEvents _editorEvents = new EditorEvents();
        private ItemCreatorView ItemView = new ItemCreatorView();
        
        private Vector2 WindowSize;        

        [UnityEditor.MenuItem("Tools/Item Creator")]
        public static void Init()
        {
            var window = ScriptableObject.CreateInstance<ItemCreatorWindow>();
            window.Show();

        }

        public void OnEnable()
        {
            _editorEvents.MouseDownEvent += DisplayCreateMenu;
            _editorEvents.MouseDownEvent += EnableResourceDrag;
            _editorEvents.MouseDownEvent += ItemView.EnableDragging;
            _editorEvents.MouseDragEvent += ResizeResourceView;
            _editorEvents.MouseDragEvent += ItemView.Drag;
            _editorEvents.MouseDownEvent += SetActiveItem;
            _editorEvents.MouseUpEvent += DisableDragging;
            _editorEvents.MouseUpEvent += ItemView.DisableDragging;
        }

        public void OnGUI()
        {
            WindowSize = new Vector2();
            if (EditorWindow.focusedWindow != null)
            {
                WindowSize = EditorWindow.focusedWindow.position.size;
            }

            ResourceRects = new Dictionary<ItemScriptable, Rect>();
            Rect ResourcesView = new Rect(0, 0, ResourcesWidth, WindowSize.y);
            GUI.Box(ResourcesView, "");
            ResouceViewScalar = new Rect(ResourcesView.width, ResourcesView.y, 5, ResourcesView.height);
            GUI.backgroundColor = Color.black;
            GUI.Box(ResouceViewScalar, "");
            var itemScriptables = BackpackViewerWindow.BackpackWindow.itemScriptables;
            foreach (var item in itemScriptables)
            {
                var position = Vector2.zero;
                if (ResourceRects.Count > 0)
                {
                    position = ResourceRects[itemScriptables[itemScriptables.IndexOf(item) - 1] as ItemScriptable]
                                   .position + new Vector2(0, 25);
                }

                var newRect = new Rect(position, new Vector2(ResourcesView.size.x, 25));
                ResourceRects.Add(item as ItemScriptable, newRect);
                if (item == ItemView._ContainedItem)
                {
                    GUI.backgroundColor = Color.blue;
                }
                else
                {
                    GUI.backgroundColor = Color.gray;
                }
                GUI.Box(newRect, item.name);
            }
            
            ItemView.Draw();
            _editorEvents.Update();
        }

        public void DisableDragging()
        {
            ResourceIsDragging = false;            
            Event.current.Use();
        }

        public void SetActiveItem()
        {
            foreach (var rects in ResourceRects)
            {
                if (rects.Value.Contains(Event.current.mousePosition))
                {
                    ItemView._ContainedItem = rects.Key;
                    Event.current.Use();
                    break;
                }
            }
        }

        public void DisplayCreateMenu()
        {
            if (Event.current.button == 1)
            {
                GenericMenu newItemMenu = new GenericMenu();
                newItemMenu.AddItem(new GUIContent("Create New Item"), true, CreateNewItem);
                newItemMenu.ShowAsContext();
                Event.current.Use();
            }
        }

        public void EnableResourceDrag()
        {
            if (Event.current.button == 0)
            {
                if (ResouceViewScalar.Contains(Event.current.mousePosition))
                {
                    ResourceIsDragging = true;
                    Event.current.Use();
                }
            }
        }

        public void ResizeResourceView()
        {
            if (ResourceIsDragging)
            {
                if (Event.current.delta.x < 0 && ResourcesWidth > 50)
                {
                    ResourcesWidth += Event.current.delta.x;
                }

                if (Event.current.delta.x > 0 && ResourcesWidth < WindowSize.x - 25)
                {
                    ResourcesWidth += Event.current.delta.x;
                }
                Event.current.Use();
            }
        }

        void CreateNewItem()
        {
            var newItem = ScriptableObject.CreateInstance(typeof(ItemScriptable));
            AssetDatabase.CreateAsset(newItem, @"Assets\Resources\newItem" + ResourceRects.Count + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}