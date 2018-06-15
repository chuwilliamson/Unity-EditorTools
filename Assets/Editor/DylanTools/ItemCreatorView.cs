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
        public Color BackgroundColor = Color.white;
        public Rect _Rect { get; private set; }
        public ItemScriptable ContianedItem;

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
            if(ContianedItem == null)
                return;
            var sealizedItem = new SerializedObject(ContianedItem);
            EditorGUI.PropertyField(new Rect(Rect.position, new Vector2(Rect.width, 20)),
                sealizedItem.FindProperty("Name"));
            ContianedItem.name = ContianedItem.Name;
            ContianedItem.Model = EditorGUI.ObjectField(
                new Rect(Rect.position + new Vector2(0, 25), new Vector2(Rect.width, 20)),
                "Model", ContianedItem.Model, typeof(GameObject), false) as GameObject;
            if (ContianedItem.Model != null)
            {
                var image = AssetPreview.GetAssetPreview(ContianedItem.Model);
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
                    ItemPosition.x += Event.current.delta.x;
                if (Event.current.delta.x > 0 && Rect.position.x + Rect.width < WindowSize.x)
                    ItemPosition.x += Event.current.delta.x;
                if (Event.current.delta.y < 0 && Rect.position.y > 0)
                    ItemPosition.y += Event.current.delta.y;
                if (Event.current.delta.y > 0 && Rect.position.y + Rect.height < WindowSize.y)
                    ItemPosition.y += Event.current.delta.y;
                Event.current.Use();
            }
        }

        public void DisableDragging()
        {
            ItemIsDragging = false;
        }
    }
}