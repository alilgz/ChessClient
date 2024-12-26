using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClient.Game
{



    public class ChessFigureMove
    {
        public string AtkSymbol = "";
        public ChessColor playerColor;
        public Figure movedFigure;
        public Figure upgradedFigure;
        public Figure takenFigure;
        public Position originalPosition;
        public Position newPosition;
        public bool CastleLeft = false;
        public bool CastleRight = false;

        // Castling kingside is recorded as "0-0," while castling queenside is "0-0-0."
        public string ToString()
        {
            if (CastleLeft)
                return "0-0";
            if (CastleRight)
                return "0-0-0";

            var takenString = (takenFigure != Figure.none ? "x" : "");
            var promotedString = upgradedFigure != Figure.none ? "=" + upgradedFigure.ToString() : "";
            return movedFigure.toString() + takenString + promotedString + originalPosition.ToString(true) + " " + newPosition.ToString() + AtkSymbol;
        }

        public static ChessFigureMove UpgradeMove(ChessMap map, Position oldPos, Position newPos, Figure taken, bool isKingAttacked, bool isGameWon)
        {
            var upgraded = map[newPos];
            return new ChessFigureMove()
            {
                playerColor = upgraded.GetColor(),
                originalPosition = oldPos,
                newPosition = newPos,
                movedFigure = upgraded.GetColor() == ChessColor.White ? Figure.wPawn : Figure.bPawn,
                takenFigure = taken,
                upgradedFigure = upgraded,
                AtkSymbol = (isGameWon ? "#" : (isKingAttacked ? "+" : ""))
            };
        }

        public static ChessFigureMove Move(ChessMap map, Position oldPos, Position newPos, Figure taken, bool isKingAttacked, bool isGameWon)
        {
            var figure = map[newPos];
            return new ChessFigureMove()
            {
                playerColor = figure.GetColor(),
                originalPosition = oldPos,
                newPosition = newPos,
                movedFigure = figure,
                takenFigure = taken,
                upgradedFigure = Figure.none,
                AtkSymbol = (isGameWon ? "#" : (isKingAttacked ? "+" : ""))

            };
        }

        public static ChessFigureMove Castle(ChessColor player, bool queenSide)
        {
            return new ChessFigureMove()
            {
                playerColor = player,
                CastleLeft = !queenSide,
                CastleRight =  queenSide
            };
        }
    }
}
