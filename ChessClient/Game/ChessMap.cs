using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClient.Game
{
    public class ChessMap
    {
        public Figure[,] map = new Figure[8, 8];
        public ChessMap()
        {


        }

        internal bool SelectedFigureIsValid(Position pos, ChessColor currentPlayer, GameStage currentStage)
        {
            //check if there figure 
            // check if color related to stage
            // 
            if (!pos.IsValid() || map[pos.x, pos.y] == Figure.none)
                return false;

            var figureColor = map[pos.x, pos.y].getColor();

            return (
                    (figureColor == ChessColor.White && currentPlayer == ChessColor.White && currentStage == GameStage.WhiteSelect) ||
                    (figureColor == ChessColor.White && currentPlayer == ChessColor.White && currentStage == GameStage.WhiteMove) ||
                    (figureColor == ChessColor.Black && currentPlayer == ChessColor.Black && currentStage == GameStage.BlackSelect) ||
                    (figureColor == ChessColor.Black && currentPlayer == ChessColor.Black && currentStage == GameStage.BlackMove)
                );
        }

        public ChessMap CalculateMoves(Position pos)
        {
            var piece = map[pos.x, pos.y];
            return piece.getPossibleMoves(this, pos);
        }

        internal bool Empty()
        {
            foreach (var r in map)
            {
                if (r != Figure.none)
                    return false;
            }
            return true;
        }

        internal void Clear()
        {
            Fill(map, Figure.none);
        }


        private void Fill(Figure[,] map, Figure f)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    map[i, j] = f;
                }
            }

        }


        public bool CanMoveToPosition(Position pos, ChessColor myColor, out bool takenFigure)
        {
            takenFigure = false;
            if (!pos.IsValid())
            {
                return false;
            }
            ChessColor opponentColor = myColor == ChessColor.White ? ChessColor.Black : ChessColor.White;


            // we cant move if there  our figure 
            if (map[pos.x, pos.y].getColor() == myColor) // todo: check for castle 
                return false;

            // we cant take opponent figure, but cant move after 
            if (map[pos.x, pos.y].getColor() == opponentColor)
            {
                takenFigure = true;
                return true;
            }
            //else it must be empty - free to go
            return true;
        }

        internal Figure[,] CheckForCollision(Direction pattern, Position pos)
        {
            Figure[,] markerMap = new Figure[8, 8];
            Fill(markerMap, Figure.none);

            ChessColor myColor = map[pos.x, pos.y].getColor();
            ChessColor opponentColor = myColor == ChessColor.White ? ChessColor.Black : ChessColor.White;
            foreach (var dir in pattern.directions)
            {
                if (pattern.distance == 0)
                {
                    var newPos = pos + dir;
                    if (!CanMoveToPosition(newPos, myColor, out var takeFigure))
                        break;
                    markerMap[newPos.x, newPos.y] = Figure.moveMarker;
                    if (takeFigure)
                        break;

                }
                else
                {

                    for (int i = 1; i <= pattern.distance; i++)
                    {
                        var newPos = pos + new Position(dir.x * i, dir.y * i);
                        if (!CanMoveToPosition(newPos, myColor, out var takeFigure))
                            break;
                        markerMap[newPos.x, newPos.y] = Figure.moveMarker;
                        if (takeFigure)
                            break;

                    }
                }
            }


            return markerMap;
        }

        internal void excludeCheckMoves()
        {
            ;
        }

        internal bool underCheck()
        {
            return false;
        }
    }
}
