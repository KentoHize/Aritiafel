using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Aritiafel.Characters.Heroes;


namespace CChessEngine
{
    public enum MoveStatus
    {
        BestMove,
        NoBestMove,
        RequestADraw,
        Resign
    }

    public class CChessAI
    {
        public string BoardNodeDataPath { get; set; }
        public string MoveRecordDataPath { get; set; }
        public CChessBoardNode StartBoardNode { get; set; }
        public List<CChessBoardNode> BoardNodes { get; set; }
        public List<CChessMove> MoveRecords { get; set; }
        public MoveStatus Go(CChessBoard board, out CChessMove bestMove, out List<CChessMove> estimateMoves)
        {
            bestMove = null;
            estimateMoves = null;
            return MoveStatus.NoBestMove;
        }

        public CChessAI()
            : this(null)
        { }

        public CChessAI(CChessBoard startBoard = null)
        {
            BoardNodeDataPath = @"C:\Programs\WPF\DeepMind\CChessEngine\Data\Board";
            MoveRecordDataPath = @"C:\Programs\WPF\DeepMind\CChessEngine\Data\Move";
            if (startBoard == null)
                startBoard = CChessBoard.StartingBoard;
            StartBoardNode = LoadBoardNode(startBoard);
            
        }

        public CChessBoardNode LoadBoardNode(CChessBoard board)
        {
            CChessBoardNode cbn;
            string filePath = Path.Combine(BoardNodeDataPath, $"{board.PrintBoardString(true)}.txt");
            if (File.Exists(filePath))
                cbn = Tina.LoadJsonFile<CChessBoardNode>(filePath);
            else
            {
                cbn = new CChessBoardNode(board);
                Tina.SaveJsonFile(filePath, cbn, true);
            }        
            //To Do

            BoardNodes.Add(cbn);
            return cbn;
        }
    }
}
