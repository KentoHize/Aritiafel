using System;
using System.Collections.Generic;
using System.Text;

namespace CChessEngine
{
    //9 x 10棋盤
    //Piece letters
    //將 帥 K k
    //士 仕 A a
    //象 相 B b
    //車 俥 R r
    //馬 傌 N n
    //包 炮 C c
    //卒 兵 P p

    //Move
    //炮2平5(炮在h8) h8 e8
    //馬8進7(馬在h1) h1 g3

    //10
    //9
    //8
    //7
    //6
    //5
    //4
    //3
    //2
    //1
    //  a b c d e f g h i

    //BoardString
    //A. 盤面
    //B. 輪誰
    //C. -
    //D. -
    //E. 閒步數
    //F. 總步數
    //G. 紅方捉數
    //H. 紅方將數
    //I. 黑方捉數
    //J. 黑方將數

    //主要為A B

    //BoardString 從上往下讀取
    //_Data 從下往上存
    //Indexer X Y易位
    public class CChessBoard
    {
        private const string ExceptionString = "\"{0}\" is not a valid Chinese Chess board data string.";
        public const string ChessLetters = "KABRNCPkabrncp ";        
        public const string StartingBoardString = "rnbakabnr/9/1c5c1/p1p1p1p1p/9/9/P1P1P1P1P/1C5C1/9/RNBAKABNR w";
        public static CChessBoard StartingBoard => new CChessBoard();
        public static Dictionary<char, char> LetterToChineseWord => new Dictionary<char, char>()
        { { 'k', '將' }, { 'a', '士' }, { 'b', '象' }, { 'n', '馬' }, { 'r', '車' }, { 'c', '包' }, { 'p', '卒' },
          { 'K', '帥' }, { 'A', '仕' }, { 'B', '相' }, { 'N', '傌' }, { 'R', '俥' }, { 'C', '炮' }, { 'P', '兵' },
          { ' ', '　'} };

        protected char[][] _Data;
        public bool IsBlackTurn { get; set; }
        public int[] CatchCount { get; protected set; } = new int[2];
        public int[] CheckCount { get; protected set; } = new int[2];
        public int HalfMoveCount { get; set; }
        public int TotalMoveCount { get; set; }
        public CChessBoard(string boardString = StartingBoardString)
            => LoadBoardString(boardString);

        //取出相反
        public char this[int x, int y]
        {
            get => _Data[y][x];
            set => _Data[y][x] = value;
        }   

        public char[][] Data
        {
            get => _Data;
            set
            {
                if (value.Length != 10)
                    throw new ArgumentException(string.Format(ExceptionString, "This"));
                for (int i = 0; i < 10; i++)
                {
                    if (value[i].Length != 9)
                        throw new ArgumentException(string.Format(ExceptionString, "This"));
                    for (int j = 0; j < 9; j++)
                        if (!ChessLetters.Contains(value[i][j].ToString()))
                            throw new ArgumentException(string.Format(ExceptionString, "This"));
                }
                _Data = value;
            }
        }
        
        private void Clear()
        {
            _Data = null;
            IsBlackTurn = false;
            CatchCount[0] = CatchCount[1] =
            CheckCount[0] = CheckCount[1] =
            HalfMoveCount = TotalMoveCount = 0;
        }

        public void SetBoardWithoutCheck(char[][] value, bool isBlackTurn = false)
        {
            Clear();
            _Data = value;
            IsBlackTurn = isBlackTurn;
        }

        public void LoadBoardString(string value)
        {
            Clear();
            string[] buffer, buffer2;
            if (!value.Contains(" "))
                throw new ArgumentException(string.Format(ExceptionString, value));
            buffer = value.Split(' ');

            if(buffer.Length != 2 && buffer.Length != 10)
                throw new ArgumentException(string.Format(ExceptionString, value));
            if (!buffer[0].Contains("/"))
                throw new ArgumentException(string.Format(ExceptionString, value));
            buffer2 = buffer[0].Split('/');
            if (buffer2.Length != 10)
                throw new ArgumentException(string.Format(ExceptionString, value));

            char[][] data = new char[10][];
            for (int i = 0; i < 10; i++)
            {
                data[i] = new char[9];
                int p = 0;
                for (int j = 0; j < buffer2[9 - i].Length; j++)
                {
                    if (Char.IsDigit(buffer2[9 - i], j))
                    {
                        int m = Convert.ToInt32(buffer2[9 - i][j].ToString());
                        if (m == 0)
                            throw new ArgumentException(string.Format(ExceptionString, value));
                        for (int k = 0; k < m; k++)
                            data[i][p++] = ' ';                            
                    }
                    else
                    {
                        if (!ChessLetters.Contains(buffer2[9 - i][j].ToString()))
                            throw new ArgumentException(string.Format(ExceptionString, value));
                        data[i][p++] = buffer2[9 - i][j];
                    }
                }
            }
            _Data = data;

            if (buffer[1] == "w" || buffer[1] == "r")
                IsBlackTurn = false;
            else if (buffer[1] == "b")
                IsBlackTurn = true;
            else
                throw new ArgumentException(string.Format(ExceptionString, value));

            if (buffer.Length == 10)
            {
                //Full Detail Mode                
                HalfMoveCount = Convert.ToInt32(buffer[4]);
                TotalMoveCount = Convert.ToInt32(buffer[5]);
                CatchCount[0] = Convert.ToInt32(buffer[6]);
                CheckCount[0] = Convert.ToInt32(buffer[7]);
                CatchCount[1] = Convert.ToInt32(buffer[8]);
                CheckCount[1] = Convert.ToInt32(buffer[9]);                
            }
        }

        public string PrintBoardString(bool fileNameFormat = false)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 9; i >= 0; i--)
            {
                int emptyCount = 0;
                for (int j = 0; j < 9; j++)
                {
                    if (_Data[i][j] == ' ')
                        emptyCount++;
                    else
                    {
                        if (emptyCount != 0)
                        {
                            result.Append(emptyCount);
                            emptyCount = 0;
                        }
                        result.Append(_Data[i][j]);
                    }
                }
                if (emptyCount != 0)
                    result.Append(emptyCount);
                result.Append('/');
            }
            result.Remove(result.Length - 1, 1);
            result.Append(' ');
            result.Append(IsBlackTurn ? 'b' : 'w');
            if (fileNameFormat)
                return result.ToString().Replace('/', '_').Replace(' ', '+');
            if (CatchCount[0] == 0 && CatchCount[1] == 0 &&
               CheckCount[0] == 0 && CheckCount[1] == 0 &&
               HalfMoveCount == 0 && TotalMoveCount == 0)
                return result.ToString();
            result.AppendFormat("- - {0} {1} {2} {3} {4} {5}", HalfMoveCount, TotalMoveCount, CatchCount[0], CheckCount[0],
                CatchCount[1], CheckCount[1]);
            return result.ToString();
        }

        public string PrintBoard(bool displayPlayerTurn = true)
        {
            StringBuilder result = new StringBuilder();
            for(int i = 9; i >= 0; i--)
            {            
                for(int j = 0; j < 9; j++)
                    result.Append(LetterToChineseWord[_Data[i][j]]);
                result.Append('\n');
            }
            if (displayPlayerTurn)
                result.AppendFormat("下一步：{0}方\n", IsBlackTurn ? "黑" : "紅");            
            return result.ToString();
        }

        public override string ToString()
            => PrintBoardString(true);
            
    }
}
