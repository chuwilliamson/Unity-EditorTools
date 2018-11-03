using UnityEngine;

namespace ChuTools.NodeEditor.EventSystems
{
    public class NodeWindowEventSystem : EditorEventSystem
    {
        public override void PollEvents(Event e)
        {
            View.NodeEditor.Drag = Vector2.zero;
            base.PollEvents(e);
        }
    }
}