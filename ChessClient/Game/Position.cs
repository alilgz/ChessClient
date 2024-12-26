using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClient.Game
{
    public class Position
    {

        public int x;
        public int y;

        public Position()
        {

        }

        public Position(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
        public bool IsValid()
        {
            return 0 <= x && x <= 7 && 0 <= y & y <= 7;
        }
        public static Position operator +(Position a, Position b)
        {
            return new Position(b.x + a.x, b.y + a.y);
        }

        public override bool Equals(Object? b)
        {
            return b != null && b is Position && ((Position)b).x == x && ((Position)b).y == y;
        }

        public string ToString(bool onlyX = false)
        {
            if (!onlyX) return ToString();
            return (x + 'A').ToString();
        }
        public override string ToString()
        {
            return (x + 'A').ToString() + " " +(8-y).ToString();
        }

    }
}
