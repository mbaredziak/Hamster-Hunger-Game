using UnityEngine;
using TMPro;

namespace Game.MiniGames.HamsterGrab
{
    [RequireComponent(typeof(TextMeshPro))]
    public class ScorePopup : MonoBehaviour
    {
        public float riseSpeed = 1.5f;
        public float lifetime = 0.8f;
        public Color positiveColor = new Color(0.3f, 1f, 0.3f);
        public Color negativeColor = new Color(1f, 0.3f, 0.3f);
        public Color goldenColor   = new Color(1f, 0.85f, 0.2f);

        private TextMeshPro tmp;
        private float age;
        private Camera cam;

        public void Setup(int points)
        {
            tmp = GetComponent<TextMeshPro>();
            tmp.text = points > 0 ? $"+{points}" : points.ToString();
            tmp.color = points >= 50 ? goldenColor : points > 0 ? positiveColor : negativeColor;
        }

        private void Start()
        {
            cam = Camera.main;
            if (tmp == null) tmp = GetComponent<TextMeshPro>();
        }

        private void Update()
        {
            age += Time.deltaTime;
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            if (cam != null) transform.rotation = cam.transform.rotation;

            Color c = tmp.color;
            c.a = 1f - age / lifetime;
            tmp.color = c;

            if (age >= lifetime) Destroy(gameObject);
        }
    }
}
