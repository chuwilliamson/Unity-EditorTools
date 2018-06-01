using Interfaces;
using UnityEditor;
using UnityEngine;

namespace JeremyTools
{
    public partial class Connection
    {
        public Connection(IDrawable inN, IDrawable outN)
        {
            In = inN;
            Out = outN;
        }
    }
}