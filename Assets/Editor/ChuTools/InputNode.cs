﻿using Interfaces;

namespace ChuTools
{

    // ReSharper disable InconsistentNaming

    /// Process:::
    /// Input node manipulates information
    /// OUT connection will carry that data
    /// IN connection will be connected to the OUT connection
    /// Display node will display the information from the OUT connection
    /// <summary>
    ///     this node will have one out
    ///     It will manipulate it's data
    ///     The Out Connection will take this data and transfer it to an inconnection
    /// </summary>
    [System.Serializable]
    public class InputNode : INode
    {
        public InputNode()
        {
            Value = 0;
        }

        public int Value { get; set; }

    }
}