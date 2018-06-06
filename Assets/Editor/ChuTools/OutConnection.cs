﻿using System;
using Interfaces;

namespace ChuTools
{
    [Serializable]
    public class OutConnection : IConnectionOut
    {
        public OutConnection(INode inputNode)
        {
            Node = inputNode;
        }

        public int Value => Node?.Value ?? 0;
        public INode Node { get; set; }
    }
}