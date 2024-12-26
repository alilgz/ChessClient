using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClient.Game
{

    public class ChessMove
    {
        // BoardHorizontal - Letter  a-h for white
        // BoardVertical - Number 1-8 a-h from white to black
        int x; // [0,0] is  A8 cuz arrays start from left/top
        int y;
        public String toString() // return chess name of coords 
        {
            return (x + 'A').ToString() + "" + (8 - y).ToString();
        }
    }
}
