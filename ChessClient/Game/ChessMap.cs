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
        public Figure this[Position index]
        {
            get => map[index.x, index.y];
            set
            {
                map[index.x, index.y] = value;
            }
        }

        internal bool SelectedFigureIsValid(Position pos, ChessColor currentPlayer, GameStage currentStage)
        {
            //check if there figure 
            // check if color related to stage
            // 
            if (!pos.IsValid() || this[pos] == Figure.none)
                return false;

            var figureColor = this[pos].GetColor();

            return (
                    (figureColor == ChessColor.White && currentPlayer == ChessColor.White && currentStage == GameStage.WhiteSelect) ||
                    (figureColor == ChessColor.White && currentPlayer == ChessColor.White && currentStage == GameStage.WhiteMove) ||
                    (figureColor == ChessColor.Black && currentPlayer == ChessColor.Black && currentStage == GameStage.BlackSelect) ||
                    (figureColor == ChessColor.Black && currentPlayer == ChessColor.Black && currentStage == GameStage.BlackMove)
                );
        }

        public ChessMap CalculateMoves(Position pos)
        {
            var piece = this[pos];
            return piece.getPossibleMoves(this, pos);
        }

        public bool Any => !Empty();
        public bool Empty()
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

        public bool CanMoveToPosition(Position pos, ChessColor myColor, out bool takenFigure, bool castling = false)
        {
            takenFigure = false;
            if (!pos.IsValid())
            {
                return false;
            }
            ChessColor opponentColor = myColor == ChessColor.White ? ChessColor.Black : ChessColor.White;


            // we cant move if there  our figure 
            if (this[pos].GetColor() == myColor && !castling) // todo: check for castle 
                return false;

            // we cant take opponent figure, but cant move after 
            if (this[pos].GetColor() == opponentColor)
            {
                takenFigure = true;
                return true;
            }
            //else it must be empty - free to go
            return true;
        }

        internal Figure[,] CheckForCollision(Direction pattern, Position pos, bool CanOnlyTake = false, bool CanOnlyMove = false, bool castling = false)
        {
            Figure[,] markerMap = new Figure[8, 8];
            Fill(markerMap, Figure.none);

            ChessColor myColor = map[pos.x, pos.y].GetColor();
            ChessColor opponentColor = myColor == ChessColor.White ? ChessColor.Black : ChessColor.White;
            foreach (var dir in pattern.directions)
            {
                if (pattern.distance == 0)
                {
                    var newPos = pos + dir;
                    if (!CanMoveToPosition(newPos, myColor, out var takeFigure, castling: castling))
                        continue;

                    if (CanOnlyMove && takeFigure)
                        continue;

                    if (CanOnlyTake && !takeFigure)
                        continue;

                    markerMap[newPos.x, newPos.y] = Figure.moveMarker;
                    if (takeFigure)
                        continue;

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

        internal bool HasNoMoves(ChessColor currentPlayer)
        {
            // there we check if specific side has at least one move out of check
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = this.map[i, j];
                    if (piece == Figure.none)
                        continue;
                    if (piece.GetColor() != currentPlayer)
                        continue;
                    var figurePos = new Position(i, j);
                    var moves = piece.getPossibleMoves(this, figurePos);
                    if (!moves.Empty())
                        return false;
                }
            }
            return true;
        }

        internal Figure[,] MergeMaps(Figure[,] moveMap2, Figure[,] moveMap1)
        {
            Figure[,] temp = new Figure[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    temp[i, j] = Figure.none;
                    temp[i, j] = moveMap1[i, j] == Figure.moveMarker || moveMap2[i, j] == Figure.moveMarker ? Figure.moveMarker : Figure.none;
                }
            }
            return temp;
        }

        internal void ExcludeCheckMoves(ChessMap map, Figure piece, Position currentPos)
        {
            // current map is move map
            // so we have to check every movement mark, if after move there no check state for us - we keep it 
            List<Position> posToClear = new List<Position>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (this.map[i, j] == Figure.moveMarker)
                    {
                        var newPosition = new Position(i, j);
                        ChessMap nextPos = map.GenerateNextPositionMap(piece, currentPos, newPosition);
                        if (nextPos.AreKingUnderAttack(piece.GetColor()))
                        {
                            posToClear.Add(newPosition);
                        }
                    }
                }
            }
            foreach (var pos in posToClear)
            {
                this.map[pos.x, pos.y] = Figure.none;
            }
        }

        private ChessMap GenerateNextPositionMap(Figure piece, Position currentPos, Position newPosition)
        {
            var newMap = new ChessMap();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    newMap.map[i, j] = this.map[i, j];
                }
            }
            newMap.map[currentPos.x, currentPos.y] = Figure.none;
            newMap.map[newPosition.x, newPosition.y] = piece;
            return newMap;
        }

        private Position GetKingPos(ChessColor kingColor)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (map[i, j] == Figure.bKing && kingColor == ChessColor.Black)
                        return new Position(i, j);
                    if (map[i, j] == Figure.wKing && kingColor == ChessColor.White)
                        return new Position(i, j);
                }
            }
            return null;
        }

        internal bool AreKingUnderAttack(ChessColor kingColor)
        {
            var kingPosition = this.GetKingPos(kingColor);
            if (kingPosition == null)
                return false;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = this.map[i, j];

                    if (piece == Figure.none)
                        continue;

                    if (piece == Figure.moveMarker)
                        continue;

                    if (piece.GetColor() != kingColor)
                    {
                        var pieceMoveMap = piece.getPossibleMoves(this, new Position(i, j), true);
                        if (pieceMoveMap.map[kingPosition.x, kingPosition.y] == Figure.moveMarker)
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
