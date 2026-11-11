using UnityEngine;

namespace Game.Infrastructure
{
    [CreateAssetMenu(menuName = "Game/Match Config")]
    public class ScriptableMatchConfig : ScriptableObject
    {
        [Range(1, 10)] public int TargetWins = 3;
        [Range(0f, 10f)] public float PreRoundSeconds = 3f;
    }
}