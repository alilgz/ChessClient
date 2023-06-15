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
            map.map[4, 0] = Figure.bKing;
            map.map[3, 0] = Figure.bQueen;

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
            map.map[4, 7] = Figure.wKing;
            map.map[3, 7] = Figure.wQueen;

        }


        public void MoveFigure(Position currentFigurePos, Position pos)
        {

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
                case GameStage.WhiteWon:
                case GameStage.BlackWon:
                case GameStage.Tie:
                    refresh = true;
                    break;
                case GameStage.WhiteUpgrade:
                case GameStage.BlackUpgrade:
                    // here we convert click on chess map into menu click position ad pick figure to replace
                    if (IsValidUpgradeMenuItem(CurrentPlayer, pos))
                    {
                        UpgradeFigure(currentFigurePos, pos);
                        possibleMoves.Clear();
                        nextStage = CurrentStage.NextStage();
                        CurrentPlayer = (CurrentPlayer == ChessColor.White ? ChessColor.Black : ChessColor.White);
                        refresh = true;
                    }
                    break;
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
                            if (PawnMovedToLastLine(currentFigurePos, pos))
                            {
                                possibleMoves.Clear();
                                nextStage = CurrentStage == GameStage.WhiteMove ? GameStage.WhiteUpgrade : GameStage.BlackUpgrade;
                                break;
                            }
                            MoveFigure(currentFigurePos, pos);
                            possibleMoves.Clear();

                            if (map.AreKingUnderAttack(ChessColor.White) && map.HasNoMoves(ChessColor.White))
                                nextStage = GameStage.BlackWon;
                            else if (map.AreKingUnderAttack(ChessColor.Black) && map.HasNoMoves(ChessColor.Black))
                                nextStage = GameStage.WhiteWon;
                            else if (!map.AreKingUnderAttack(ChessColor.Black) && map.HasNoMoves(ChessColor.Black))
                                nextStage = GameStage.Tie;
                            else if (!map.AreKingUnderAttack(ChessColor.White) && map.HasNoMoves(ChessColor.White))
                                nextStage = GameStage.Tie;
                            else
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

        private void UpgradeFigure(Position currentFigurePos, Position pos)
        {
            var pawn = map[currentFigurePos];
            var color = pawn.GetColor();
            Figure upgraded = Figure.none;
            
            upgraded = FigureHelper.UpgradeMenu[pos.y];

            var newY = currentFigurePos.y == 1 ? 0 : 7;

            map[currentFigurePos] = Figure.none;
            map.map[pos.x, newY] = upgraded;
        }

        private bool IsValidUpgradeMenuItem(ChessColor player, Position pos)
        {
            // white q, kn, r, b, 
            // 0,0 - upper left , white
            // 7,7 lower, right , black
            
            return player == ChessColor.White && pos.y <= 3 || player == ChessColor.Black && pos.y >= 4;
        }

        private bool PawnMovedToLastLine(Position currentFigurePos, Position pos)
        {
            var currFigure = map[currentFigurePos];
            if (!currFigure.isPawn())
                return false;
            var color = currFigure.GetColor();

            return (color == ChessColor.White && pos.y == 0) || (color == ChessColor.Black && pos.y == 7);
        }
    }
}
