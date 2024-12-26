using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClient.Game
{
    public struct CastlingStatus
    {
        public bool Done;
        public bool KingMoved;
        public bool LeftRookMoved;
        public bool RightRookMoved;

        public static CastlingStatus getCastling()
        {
            return new CastlingStatus() { Done = false, KingMoved = false, LeftRookMoved = false, RightRookMoved = false };
        }
        public bool PossibleLeft() => !Done && !KingMoved && !LeftRookMoved;
        public bool PossibleRight() => !Done && !KingMoved && !RightRookMoved;

    }
}