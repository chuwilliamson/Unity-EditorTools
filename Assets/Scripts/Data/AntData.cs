using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/AntData")]
    public class AntData : ScriptableObject
    {
        //updated from FSMBehaviour
        public Vector3 AntPosition;
        public Vector3 HomePosition;
        public Vector3 LeafPosition;
        public Vector3 Velocity;
        public Vector3 CursorPosition;
        public List<string> Inventory;
        //only readable info
        public float CursorDistance => Vector3.Distance(a: CursorPosition, b: AntPosition);
        public float HomeDistance => Vector3.Distance(a: HomePosition, b: AntPosition);
        public float LeafDistance => Vector3.Distance(a: LeafPosition, b: AntPosition);
    }
}