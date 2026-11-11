using UnityEngine;

namespace Game.Infrastructure
{
    [CreateAssetMenu(menuName = "Game/Input Scheme")]
    public class ScriptableInputScheme : ScriptableObject
    {
        [Header("Player 1")]
        public KeyCode P1Left = KeyCode.A;
        public KeyCode P1Right = KeyCode.D;
        public KeyCode P1Up = KeyCode.W;
        public KeyCode P1Down = KeyCode.S;
        public KeyCode P1Action = KeyCode.Space;

        [Header("Player 2")]
        public KeyCode P2Left = KeyCode.LeftArrow;
        public KeyCode P2Right = KeyCode.RightArrow;
        public KeyCode P2Up = KeyCode.UpArrow;
        public KeyCode P2Down = KeyCode.DownArrow;
        public KeyCode P2Action = KeyCode.Return;

        public string HintFor(int player)
        {
            return player == 0
                ? $"{P1Left}/{P1Right}/{P1Up}/{P1Down} + {P1Action}"
                : $"{P2Left}/{P2Right}/{P2Up}/{P2Down} + {P2Action}";
        }
    }
}