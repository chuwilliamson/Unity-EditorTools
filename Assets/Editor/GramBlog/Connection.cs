using ChuTools;
using System;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace _Editor.GramBlog
{
    public class Connection
    {
        public ConnectionPoint inPoint;

        [XmlIgnore]
        public Action<Connection> OnClickRemoveConnection;

        public ConnectionPoint outPoint;

        public Connection()
        {
        }

        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> onClickRemoveConnection)
        {
            this.inPoint = inPoint;
            this.outPoint = outPoint;
            this.OnClickRemoveConnection = onClickRemoveConnection;
        }


        public void Draw()
        {
            Handles.DrawBezier(
                startPosition: inPoint.rect.center,
                endPosition: outPoint.rect.center,
                startTangent: inPoint.rect.center + Vector2.left * 50f,
                endTangent: outPoint.rect.center - Vector2.left * 50f,
                color: Color.white,
                texture: null,
                width: 2f
            );

            if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, direction: Quaternion.identity, size: 4,
                pickSize: 8, capFunc: Handles.RectangleCap))
                if (OnClickRemoveConnection != null)
                    OnClickRemoveConnection(this);
        }
    }
}