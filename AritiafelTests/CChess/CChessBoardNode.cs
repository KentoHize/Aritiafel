using System;
using System.Collections.Generic;
using System.Text;

namespace CChessEngine
{
    public class CChessBoardNode
    {
        public const long DefaultScore = 50000000000000000;
        public const long MaxScore = 100000000000000000;
        public CChessBoard Board { get; set; }
        public List<CChessMoveData> NextMoves { get; set; }
        public long Player1WinCount { get; set; }
        public long Player2WinCount { get; set; }
        public long DrawCount { get; set; }
        public long Score { get; set; }
        public bool CompleteNode { get; set; }

        public CChessBoardNode()
            : this(null)
        { }

        public CChessBoardNode(CChessBoard board, List<CChessMoveData> nextMoves = null)
        {
            Board = board;            
            Score = DefaultScore;
            if (board == null)
                return;
            if (nextMoves == null)
                nextMoves = CChessSystem.GetLegalMoves(board).ToMoveDataList();
            NextMoves = nextMoves;
        }

        //public CChessBoardNode(CChessBoard board, List<C>)

        //Expand
        //Save
        //Load
    }
}
