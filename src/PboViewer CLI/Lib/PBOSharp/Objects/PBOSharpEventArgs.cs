using PBOSharp.Enums;
using System;

namespace PBOSharp.Objects
{
    public delegate void PBOSharpEventHandler(PBOSharpEventArgs args);

    public class PBOSharpEventArgs : EventArgs
    {
        public string Message { get; internal set; }
        public EventType Type { get; internal set; }

        public PBOSharpEventArgs(string message, EventType type)
        {
            Message = message;
            Type = type;
        }
    }
}
