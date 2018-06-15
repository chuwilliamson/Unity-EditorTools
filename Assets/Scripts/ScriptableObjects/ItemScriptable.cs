using System.Collections;
using System.Collections.Generic;
using Interface;
using UnityEngine;
using UnityEngine.UI;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class ItemScriptable : ScriptableObject, IPackable
    {        
        public string Name;
        public GameObject Model;
        public Texture2D UIImage;
        public virtual void TryAddItem(IPacker packer)
        {
            packer.PackItem(this);
        }
    }
}