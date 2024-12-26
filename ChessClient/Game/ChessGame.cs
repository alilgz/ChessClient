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
        private Figure lastTakenFigure = Figure.none; 
        private CastlingStatus whiteCastle = CastlingStatus.getCastling();
        private CastlingStatus blackCastle = CastlingStatus.getCastling();
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

        public void MoveFigure(Position currentPos, Position newPos, out Figure takenFigure)
        {
            takenFigure = map[newPos];
            var ourFigure = map[currentPos];
            map[currentPos] = Figure.none; // todo : castle  ?
            map[newPos] = ourFigure;
        }

        public void OnClick(Position pos, out bool refresh)
        {
            refresh = false;
            var nextStage = CurrentStage;
            switch (CurrentStage)
            {
                /* someone won or tie: there will be message */
                case GameStage.WhiteWon:
                case GameStage.BlackWon:
                case GameStage.Tie:
                    refresh = true;
                    break;
                /* click on menu to figure upgrade  */
                case GameStage.WhiteUpgrade:
                case GameStage.BlackUpgrade:
                    // convert click on chess map into menu click position and pick figure to replace
                    if (IsValidUpgradeMenuItem(CurrentPlayer, pos))
                    {
                        UpgradeFigure(currentFigurePos, pos);
                        possibleMoves.Clear();
                        nextStage = CheckIfGameDone(out bool cw, out bool cb);
                        refresh = true;
                        lastTakenFigure = map[pos];
                        var isKingAttacked = CurrentPlayer == ChessColor.White ? cw : cb;
                        var isGameWon = CurrentPlayer == ChessColor.White ? nextStage == GameStage.WhiteWon : nextStage == GameStage.BlackWon;

                        AddMove(ChessFigureMove.UpgradeMove(map, currentFigurePos, pos, lastTakenFigure, isKingAttacked, isGameWon));
                        CurrentPlayer = (CurrentPlayer == ChessColor.White ? ChessColor.Black : ChessColor.White);
                    }
                    break;
                    // if valid figure  selected - paint markers where it can be moved
                case GameStage.WhiteSelect:
                case GameStage.BlackSelect:
                    if (map.SelectedFigureIsValid(pos, CurrentPlayer, CurrentStage))
                    {
                        possibleMoves = map.CalculateMoves(pos);
                        if (possibleMoves.Empty())
                            break;

                        nextStage = CurrentStage.NextStage();
                        currentFigurePos = pos;
                        refresh = true;
                    }
                    break;
                // move figure to new pos, save taked figure(todo), save move in list 
                // if same figure seleted - back to 'select mode'
                // todo: if same color figure selected (and its not castle) - back to 'select mode'
                case GameStage.WhiteMove:
                case GameStage.BlackMove:
                    UpdateCastlingStatus(currentFigurePos); // or we can replace this logic with checking list of moves - we can easy find if specific figures moved
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
                            MoveFigure(currentFigurePos, pos, out lastTakenFigure);
                            possibleMoves.Clear();
                            nextStage = CheckIfGameDone(out bool cw, out bool cb);
                            var isKingAttacked = CurrentPlayer == ChessColor.White ? cw : cb;
                            var isGameWon = CurrentPlayer == ChessColor.White ? nextStage == GameStage.WhiteWon : nextStage == GameStage.BlackWon;
                            AddMove(ChessFigureMove.Move(map, currentFigurePos, pos, lastTakenFigure, isKingAttacked, isGameWon));

                            CurrentPlayer = (CurrentPlayer == ChessColor.White ? ChessColor.Black : ChessColor.White);
                            refresh = true;
                        } 
                    }

                    //checkForCheck ?

                    break;
            }
            CurrentStage = nextStage;
        }

        private void UpdateCastlingStatus(Position pos)
        {
            var figure = map[pos];
            // todo: improve, unify for black             
            if (CurrentPlayer == ChessColor.White)
            {
                if (!whiteCastle.PossibleLeft() && !whiteCastle.PossibleRight())
                    return;

                if (figure.isKing())
                    whiteCastle.KingMoved = true;
                if (figure.isRook() && pos.x ==0)
                    whiteCastle.LeftRookMoved= true;
                if (figure.isRook() && pos.x == 7)
                    whiteCastle.RightRookMoved = true;
            }
        }

        private void AddMove(ChessFigureMove move)
        {
          moves.Add(move);
        }

        public GameStage CheckIfGameDone(out  bool whiteKingAttacked, out bool blackKingAttacked)
        {
            blackKingAttacked = map.AreKingUnderAttack(ChessColor.Black);
            whiteKingAttacked = map.AreKingUnderAttack(ChessColor.White);

            if (whiteKingAttacked && map.HasNoMoves(ChessColor.White))
                return GameStage.BlackWon;
            else if (blackKingAttacked && map.HasNoMoves(ChessColor.Black))
                return GameStage.WhiteWon;
            else if (!blackKingAttacked && map.HasNoMoves(ChessColor.Black))
                return GameStage.Tie;
            else if (!whiteKingAttacked && map.HasNoMoves(ChessColor.White))
                return GameStage.Tie;
            else
                return CurrentStage.NextStage();
        }

        private void UpgradeFigure(Position currentFigurePos, Position pos)
        {
            Figure upgraded =  FigureHelper.UpgradeMenu[pos.y];
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
