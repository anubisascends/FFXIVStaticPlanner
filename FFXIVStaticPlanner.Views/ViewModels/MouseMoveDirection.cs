using System;

namespace FFXIVStaticPlanner.ViewModels
{
    [Flags]
    public enum MouseMoveDirection
    {
        None    = 0x00,
        Left    = 0x01,
        Right   = 0x02,
        Up      = 0x04,
        Down    = 0x08
    }
}