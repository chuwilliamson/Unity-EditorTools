﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Interface;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class BackpackScriptable : ScriptableObject, IPacker, IPackable
    {
        #region IPacker Implementaion        
        [SerializeField]
        private int Capacity;
        public int _Capacity
        {
            get { return Capacity; }
        }
        [SerializeField]
        private List<ItemScriptable> Packables = new List<ItemScriptable>();

        public List<ItemScriptable> _Packable
        {
            get { return Packables; }
        }

        public bool PackItem(IPackable packable)
        {
            if (packable == this)
            {
                return false;
            }
            if (Packables.Count < Capacity)
            {
                if (!Packables.Contains(packable as ItemScriptable))
                {
                    Packables.Add(packable as ItemScriptable);
                }
                return true;
            }
            return false;
        }
        #endregion

        #region IPackable Implementation       
        public void UnpackItem(IPackable packable)
        {
            if (Packables.Contains(packable as ItemScriptable))
            {
                Packables.Remove(packable as ItemScriptable);
            }
        }

        [SerializeField]
        private string _Name;
        public string Name
        {
            get { return _Name; }
        }
        public void TryAddItem(IPacker packer)
        {
            packer.PackItem(this);
        }
        #endregion
    }
}
