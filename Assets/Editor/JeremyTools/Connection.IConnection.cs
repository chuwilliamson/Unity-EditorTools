
using Interfaces;
using UnityEditor;
using UnityEngine;
namespace JeremyTools
{
    public partial class Connection : IConnection
    {
        public IDrawable In { get; set; }
        public IDrawable Out { get; set; }
    }
}