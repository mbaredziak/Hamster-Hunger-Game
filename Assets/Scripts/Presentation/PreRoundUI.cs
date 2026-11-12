using Game.Application;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Presentation
{
    public class PreRoundUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _descriptionText;
        [SerializeField] private Text _p1HintText;
        [SerializeField] private Text _p2HintText;

        public void Show(MiniGameEntry entry)
        {
            _panel.SetActive(true);
            _nameText.text = entry.DisplayName;
            _descriptionText.text = entry.Description;
            _p1HintText.text = "P1: " + entry.P1Hint;
            _p2HintText.text = "P2: " + entry.P2Hint;
        }

        public void Hide() => _panel.SetActive(false);
    }
}