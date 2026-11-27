using UnityEngine;

namespace Game.MiniGames.HamsterGrab
{
    public enum ItemType
    {
        Treat,
        Trap,
        GoldenTreat
    }

    [RequireComponent(typeof(Collider))]
    public class ConveyorItem : MonoBehaviour
    {
        [Header("Typ i punkty")]
        public ItemType type = ItemType.Treat;
        public int points = 10;

        [Header("Efekt wizualny (opcjonalnie)")]
        public float spinSpeed = 0f;

        [HideInInspector] public Vector3 moveDirection = Vector3.back;
        [HideInInspector] public float maxTravelDistance = 20f;

        private float travelled;

        private void Update()
        {
            var rm = HamsterGrabRound.Instance;
            if (rm == null || !rm.IsPlaying) return;

            float step = rm.CurrentSpeed * Time.deltaTime;
            transform.position += moveDirection.normalized * step;
            travelled += step;

            if (spinSpeed != 0f)
                transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);

            if (travelled >= maxTravelDistance)
                Destroy(gameObject);
        }
    }
}
