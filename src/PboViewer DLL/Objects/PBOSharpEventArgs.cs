using PboViewer_Lib;
using System;

namespace PboViewer_Lib.Objects
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
