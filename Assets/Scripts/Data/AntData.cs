using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/AntData")]
    public class AntData : ScriptableObject
    {
        public Vector2 AntPosition;
        public Vector2 HomePosition;
        public Vector2 LeafPosition;

        public float CursorDistance => Vector2.Distance(a: Input.mousePosition, b: AntPosition);
        public float HomeDistance => Vector2.Distance(a: HomePosition, b: AntPosition);
        public float LeafDistance => Vector2.Distance(a: LeafPosition, b: AntPosition);
    }
}