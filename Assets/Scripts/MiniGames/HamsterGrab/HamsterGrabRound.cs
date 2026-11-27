using System;
using Game.Composition;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Game.MiniGames.HamsterGrab
{
    public class HamsterGrabRound : MonoBehaviour
    {
        public static HamsterGrabRound Instance { get; private set; }

        public enum State { Countdown, Playing, Finished }

        [Header("Runda")]
        public float roundDuration = 45f;
        public float countdownTime = 3f;
        public bool allowNegativeScore = false;
        public float overtimeDuration = 10f;
        public int maxOvertimes = 3;
        public float resultScreenTime = 3f;

        [Header("Skalowanie trudności")]
        public float startSpeed = 4f;
        public float endSpeed = 10f;
        public float startSpawnInterval = 1.2f;
        public float endSpawnInterval = 0.45f;
        public AnimationCurve difficultyCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Dogrywka")]
        public float overtimeEndSpeed = 14f;
        public float overtimeEndSpawnInterval = 0.3f;

        [Header("UI")]
        public TMP_Text p1ScoreText;
        public TMP_Text p2ScoreText;
        public TMP_Text timerText;
        public TMP_Text centerText;

        [Header("Popup punktów")]
        public ScorePopup scorePopupPrefab;

        public event Action<int> OnRoundFinished;

        public State CurrentState { get; private set; }
        public bool IsPlaying => CurrentState == State.Playing;
        public bool IsOvertime { get; private set; }
        public float CurrentSpeed { get; private set; }
        public float CurrentSpawnInterval { get; private set; }
        public float TimeLeft { get; private set; }
        public int[] Scores { get; } = new int[2];

        private float countdownLeft;
        private float startTextHideTimer;
        private int overtimeCount;
        private float overtimeElapsed;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void Start()
        {
            CurrentState = State.Countdown;
            countdownLeft = countdownTime;
            TimeLeft = roundDuration;
            UpdateDifficulty(0f);
            RefreshUI();
        }

        private void Update()
        {
            switch (CurrentState)
            {
                case State.Countdown:
                    countdownLeft -= Time.deltaTime;
                    SetCenterText(Mathf.CeilToInt(countdownLeft).ToString());
                    if (countdownLeft <= 0f)
                    {
                        CurrentState = State.Playing;
                        ShowTemporaryText("START!");
                    }
                    break;

                case State.Playing:
                    TimeLeft -= Time.deltaTime;
                    if (IsOvertime)
                    {
                        overtimeElapsed += Time.deltaTime;
                        UpdateOvertimeDifficulty();
                    }
                    else
                    {
                        UpdateDifficulty(1f - TimeLeft / roundDuration);
                    }

                    if (startTextHideTimer > 0f)
                    {
                        startTextHideTimer -= Time.deltaTime;
                        if (startTextHideTimer <= 0f) SetCenterText("");
                    }

                    if (TimeLeft <= 0f)
                    {
                        TimeLeft = 0f;
                        OnTimeUp();
                    }
                    RefreshUI();
                    break;

                case State.Finished:
                    if (GameManager.Instance == null && HamsterGrabInput.RestartPressed())
                        SceneManager.LoadScene(gameObject.scene.name);
                    break;
            }
        }

        private void OnTimeUp()
        {
            bool tie = Scores[0] == Scores[1];

            if (tie && overtimeCount < maxOvertimes)
            {
                overtimeCount++;
                IsOvertime = true;
                TimeLeft = overtimeDuration;
                ShowTemporaryText("REMIS! DOGRYWKA!");
                return;
            }

            int winner = tie
                ? UnityEngine.Random.Range(0, 2)
                : (Scores[0] > Scores[1] ? 0 : 1);

            FinishRound(winner);
        }

        private void UpdateOvertimeDifficulty()
        {
            float total = Mathf.Max(0.01f, maxOvertimes * overtimeDuration);
            float k = Mathf.Clamp01(overtimeElapsed / total);
            CurrentSpeed = Mathf.Lerp(endSpeed, overtimeEndSpeed, k);
            CurrentSpawnInterval = Mathf.Lerp(endSpawnInterval, overtimeEndSpawnInterval, k);
        }

        private void UpdateDifficulty(float t01)
        {
            float k = difficultyCurve.Evaluate(Mathf.Clamp01(t01));
            CurrentSpeed = Mathf.Lerp(startSpeed, endSpeed, k);
            CurrentSpawnInterval = Mathf.Lerp(startSpawnInterval, endSpawnInterval, k);
        }

        public void AddScore(int playerIndex, int points, Vector3 worldPos)
        {
            if (!IsPlaying) return;

            Scores[playerIndex] += points;
            if (!allowNegativeScore && Scores[playerIndex] < 0)
                Scores[playerIndex] = 0;

            if (scorePopupPrefab != null)
            {
                var popup = Instantiate(scorePopupPrefab, worldPos + Vector3.up, Quaternion.identity);
                popup.Setup(points);
            }

            RefreshUI();
        }

        private void FinishRound(int winner)
        {
            CurrentState = State.Finished;

            bool standalone = GameManager.Instance == null;
            string hint = standalone ? "\n<size=50%>R - zagraj ponownie</size>" : "";
            SetCenterText($"WYGRYWA GRACZ {winner + 1}!{hint}");

            pendingWinner = winner;
            OnRoundFinished?.Invoke(winner);

            if (!standalone)
                Invoke(nameof(ReportToGameManager), resultScreenTime);
        }

        private int pendingWinner;

        private void ReportToGameManager()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.ReportRoundResult(pendingWinner);
        }

        private void ShowTemporaryText(string text)
        {
            SetCenterText(text);
            startTextHideTimer = 0.9f;
        }

        private void RefreshUI()
        {
            if (p1ScoreText) p1ScoreText.text = $"P1: {Scores[0]}";
            if (p2ScoreText) p2ScoreText.text = $"P2: {Scores[1]}";
            if (timerText) timerText.text = Mathf.CeilToInt(TimeLeft).ToString();
        }

        private void SetCenterText(string text)
        {
            if (centerText) centerText.text = text;
        }
    }
}