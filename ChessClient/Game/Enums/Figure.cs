using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClient.Game
{

    
    public enum Figure:byte { none=0, moveMarker,  bKing, bQueen, bPawn, bKnight, bBishop, bRook, wKing, wQueen, wPawn, wKnight, wBishop, wRook };
    
    public static class FigureHelper
    {
        
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
        public static ChessMap getPossibleMoves(this Figure piece, ChessMap map, Position pos)
        {
            Direction pattern =null; 
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
                    pattern = Direction.getBlackPawnDirection();
                    break;
                case Figure.wPawn:
                    pattern = Direction.getwhitePawnDirection();
                    break;
                default:
                    return null;
            }


            var moveMap = map.CheckForCollision(pattern, pos);
            var nextPositionMap = new ChessMap() { map = moveMap };
            if (nextPositionMap.underCheck())
            {
                nextPositionMap.excludeCheckMoves();
            }

            return nextPositionMap;
        }

    }
    
}
