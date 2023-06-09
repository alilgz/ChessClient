using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClient.Game
{

    public enum GameStage { WhiteSelect, WhiteMove, BlackSelect, BlackMove, BlackWon, WhiteWon, Tie };
    public static class StageHelper
    {
        public static GameStage NextStage(this GameStage stage)
        {
            switch (stage)
            {
                case GameStage.WhiteSelect:
                    return GameStage.WhiteMove;
                case GameStage.BlackSelect:
                    return GameStage.BlackMove;
                case GameStage.BlackMove:
                    return GameStage.WhiteSelect;
                case GameStage.WhiteMove:
                    return GameStage.BlackSelect;
            }
            return GameStage.WhiteSelect;
        }
        public static GameStage PreviousStage(this GameStage stage)
        {
            switch (stage)
            {
                case GameStage.BlackMove:
                    return GameStage.BlackSelect;
                case GameStage.WhiteMove:
                    return GameStage.WhiteSelect;
            }
            return stage;
        }


        

    }

}
