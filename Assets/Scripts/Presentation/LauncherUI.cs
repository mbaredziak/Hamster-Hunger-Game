using Game.Composition;
using UnityEngine;

namespace Game.Presentation
{
    public class LauncherUI : MonoBehaviour
    {
        public PreRoundUI PreRound;
        public ScoreboardUI Scoreboard;

        [SerializeField] private GameObject[] _pages;
        [SerializeField] private GameObject _launcherPanel;
        [SerializeField] private GameManager _gameManager;

        public void SwitchPage(int pageIndex)
        {
            for (int i = 0; i < _pages.Length; i++)
                if (_pages[i] != null) _pages[i].SetActive(i == pageIndex);
        }

        public void OnPlayButtonClicked()
        {
            _launcherPanel.SetActive(false);
            _gameManager.StartMatch();
        }
    }
}