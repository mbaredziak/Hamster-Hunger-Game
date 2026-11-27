using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Game.MiniGames.HamsterGrab
{
    public static class HamsterGrabInput
    {
        public static bool LeftPressed(int playerIndex)
        {
    #if ENABLE_INPUT_SYSTEM
            var kb = Keyboard.current;
            if (kb == null) return false;
            return playerIndex == 0 ? kb.aKey.wasPressedThisFrame : kb.leftArrowKey.wasPressedThisFrame;
    #else
            return Input.GetKeyDown(playerIndex == 0 ? KeyCode.A : KeyCode.LeftArrow);
    #endif
        }

        public static bool RightPressed(int playerIndex)
        {
    #if ENABLE_INPUT_SYSTEM
            var kb = Keyboard.current;
            if (kb == null) return false;
            return playerIndex == 0 ? kb.dKey.wasPressedThisFrame : kb.rightArrowKey.wasPressedThisFrame;
    #else
            return Input.GetKeyDown(playerIndex == 0 ? KeyCode.D : KeyCode.RightArrow);
    #endif
        }

        public static bool RestartPressed()
        {
    #if ENABLE_INPUT_SYSTEM
            var kb = Keyboard.current;
            return kb != null && kb.rKey.wasPressedThisFrame;
    #else
            return Input.GetKeyDown(KeyCode.R);
    #endif
        }
    }
}
