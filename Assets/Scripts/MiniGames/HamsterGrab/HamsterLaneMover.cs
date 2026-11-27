using UnityEngine;

namespace Game.MiniGames.HamsterGrab
{
    [RequireComponent(typeof(Collider))]
    public class HamsterLaneMover : MonoBehaviour
    {
        [Header("Gracz")]
        [Range(0, 1)] public int playerIndex = 0;

        [Header("Pasy")]
        public Transform[] lanes = new Transform[3];
        public int startLane = 1;

        [Header("Ruch")]
        public float hopSpeed = 25f;
        public float hopHeight = 0.3f;

        private int currentLane;
        private float baseY;

        private void Start()
        {
            currentLane = Mathf.Clamp(startLane, 0, lanes.Length - 1);
            baseY = transform.position.y;
            SnapToLane();
        }

        private void Update()
        {
            var rm = HamsterGrabRound.Instance;
            bool canMove = rm == null || rm.IsPlaying;

            if (canMove)
            {
                if (HamsterGrabInput.LeftPressed(playerIndex))  currentLane = Mathf.Max(0, currentLane - 1);
                if (HamsterGrabInput.RightPressed(playerIndex)) currentLane = Mathf.Min(lanes.Length - 1, currentLane + 1);
            }

            Vector3 pos = transform.position;
            float targetX = lanes[currentLane].position.x;
            float distance = Mathf.Abs(targetX - pos.x);

            pos.x = Mathf.MoveTowards(pos.x, targetX, hopSpeed * Time.deltaTime);
            pos.y = baseY + (distance > 0.01f ? Mathf.Min(distance, 1f) * hopHeight : 0f);
            transform.position = pos;
        }

        private void SnapToLane()
        {
            Vector3 pos = transform.position;
            pos.x = lanes[currentLane].position.x;
            transform.position = pos;
        }

        private void OnTriggerEnter(Collider other)
        {
            var item = other.GetComponentInParent<ConveyorItem>();
            if (item == null) return;

            var rm = HamsterGrabRound.Instance;
            if (rm == null || !rm.IsPlaying) return;

            rm.AddScore(playerIndex, item.points, item.transform.position);
            Destroy(item.gameObject);
        }
    }
}
