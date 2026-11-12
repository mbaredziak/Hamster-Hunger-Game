using UnityEngine;

namespace Game.Infrastructure
{
    public class PlayerInput
    {
        private readonly ScriptableInputScheme _scheme;
        private readonly int _index;

        public PlayerInput(ScriptableInputScheme scheme, int playerIndex)
        {
            _scheme = scheme;
            _index = playerIndex;
        }

        public bool Left => Input.GetKey(_index == 0 ? _scheme.P1Left : _scheme.P2Left);
        public bool Right => Input.GetKey(_index == 0 ? _scheme.P1Right : _scheme.P2Right);
        public bool Up => Input.GetKey(_index == 0 ? _scheme.P1Up : _scheme.P2Up);
        public bool Down => Input.GetKey(_index == 0 ? _scheme.P1Down : _scheme.P2Down);
        public bool Action => Input.GetKey(_index == 0 ? _scheme.P1Action : _scheme.P2Action);
        public bool ActionDown => Input.GetKeyDown(_index == 0 ? _scheme.P1Action : _scheme.P2Action);
    }
}