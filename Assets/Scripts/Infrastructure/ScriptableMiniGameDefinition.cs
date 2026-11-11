using UnityEngine;

namespace Game.Infrastructure
{
    [CreateAssetMenu(menuName = "Game/MiniGame Definition")]
    public class ScriptableMiniGameDefinition : ScriptableObject
    {
        public string DisplayName;
        [TextArea] public string Description;
        public string SceneName;
        public ScriptableInputScheme InputScheme;
        public Sprite Icon;
    }
}