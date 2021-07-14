using System;
using System.Collections.Generic;
using System.Text;

namespace CChessEngine
{
    public enum CChessStatus
    {
        None = 0,
        AtStart, //開局
        Smooth, //平穩進行 
        RedCatch, //紅方捉子
        BlackCatch, //黑方捉子
        RedCheck, //紅方將軍
        BlackCheck, //黑方將軍
        RedCheckmate, //紅方將死
        BlackCheckmate, //黑方將死
        RedWin, //紅方獲勝
        BlackWin, //黑方獲勝
        Draw //平手
    }

    public static partial class Extension
    {
        public static string ToCString(this CChessStatus ccs)
        {
            switch(ccs)
            {
                case CChessStatus.None:
                    return "無";
                case CChessStatus.AtStart:
                    return "開局";
                case CChessStatus.Smooth:
                    return "平穩";
                case CChessStatus.BlackCatch:
                    return "黑方捉子";
                case CChessStatus.RedCatch:
                    return "紅方捉子";
                case CChessStatus.BlackCheck:
                    return "黑方將軍";
                case CChessStatus.RedCheck:
                    return "紅方將軍";
                case CChessStatus.BlackCheckmate:
                    return "黑方絕殺";
                case CChessStatus.RedCheckmate:
                    return "紅方絕殺";
                case CChessStatus.BlackWin:
                    return "黑方勝利";
                case CChessStatus.RedWin:
                    return "紅方勝利";
                case CChessStatus.Draw:
                    return "和局";
                default:
                    throw new ArgumentOutOfRangeException(nameof(ccs));
            }
        }
    }
}
