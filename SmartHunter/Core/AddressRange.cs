using System;

namespace SmartHunter.Core
{
    public struct AddressRange
    {
        public ulong Start { get; private set; }
        public ulong End { get; private set; }

        public AddressRange(ulong start, ulong end)
        {
            Start = start;
            End = end;
        }
    }
}
