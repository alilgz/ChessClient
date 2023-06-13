using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClient.Game
{


    public enum Figure : byte { none = 0, moveMarker = 1, bKing = 2, bQueen = 3, bPawn = 4, bKnight = 5, bBishop = 6, bRook = 7, wKing = 8, wQueen = 9, wPawn = 10, wKnight = 11, wBishop = 12, wRook = 13 };

    public static class FigureHelper
    {

        public static bool isPawn(this Figure piece)
        {
            return piece == Figure.bPawn || piece == Figure.wPawn;
        }

        public static ChessColor getColor(this Figure piece)
        {
            switch (piece)
            {
                case Figure.bKing:
                case Figure.bQueen:
                case Figure.bKnight:
                case Figure.bBishop:
                case Figure.bPawn:
                case Figure.bRook:
                    return ChessColor.Black;

                case Figure.wKing:
                case Figure.wQueen:
                case Figure.wKnight:
                case Figure.wRook:
                case Figure.wBishop:
                case Figure.wPawn:
                    return ChessColor.White;

                case Figure.none:
                default:
                    return ChessColor.Empty;
            }
        }
        public static ChessMap getPossibleMoves(this Figure piece, ChessMap map, Position pos, bool skipUnderCheck = false)
        {
            Direction pattern = null;
            Direction patternAttack = null;
            Direction patternMove = null;
            pattern = getFigureDirections(piece, pos);
            if (piece.isPawn())
            {
                patternAttack = getFigureDirections(piece, pos, attack: true);
                patternMove = getFigureDirections(piece, pos, move: true);
            }

            Figure[,] moveMap = null;
            if (patternAttack != null && patternMove != null)
            {
                var moveMap1 = map.CheckForCollision(patternAttack, pos, CanOnlyTake: true);
                var moveMap2 = map.CheckForCollision(patternMove, pos, CanOnlyMove: true);
                moveMap = map.MergeMaps(moveMap2, moveMap1);
            }
            else
            {
                moveMap = map.CheckForCollision(pattern, pos);
            }

            var nextPositionMap = new ChessMap() { map = moveMap };
            if (!skipUnderCheck)
            {
                
                if (map.underCheck(piece.getColor()))
                {
                    nextPositionMap.excludeCheckMoves(map, piece, pos);
                }
            }

            return nextPositionMap;
        }

        private static Direction getFigureDirections(Figure piece, Position pos, bool attack = false, bool move = false)
        {
            Direction pattern = null;
            switch (piece)
            {
                case Figure.bKing:
                case Figure.wKing:
                    pattern = Direction.getKingDirections();
                    break;

                case Figure.bQueen:
                case Figure.wQueen:
                    pattern = Direction.getQueenDirections();
                    break;

                case Figure.bKnight:
                case Figure.wKnight:
                    pattern = Direction.getKnightDirections();
                    break;
                case Figure.bBishop:
                case Figure.wBishop:
                    pattern = Direction.getBishopDirections();
                    break;

                case Figure.bRook:
                case Figure.wRook:
                    pattern = Direction.getRookDirections();
                    break;

                case Figure.bPawn:
                    if (move)
                    {
                        pattern = Direction.getBlackPawnDirection(false);
                        if (pos.y != 1)
                        {
                            pattern.directions.RemoveAt(1);
                        }
                    }
                    if (attack)
                    {
                        pattern = Direction.getBlackPawnDirection(true);
                    }
                    break;

                case Figure.wPawn:
                    if (move)
                    {
                        pattern = Direction.getwhitePawnDirection(false);
                        if (pos.y != 6)
                        {
                            pattern.directions.RemoveAt(1);
                        }
                    }
                    if (attack)
                    {
                        pattern = Direction.getwhitePawnDirection(true);
                    }
                    break;

                default:
                    return null;
            }

            return pattern;
        }
    }

}
