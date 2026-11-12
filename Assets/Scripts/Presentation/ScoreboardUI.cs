using UnityEngine;
using UnityEngine.UI;

namespace Game.Presentation
{
    public class ScoreboardUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _podiumPanel;
        [SerializeField] private Text _p1ScoreText;
        [SerializeField] private Text _p2ScoreText;
        [SerializeField] private Text _winnerText;

        public void Show() => _panel.SetActive(true);
        public void Hide() => _panel.SetActive(false);

        public void SetScore(int p1, int p2)
        {
            _p1ScoreText.text = p1.ToString();
            _p2ScoreText.text = p2.ToString();
        }

        public void ShowPodium(int winnerIndex)
        {
            _panel.SetActive(false);
            _podiumPanel.SetActive(true);
            _winnerText.text = winnerIndex == 0
                ? "Wygrywa Gracz 1!"
                : "Wygrywa Gracz 2!";
        }
    }
}