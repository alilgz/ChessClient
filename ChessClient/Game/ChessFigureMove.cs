using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClient.Game
{

    
    
    public class ChessFigureMove
    {
        public ChessColor playerColor;
        public Figure movedFigure;
        public Figure? upgradedFigure;
        public Figure? takenFigure;
        public ChessMove originalPosition;
        public ChessMove newPosition;
    }

}
