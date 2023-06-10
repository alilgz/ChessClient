using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClient.Game
{
    public class ChessGame
    {

        public ChessMap map;
        public ChessMap possibleMoves;
        public List<ChessFigureMove> moves;
        public ChessColor CurrentPlayer;
        public GameStage CurrentStage;
        public Position currentFigurePos;

        public ChessGame()
        {
            map = new ChessMap();
            possibleMoves = new ChessMap();
            moves = new List<ChessFigureMove>();
            CurrentPlayer = ChessColor.White;
            CurrentStage = GameStage.WhiteSelect;
            currentFigurePos = null;
        }
        public void Restart()
        {
            map = new ChessMap();
            map.map[0, 0] = Figure.bRook;
            map.map[7, 0] = Figure.bRook;
            map.map[1, 0] = Figure.bKnight;
            map.map[6, 0] = Figure.bKnight;
            map.map[2, 0] = Figure.bBishop;
            map.map[5, 0] = Figure.bBishop;
            map.map[3, 0] = Figure.bKing;
            map.map[4, 0] = Figure.bQueen;

            map.map[0, 1] = Figure.bPawn;
            map.map[1, 1] = Figure.bPawn;
            map.map[2, 1] = Figure.bPawn;
            map.map[3, 1] = Figure.bPawn;
            map.map[4, 1] = Figure.bPawn;
            map.map[5, 1] = Figure.bPawn;
            map.map[6, 1] = Figure.bPawn;
            map.map[7, 1] = Figure.bPawn;

            map.map[0, 6] = Figure.wPawn;
            map.map[1, 6] = Figure.wPawn;
            map.map[2, 6] = Figure.wPawn;
            map.map[3, 6] = Figure.wPawn;
            map.map[4, 6] = Figure.wPawn;
            map.map[5, 6] = Figure.wPawn;
            map.map[6, 6] = Figure.wPawn;
            map.map[7, 6] = Figure.wPawn;

            map.map[0, 7] = Figure.wRook;
            map.map[7, 7] = Figure.wRook;
            map.map[1, 7] = Figure.wKnight;
            map.map[6, 7] = Figure.wKnight;
            map.map[2, 7] = Figure.wBishop;
            map.map[5, 7] = Figure.wBishop;
            map.map[3, 7] = Figure.wKing;
            map.map[4, 7] = Figure.wQueen;

        }


        public void MoveFigure(Position currentFigurePos, Position pos) {

            var figure = map.map[currentFigurePos.x, currentFigurePos.y];
            map.map[currentFigurePos.x, currentFigurePos.y] = Figure.none; // todo : taken and castle 
            map.map[pos.x, pos.y] = figure;

        }

        public void OnClick(Position pos, out bool refresh)
        {
            refresh = false;
            var nextStage = CurrentStage;
            switch (CurrentStage)
            {
                case GameStage.WhiteSelect:
                case GameStage.BlackSelect:
                    if (map.SelectedFigureIsValid(pos, CurrentPlayer, CurrentStage))
                    {
                        possibleMoves = map.CalculateMoves(pos);
                        if (!possibleMoves.Empty())
                        {
                            nextStage = CurrentStage.NextStage();
                            currentFigurePos = pos;
                            refresh = true;
                        }
                    }
                    break;
                case GameStage.WhiteMove:
                case GameStage.BlackMove:
                    if (pos.Equals(currentFigurePos))
                    {
                        //redo - same figure selected
                        nextStage = CurrentStage.PreviousStage();
                        possibleMoves.Clear();
                        refresh = true;
                        break;
                    }
                    else
                    {
                        if (possibleMoves.map[pos.x, pos.y] == Figure.moveMarker)
                        {
                            MoveFigure(currentFigurePos, pos);
                            possibleMoves.Clear();
                            nextStage = CurrentStage.NextStage();
                            CurrentPlayer = (CurrentPlayer == ChessColor.White ? ChessColor.Black : ChessColor.White);
                            refresh = true;
                        }
                        else
                        {
                            // do nothing 
                        }
                    }

                    //checkForCheck ?

                    break;
            }
            CurrentStage = nextStage;
        }
    }
}
