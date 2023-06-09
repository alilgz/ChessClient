using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClient.Game
{
    public class Direction
    {
        public List<Position> directions;
        public int distance; // 1 for king and pawn( ? patch later ? ), 0 for knight , 7 to queen, rook, bishop
        public bool CanBeAttacked = true;

        public static Direction getRookDirections()
        {
            return new Direction()
            {
                distance = 7,
                directions = new List<Position> {
                    new Position(0, -1),  //down
                    new Position(0, 1),  //up
                    new Position(-1, 0),  //left
                    new Position(1, 0),  //right

                }
            };
        }
        public static Direction getBishopDirections()
        {
            return new Direction()
            {
                distance = 7,
                directions = new List<Position> {
                    new Position(-1, -1),  //down-left
                    new Position(1, 1),  //up-right
                    new Position(-1, 1),  //
                    new Position(1, -1),  //

                }
            };
        }

        public static Direction getQueenDirections()
        {
            return new Direction()
            {
                distance = 7,
                directions = new List<Position> {
                    new Position(-1, -1),  //down-left
                    new Position(1, 1),  //up-right
                    new Position(-1, 1),  //
                    new Position(1, -1),  //
                    new Position(0, -1),  //down
                    new Position(0, 1),  //up
                    new Position(-1, 0),  //left
                    new Position(1, 0),  //right

                }
            };
        }

        public static Direction getKingDirections()
        {
            var dir = getQueenDirections();
            dir.distance = 1;
            dir.CanBeAttacked = false;
            return dir;
        }
        public static Direction getKnightDirections()
        {
            return new Direction()
            {
                distance = 0, // no multiply, exact coords
                directions = new List<Position> {
                    new Position(2, -1), 
                    new Position(2, 1),
                    new Position(-2, -1),
                    new Position(-2, 1),
                    new Position(1, -2),
                    new Position(-1, -2),
                    new Position(-1, 2),
                    new Position(1, 2),
                }
            };
        }

        public static Direction getBlackPawnDirection()
        {
            return new Direction()
            {
                distance = 0, 
                directions = new List<Position> {
                    new Position(0, 1),
                    new Position(0, 2),
                }
            };
        }

        public static Direction getwhitePawnDirection()
        {
            return new Direction()
            {
                distance = 0,
                directions = new List<Position> {
                    new Position(0, -1),
                    new Position(0, -2),
                }
            };
        }

    }
}
