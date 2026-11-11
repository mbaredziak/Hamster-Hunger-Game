using System;

namespace Game.Domain
{
    public class MatchState
    {
        public int TargetWins { get; }
        public int P1Wins { get; private set; }
        public int P2Wins { get; private set; }

        public MatchState(int targetWins)
        {
            if (targetWins <= 0) throw new ArgumentOutOfRangeException(nameof(targetWins));
            TargetWins = targetWins;
        }

        public bool IsOver => P1Wins >= TargetWins || P2Wins >= TargetWins;
        public int WinnerIndex => P1Wins >= TargetWins ? 0 : 1;

        public void AddWin(int playerIndex)
        {
            if (playerIndex == 0) P1Wins++;
            else if (playerIndex == 1) P2Wins++;
            else throw new ArgumentException("playerIndex musi być 0 lub 1", nameof(playerIndex));
        }
    }
}