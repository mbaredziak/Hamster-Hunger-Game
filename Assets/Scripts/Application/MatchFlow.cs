using System;
using System.Collections.Generic;
using Game.Domain;

namespace Game.Application
{
    public class MatchFlow
    {
        private readonly int _targetWins;
        private readonly Random _rng = new Random();
        private IReadOnlyList<MiniGameEntry> _pool;
        private MiniGameEntry _last;

        public MatchState State { get; private set; }
        public MiniGameEntry Current { get; private set; }

        public event Action<MiniGameEntry> PreRoundStarted;
        public event Action<MiniGameEntry> RoundStarted;
        public event Action<int, int> ScoreChanged;
        public event Action<int> MatchFinished;

        public MatchFlow(int targetWins)
        {
            if (targetWins <= 0) throw new ArgumentOutOfRangeException(nameof(targetWins));
            _targetWins = targetWins;
        }

        public void Begin(IReadOnlyList<MiniGameEntry> pool)
        {
            if (pool == null || pool.Count == 0)
                throw new ArgumentException("Pula minigier jest pusta.", nameof(pool));

            _pool = pool;
            _last = null;
            State = new MatchState(_targetWins);
            ScoreChanged?.Invoke(0, 0);
            GoToNextRound();
        }

        public void ConfirmPreRound() => RoundStarted?.Invoke(Current);

        public void ReportResult(int winnerIndex)
        {
            State.AddWin(winnerIndex);
            ScoreChanged?.Invoke(State.P1Wins, State.P2Wins);

            if (State.IsOver) MatchFinished?.Invoke(State.WinnerIndex);
            else GoToNextRound();
        }

        private void GoToNextRound()
        {
            Current = PickNext();
            _last = Current;
            PreRoundStarted?.Invoke(Current);
        }

        private MiniGameEntry PickNext()
        {
            if (_pool.Count == 1) return _pool[0];

            MiniGameEntry candidate;
            int guard = 0;
            do { candidate = _pool[_rng.Next(_pool.Count)]; }
            while (candidate.SceneName == _last?.SceneName && ++guard < 16);
            return candidate;
        }
    }
}