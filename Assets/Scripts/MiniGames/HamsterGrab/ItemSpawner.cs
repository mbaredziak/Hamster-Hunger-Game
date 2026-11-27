using UnityEngine;

namespace Game.MiniGames.HamsterGrab
{
    [System.Serializable]
    public class SpawnEntry
    {
        public ConveyorItem prefab;
        public float weight = 1f;
    }

    public class ItemSpawner : MonoBehaviour
    {
        [Header("Punkty spawnu")]
        public Transform[] player1SpawnPoints = new Transform[3];
        public Transform[] player2SpawnPoints = new Transform[3];

        [Header("Przedmioty")]
        public SpawnEntry[] items;

        [Header("Ruch przedmiotów")]
        public Vector3 moveDirection = Vector3.back;
        public float maxTravelDistance = 17f;

        [Header("Ustawienia spawnu")]
        public bool samePatternForBoth = true;

        public float firstSpawnDelay = 0.5f;

        private float timer;

        private void Start()
        {
            timer = firstSpawnDelay;
        }

        private void Update()
        {
            var rm = HamsterGrabRound.Instance;
            if (rm == null || !rm.IsPlaying) return;

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                SpawnWave();
                timer = rm.CurrentSpawnInterval;
            }
        }

        private void SpawnWave()
        {
            int lane = Random.Range(0, 3);
            ConveyorItem prefab = PickRandomItem();

            Spawn(prefab, player1SpawnPoints[lane]);

            if (samePatternForBoth)
                Spawn(prefab, player2SpawnPoints[lane]);
            else
                Spawn(PickRandomItem(), player2SpawnPoints[Random.Range(0, 3)]);
        }

        private void Spawn(ConveyorItem prefab, Transform point)
        {
            if (prefab == null || point == null) return;

            ConveyorItem item = Instantiate(prefab, point.position, prefab.transform.rotation);
            item.moveDirection = moveDirection;
            item.maxTravelDistance = maxTravelDistance;
        }

        private ConveyorItem PickRandomItem()
        {
            float total = 0f;
            foreach (var e in items) total += Mathf.Max(0f, e.weight);
            if (total <= 0f) return null;

            float roll = Random.Range(0f, total);
            foreach (var e in items)
            {
                roll -= Mathf.Max(0f, e.weight);
                if (roll <= 0f) return e.prefab;
            }
            return items[items.Length - 1].prefab;
        }
    }
}
