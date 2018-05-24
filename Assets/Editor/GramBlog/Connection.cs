using ChuTools;
using System;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace _Editor.GramBlog
{
    public class Connection
    {
        public ConnectionPoint InPoint;

        [XmlIgnore]
        public Action<Connection> OnClickRemoveConnection;

        public ConnectionPoint OutPoint;

        public Connection()
        {
        }

        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> onClickRemoveConnection)
        {
            this.InPoint = inPoint;
            this.OutPoint = outPoint;
            this.OnClickRemoveConnection = onClickRemoveConnection;
        }

        public void Draw()
        {
            Handles.DrawBezier(InPoint.rect.center, OutPoint.rect.center,
                startTangent: InPoint.rect.center + Vector2.left * 50f,
                endTangent: OutPoint.rect.center - Vector2.left * 50f, color: Color.white, texture: null, width: 2f);
            if (Handles.Button(position: (InPoint.rect.center + OutPoint.rect.center) * 0.5f,
                direction: Quaternion.identity, size: 4, pickSize: 8, capFunction: Target))
            {
                OnClickRemoveConnection?.Invoke(this);
            }
        }

        private void Target(int controlId, Vector3 position, Quaternion rotation, float size, EventType eventType)
        {
            throw new NotImplementedException();
        }
    }
}