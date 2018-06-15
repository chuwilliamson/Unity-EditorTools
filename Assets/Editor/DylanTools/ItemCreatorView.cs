using System.Collections;
using System.Collections.Generic;
using EditorInterfaces;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ItemWindow
{
    public class ItemCreatorView : IDrawable, IDraggable
    {
        private Rect Rect;
        private Vector2 ItemPosition = new Vector2(250, 0);
        private bool ItemIsDragging;
        private Color BackgroundColor = Color.white;
        public Rect _Rect
        {
            get { return Rect;}
        }
        private ItemScriptable ContainedItem;

        public ItemScriptable _ContainedItem
        {
            get { return ContainedItem; }
            set { ContainedItem = value; }
        }

        public bool IsDraggable
        {
            get { return ItemIsDragging; }
        }

        public Vector2 Positon
        {
            get { return ItemPosition; }
            set { ItemPosition = value; }
        }

        public void Draw()
        {
            GUI.backgroundColor = BackgroundColor;
            Rect = new Rect(ItemPosition, new Vector2(250, 250));
            GUI.Box(Rect, "");
            if (ContainedItem == null)
            {
                return;
            }
            var sealizedItem = new SerializedObject(ContainedItem);
            EditorGUI.PropertyField(new Rect(Rect.position, new Vector2(Rect.width, 20)),
                sealizedItem.FindProperty("Name"));
            ContainedItem.name = ContainedItem.Name;
            ContainedItem.Model = EditorGUI.ObjectField(
                new Rect(Rect.position + new Vector2(0, 25), new Vector2(Rect.width, 20)),
                "_Model", ContainedItem.Model, typeof(GameObject), false) as GameObject;
            if (ContainedItem.Model != null)
            {
                var image = AssetPreview.GetAssetPreview(ContainedItem.Model);
                GUILayout.BeginArea(new Rect(Rect.position + new Vector2(Rect.width / 4, 50),
                    new Vector2(image.width, image.height)));
                GUILayout.Label(image);
                GUILayout.EndArea();
            }
            sealizedItem.ApplyModifiedProperties();
        }

        public void EnableDragging()
        {
            if (Event.current.button == 0)
            {
                if (Rect.Contains(Event.current.mousePosition))
                {
                    ItemIsDragging = true;
                    Event.current.Use();
                }
            }
        }

        public void Drag()
        {
            var WindowSize = new Vector2();
            if (EditorWindow.focusedWindow != null)
                WindowSize = EditorWindow.focusedWindow.position.size;
            if (ItemIsDragging)
            {
                if (Event.current.delta.x < 0 && Rect.position.x > 0)
                {
                    ItemPosition.x += Event.current.delta.x;
                }

                if (Event.current.delta.x > 0 && Rect.position.x + Rect.width < WindowSize.x)
                {
                    ItemPosition.x += Event.current.delta.x;
                }

                if (Event.current.delta.y < 0 && Rect.position.y > 0)
                {
                    ItemPosition.y += Event.current.delta.y;
                }

                if (Event.current.delta.y > 0 && Rect.position.y + Rect.height < WindowSize.y)
                {
                    ItemPosition.y += Event.current.delta.y;
                }
                Event.current.Use();
            }
        }

        public void DisableDragging()
        {
            ItemIsDragging = false;
        }
    }
}