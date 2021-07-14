using System;
using System.Collections.Generic;
using System.Text;

namespace CChessEngine
{
    public class CChessMoveData
    {
        public CChessMove Move { get; set; }
        public CChessBoardNode NextBoardNode { get; set; }
        public CChessMoveData()
            : this(null)
        { }

        public CChessMoveData(CChessMove move, CChessBoardNode ccbn = null)
        {
            if (move == null)
                return;
            Move = move;
            if(ccbn != null)
                NextBoardNode = ccbn;
        }
    }

    public static partial class Extension
    {
        public static List<CChessMoveData> ToMoveDataList(this List<CChessMove> movelist)
        {
            if (movelist == null)
                return null;
            List<CChessMoveData> result = new List<CChessMoveData>();
            for(int i = 0; i < movelist.Count; i++)
                result.Add(new CChessMoveData(movelist[i]));
            return result;
        }
    }
}
