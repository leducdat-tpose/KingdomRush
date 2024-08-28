using UnityEngine;

namespace Assets.DigitalSoftware.zFPS.DemoScene.Scripts
{
    public class RotateAroundY : MonoBehaviour
    {
        [SerializeField] private float _speed = 10.0f;

        private Transform _transform;

        // Start is called before the first frame update
        void Start()
        {
            _transform = this.transform;
        }

        // Update is called once per frame
        void Update()
        {
            _transform.Rotate(new Vector3(0f, _speed * Time.deltaTime, 0f));
        }
    }
}
