using System;
using System.Collections.Generic;
using System.Text;

namespace CChessEngine
{
    public class CChessMove
    {
        public byte X1 { get; set; }
        public byte Y1 { get; set; }
        public byte X2 { get; set; }
        public byte Y2 { get; set; }
        public CChessMove()
        { }

        public CChessMove(byte x1, byte y1, byte x2, byte y2)
        {   
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public CChessMove(int x1, int y1, int x2, int y2)
            : this((byte)x1, (byte)y1, (byte)x2, (byte)y2)
        { }

        public override string ToString()
            => $"{(char)('a' + X1)}{Y1}{(char)('a' + X2)}{Y2}";

        public string ToTestString()
            => $"{X1},{Y1} => {X2},{Y2}";

        public override bool Equals(object obj)
        {
            if (!(obj is CChessMove ccm))
                return false;
            if (X1 == ccm.X1 && X2 == ccm.X2 && Y1 == ccm.Y1 && Y2 == ccm.Y2)
                return true;
            return false;
        }   

        public override int GetHashCode()
            => X1.GetHashCode() ^ X2.GetHashCode() ^ Y1.GetHashCode() ^ Y2.GetHashCode();
    }
}
