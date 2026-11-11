using System.Collections;
using System.Collections.Generic;
using Game.Application;
using Game.Infrastructure;
using Game.Presentation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Composition
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private ScriptableMatchConfig _config;
        [SerializeField] private ScriptableMiniGameDefinition[] _definitions;
        [SerializeField] private LauncherUI _ui;

        private MatchFlow _flow;
        private string _currentScene;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            _flow = new MatchFlow(_config.TargetWins);
            _flow.PreRoundStarted += OnPreRoundStarted;
            _flow.RoundStarted += OnRoundStarted;
            _flow.ScoreChanged += OnScoreChanged;
            _flow.MatchFinished += OnMatchFinished;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void StartMatch() => _flow.Begin(BuildPool());

        public void ReportRoundResult(int winnerIndex) => StartCoroutine(CleanupAndReport(winnerIndex));

        private List<MiniGameEntry> BuildPool()
        {
            var list = new List<MiniGameEntry>(_definitions.Length);
            foreach (var d in _definitions)
            {
                if (d == null) continue;
                list.Add(new MiniGameEntry
                {
                    DisplayName = d.DisplayName,
                    Description = d.Description,
                    SceneName = d.SceneName,
                    P1Hint = d.InputScheme != null ? d.InputScheme.HintFor(0) : "?",
                    P2Hint = d.InputScheme != null ? d.InputScheme.HintFor(1) : "?"
                });
            }
            return list;
        }

        private void OnPreRoundStarted(MiniGameEntry entry) => StartCoroutine(PreRoundSequence(entry));

        private IEnumerator PreRoundSequence(MiniGameEntry entry)
        {
            _ui.PreRound.Show(entry);
            yield return new WaitForSeconds(_config.PreRoundSeconds);
            _ui.PreRound.Hide();

            yield return SceneManager.LoadSceneAsync(entry.SceneName, LoadSceneMode.Additive);
            _currentScene = entry.SceneName;

            _flow.ConfirmPreRound();
        }

        private void OnRoundStarted(MiniGameEntry entry) => _ui.Scoreboard.Show();

        private void OnScoreChanged(int p1, int p2) => _ui.Scoreboard.SetScore(p1, p2);

        private void OnMatchFinished(int winner) => _ui.Scoreboard.ShowPodium(winner);

        private IEnumerator CleanupAndReport(int winnerIndex)
        {
            if (!string.IsNullOrEmpty(_currentScene))
            {
                yield return SceneManager.UnloadSceneAsync(_currentScene);
                _currentScene = null;
            }
            _ui.Scoreboard.Hide();
            _flow.ReportResult(winnerIndex);
        }
    }
}